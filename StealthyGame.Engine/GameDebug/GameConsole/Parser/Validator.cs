using StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	class Validator
	{
		private static bool EndsBlock(string line)
		{
			return line.StartsWith("cmd") ||
				line.StartsWith("parameter") ||
				line.StartsWith("metaparameter") ||
				line.StartsWith("example");
		}
		
		public static void Validate(CommandFileReader reader, int[] supportedVersions)
		{
			reader.Reset();
			CheckVersion(reader, supportedVersions);
			reader.Reset();
			FileStructure structure = CollectStructure(reader);
			ValidateStructure(structure);
		}

		private static void CheckVersion(CommandFileReader reader, int[] supportedVersions)
		{
			string line = null;
			CommandFileLine lastChecked = null;
			while(!reader.IsDone)
			{
				line = reader.NextLine();

				if(line.StartsWith("#ver"))
				{
					if (lastChecked!= null)
						throw new MultipleDeclarationException(lastChecked, reader.GetLineObject());
					lastChecked = reader.GetLineObject();
					if (int.TryParse(line.Substring("#ver".Length), out int version))
					{
						if (!supportedVersions.Contains(version))
							throw new WrongVersionException(supportedVersions, version);
					}
					else throw new PositionException(lastChecked,
						new MalformedCodeException(CreateVersionRegex(supportedVersions)));
				}
			}
		}

		private static FileStructure CollectStructure(CommandFileReader reader)
		{
			FileStructure result = new FileStructure();

			string line = null;
			while(!reader.IsDone)
			{
				line = reader.NextLine();
				if (line.StartsWith("#")) continue;
				if (Regex.IsMatch(line, RegexCommand))
				{
					result.Add(CollectCommandStructure(reader.Copy(), out int skip));
					reader.Skip(skip);
					if (!reader.IsDone)
						reader.Back();
				}
				else
					throw new PositionException(reader.GetLineObject(),
						new MalformedCodeException(RegexCommand));
			}
			return result;
		}
		private static CommandStructure CollectCommandStructure(CommandFileReader reader, out int skip)
		{
			CommandStructure result = null;

			string line = reader.NextLine();
			if (Regex.IsMatch(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)"))
			{
				string firstName = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
					.Trim('(', ')').Split(',')[0].Trim();
				result = new CommandStructure(firstName);
			}
			else throw new PositionException(reader.GetLineObject(), 
				new MalformedCodeException(@"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)"));
			while(!reader.IsDone)
			{
				line = reader.NextLine();
				if (Regex.IsMatch(line, RegexParameter))
				{
					result.Add(CollectParameterStructure(reader.Copy(), result, out int _skip));
					reader.Skip(_skip);
					if(!reader.IsDone)
					reader.Back();
				}
				else if (Regex.IsMatch(line, RegexExample))
				{
					result.Add(CollectExampleStructure(reader.Copy(), out int _skip));
					reader.Skip(_skip);
					if (!reader.IsDone)
						reader.Back();
				}
				else if (Regex.IsMatch(line, RegexCommandContents))
				{
				}
				else if (EndsBlock(line))
					break;
				else throw new PositionException(reader.GetLineObject(),
					new MalformedCodeException(RegexParameter, RegexExample, RegexCommandContents));

			}
			skip = reader.Position - reader.Start - 1;
			return result;
		}
		private static ParameterStructure CollectParameterStructure(CommandFileReader reader, CommandStructure parent, out int skip)
		{
			ParameterStructure result = null;

			string line = reader.NextLine();
			if (Regex.IsMatch(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)"))
			{
				string[] names = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
					.Trim('(', ')').Split(',').Select(n => n.Trim()).ToArray();
				result = new ParameterStructure(parent, names);
			}
			else throw new PositionException(reader.GetLineObject(),
				new MalformedCodeException(@"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)"));
			while(!reader.IsDone)
			{
				line = reader.NextLine();
				if (line.StartsWith("flags:"))
				{
					string[] values = line.Substring(line.IndexOf(':') + 1).Trim().Split(',').Select(v => v.Trim()).ToArray();
					foreach (var value in values)
					{
						result.FlagsPosition(reader.GetLineObject());
						switch (value)
						{
							case "short":
								result.SetShort();
								break;
							case "exclusive":
								result.SetExclusive();
								break;
							case "meta":
								//result.IsMeta = true;
								throw new NotImplementedException();
								break;
							default:
								throw new PositionException(reader.GetLineObject(),
									new WrongValueException(value, new string[]
									{
										"short", "exclusive", "meta"
									}));
						}
					}
				}
				else if (line.StartsWith("type:"))
				{
					string value = line.Substring(line.IndexOf(':') + 1).Trim();
					if (!Regex.IsMatch(line, RegexParameterType))
						throw new PositionException(reader.GetLineObject(),
							new WrongValueException(value, new string[]
							{
								"string", "file", "command", "bool", "int", "float"
							}));

				}
				else if (Regex.IsMatch(line, RegexParameterOthers))
				{
					if (result.IsExclusive)
						throw new PositionException(reader.GetLineObject(),
							new ExclusiveParameterException(result));
					result.SetOther(line.Split(':')[1].Split(',').Select(o => o.Trim()).ToArray());
				}
				else if (EndsBlock(line)) break;
				else throw new PositionException(reader.GetLineObject(), 
					new MalformedCodeException(RegexParameterFlags, RegexParameterOthers, RegexParameterType));
			}

			skip = reader.Position - reader.Start - 1;
			return result;
		}
		private static string[] CollectExampleStructure(CommandFileReader reader, out int skip)
		{
			string[] result = null;

			string line = reader.NextLine();
			if (Regex.IsMatch(line, RegexExample))
			{
				string[] used = Regex.Match(line, @"\(" + CreateMulitpleRegex(RegexVariable) + @"\)").Value
								.Trim('(', ')').Split(',').Select(s => s.Trim()).ToArray();
				if (used.Length > 0 && used[0] != string.Empty)
					result = used;
			}
			else throw new PositionException(reader.GetLineObject(),
				new MalformedCodeException(RegexExample));
			while (!reader.IsDone)
			{
				line = reader.NextLine();

				if (Regex.IsMatch(line, RegexExampleExplanation))
				{				}
				else if (Regex.IsMatch(line, RegexExampleLine))
				{
				}
				else if (EndsBlock(line))
					break;
				else throw new PositionException(reader.GetLineObject(),
					new MalformedCodeException(RegexExampleExplanation, RegexExampleLine));
			}
			skip = reader.Position - reader.Start - 1;

			return result;
		}
		private static string CreateVersionRegex(int[] supportedVersions)
		{
			throw new NotImplementedException();
		}
		private static void ValidateStructure(FileStructure structure)
		{
			foreach (CommandStructure cmdStructure in structure)
			{
				ValidateCommandStructure(cmdStructure);
			}
		}
		private static void ValidateCommandStructure(CommandStructure cmdStructure)
		{
			string[][] names = new string[cmdStructure.GetParameters().Count()][];
			int i = 0;
			foreach (var parameter in cmdStructure.GetParameters())
			{
				names[i] = parameter.Names;
				for (int j = 0; j < i; j++)
					foreach (var name in names[i])
						if (names[j].Contains(name))
							throw new NameAlreadyUsedException(name, parameter, cmdStructure[j]);
				ValidateParameterStructure(parameter);
				i++;
			}
			foreach (var example in cmdStructure.GetExamples())
			{
				if(example != null)
					ValidateExampleStructure(cmdStructure, example);
			}
		}
		private static void ValidateExampleStructure(CommandStructure parent, string[] example)
		{
			ParameterStructure[] usedParameters = new ParameterStructure[example.Length];
			for (int i = 0; i < example.Length; i++)
			{
				if (!parent.HasParameter(example[i]))
					throw new ParameterDoesNotExistException(example, example[i]);
				usedParameters[i] = parent.GetParameterByName(example[i]);
			}
			foreach (var usedParameter in usedParameters)
			{
				foreach (var otherParameter in usedParameters)
				{
					if (usedParameter == otherParameter) continue;
					if (!usedParameter.Allows(otherParameter))
						throw new ParameterNotAllowedException(usedParameter, otherParameter);
					if (!otherParameter.Allows(usedParameter))
						throw new ParameterNotAllowedException(otherParameter, usedParameter);
				}
			}
		}
		private static void ValidateParameterStructure(ParameterStructure parameter)
		{
			if (parameter.IsExclusive || parameter.Others == null) return;
			foreach (var other in parameter.Others)
			{
				if (!parameter.Parent.HasParameter(other))
					throw new ParameterDoesNotExistException(parameter, other);
				var otherParameter = parameter.Parent.GetParameterByName(other);
				if (!otherParameter.Allows(parameter))
					throw new ParameterNotAllowedException(otherParameter, parameter);
			}
		}
		
		#region Regex
		//TODO: Use string.Format();
		private static string RegexVariable => "[a-zA-Z][a-zA-Z_0-9]*";
		private static string RegexCommand => @"cmd\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\):";
		private static string RegexParameter => CreateOptionRegex("parameter", "metaparameter") + @"\(" + CreateMulitpleRegex(RegexVariable) + @"\):";
		private static string RegexExample => @"example\(" + CreateOptionRegex(RegexVariable + @"(\s*,\s*" + RegexVariable + @")*", "#empty") + @"\):";
		private static string RegexCommandContents => @"callback:\s?" + RegexVariable;
		private static string RegexParameterContents => CreateOptionRegex(
					RegexParameterFlags,
					RegexParameterType,
					RegexParameterOthers        //TODO: Implement List and Use list of possible values -> See SecondRunIsValid
				);
		private static string RegexParameterFlags =>
			CreateKeyValueRegex("flags", CreateMulitpleRegex(CreateOptionRegex("short", "exclusive")));
		private static string RegexParameterType =>
			CreateKeyValueRegex("type", CreateOptionRegex("bool", "string", "file", "int", "float", "command")); //bool, string, file, command, int, or float
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
		#endregion

	}
}
