using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class Parser
	{
		public CommandsFile Parse(string file, Type targetClass)
		{
			CommandFileReader reader = new CommandFileReader(file);
			CommandsFile result = new CommandsFile(5);

			try
			{
				Validator.Validate(reader, new int[] { 5 });
			}
			catch (Exception e)
			{
				Console.Write(e);
				GameConsole.Error(e);
				return null;
			}
			reader.Reset();
			string line = null;
			while(!reader.IsDone)
			{
				line = reader.NextLine();
				if (line.StartsWith("#")) continue;
				if (Regex.IsMatch(line, RegexCommand))
				{
					result.Add(ParseCommand(reader.Copy(), result, targetClass, out int skip));
					reader.Skip(skip);
					if (!reader.IsDone)
						reader.Back();
				}
				else throw new Exception();
			}

			result.Load();
			return result;
		}

		public ConsoleCommand ParseCommand(CommandFileReader reader, CommandsFile commandsFile, Type targetClass, out int skip)
		{
			ConsoleCommand result = new ConsoleCommand();

			string line = reader.NextLine();
			if(Regex.IsMatch(line, RegexCommand))
			{
				result.Names = Regex.Match(line, @"\(" + CreateMulitpleRegex(RegexVariable) + @"\)").Value
								.Trim('(', ')').Split(',').Select(s => s.Trim()).ToArray();
			}
			while(!reader.IsDone)
			{
				line = reader.NextLine();

				if (Regex.IsMatch(line, RegexParameter))
				{
					result.Add(ParseParameter(reader.Copy(), commandsFile, out int _skip));
					reader.Skip(_skip);
					if(!reader.IsDone)
					reader.Back();
				}
				else if (Regex.IsMatch(line, RegexExample))
				{
					result.Add(ParseExample(reader.Copy(), commandsFile, out int _skip));
					reader.Skip(_skip);
					if (!reader.IsDone)
						reader.Back();
				}
				else if (Regex.IsMatch(line, RegexCommandContents))
				{
					string value = line.Substring(line.IndexOf(':') + 1).Trim();
					result.SetCallback(targetClass.GetMethod(value));
				}
				else if (EndsBlock(line))
					break;
				else throw new Exception();
			}

			skip = reader.Position - reader.Start - 1;
			return result;
		}

		public Parameter ParseParameter(CommandFileReader reader, CommandsFile commandsFile, out int skip)
		{
			Parameter result = new Parameter();

			string line = reader.NextLine();
			if (Regex.IsMatch(line, RegexParameter))
			{
				result.Names = Regex.Match(line, @"\(" + CreateMulitpleRegex(RegexVariable) + @"\)").Value
								.Trim('(', ')').Split(',').Select(s => s.Trim()).ToArray();
			}
			while (!reader.IsDone)
			{
				line = reader.NextLine();

				if (Regex.IsMatch(line, RegexParameterType))
				{
					string value = line.Substring(line.IndexOf(':') + 1).Trim();
					switch (value)
					{
						case "bool":
							result.Type = ParameterType.Boolean;
							break;
						case "string":
							result.Type = ParameterType.String;
							break;
						case "file":
							result.Type = ParameterType.File;
							break;
						case "int":
							result.Type = ParameterType.Integer;
							break;
						case "float":
							result.Type = ParameterType.Float;
							break;
						case "command":
							result.Type = ParameterType.Command;
							break;
						default:
							throw new Exception("Can't happen because of Regex above.");
					}
				}
				else if (Regex.IsMatch(line, RegexParameterFlags))
				{
					string[] values = line.Substring(line.IndexOf(':') + 1).Trim().Split(',').Select(v => v.Trim()).ToArray();
					foreach (var value in values)
					{
						switch (value)
						{
							case "short":
								result.HasShort = true;
								break;
							case "exclusive":
								result.SetOthers(null);
								break;
							case "meta":
								result.IsMeta = true;
								break;
							default:
								throw new NotImplementedException("No Flag with this name known: " + value);
						}
					}
				}
				else if (Regex.IsMatch(line, RegexParameterOthers))
				{
					string[] others = line.Substring(line.IndexOf(':') + 1).Trim().Split(',').Select(o => o.Trim()).ToArray();
					commandsFile.Add(result, others);
				}
				else if (EndsBlock(line))
					break;
				else throw new Exception();
			}

			skip = reader.Position - reader.Start - 1;
			return result;
		}

		public CommandExample ParseExample(CommandFileReader reader, CommandsFile commandsFile, out int skip)
		{
			CommandExample result = new CommandExample();

			string line = reader.NextLine();
			if (Regex.IsMatch(line, RegexExample))
			{
				string[] used = Regex.Match(line, @"\(" + CreateMulitpleRegex(RegexVariable) + @"\)").Value
								.Trim('(', ')').Split(',').Select(s => s.Trim()).ToArray();
				if(used.Length > 0 && used[0] != string.Empty)
					commandsFile.Add(result, used);
			}
			while (!reader.IsDone)
			{
				line = reader.NextLine();

				if (Regex.IsMatch(line, RegexExampleExplanation))
				{
					string value = line.Substring(line.IndexOf(':') + 1).Trim();
					result.AddExplanation(value);
				}
				else if (Regex.IsMatch(line, RegexExampleLine))
				{
					string value = line.Substring(line.IndexOf(':') + 1).Trim();
					result.AddLine(value);
				}
				else if (EndsBlock(line))
					break;
				else throw new Exception();
			}
			skip = reader.Position - reader.Start - 1;
			return result;
		}

		private bool EndsBlock(string line)
		{
			return line.StartsWith("cmd") ||
				line.StartsWith("parameter") ||
				line.StartsWith("metaparameter") ||
				line.StartsWith("example");
		}


		//TODO: Use string.Format();
		private static string RegexVariable => "[a-zA-Z][a-zA-Z_0-9]*";
		private static string RegexCommand => @"cmd\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\):";
		private static string RegexParameter => CreateOptionRegex("parameter", "metaparameter") + @"\(" + CreateMulitpleRegex(RegexVariable) + @"\):";
		private static string RegexExample => @"example\(" + CreateOptionRegex(RegexVariable + @"(\s*,\s*" + RegexVariable + @")*", "#empty") + @"\):";
		private static string RegexCommandContents => @"callback:\s?" + RegexVariable;
		private static string RegexParameterContents => CreateOptionRegex(
					RegexParameterFlags,
					RegexParameterType,
					RegexParameterOthers
				);
		private static string RegexParameterFlags =>
			CreateKeyValueRegex("flags", CreateMulitpleRegex(CreateOptionRegex("short", "exclusive")));
		private static string RegexParameterType =>
			CreateKeyValueRegex("type", CreateOptionRegex("bool", "string", "file", "int", "float", "command"));
		private static string RegexParameterOthers =>
			CreateRegex("others", true);
		private static string RegexExampleContents => CreateOptionRegex(
					RegexExampleLine,
					RegexExampleExplanation
				);
		private static string RegexExampleLine =>
					CreateRegex("line", false, true);
		private static string RegexExampleExplanation =>
					CreateRegex(CreateOptionRegex("exp", "explanation"), false, true);

		private static string CreateRegex(string name, bool multiple = false, bool allowStringValues = false)
		{
			string value = "[a-zA-Z][a-zA-Z_0-9]*";
			if (allowStringValues)
				value = ".*";
			if (multiple)
				value = CreateMulitpleRegex(value);
			return CreateKeyValueRegex(name, value);
		}

		private static string CreateMulitpleRegex(string value)
		{
			return value + @"(\s*,\s*" + value + ")*";
		}

		private static string CreateKeyValueRegex(string key, string value)
		{
			return key + ":\\s*" + value;
		}

		private static string CreateOptionRegex(params string[] options)
		{
			return "(" + string.Join("|", options) + ")";
		}
	}
}
