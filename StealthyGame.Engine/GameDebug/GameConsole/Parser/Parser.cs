using StealthyGame.Engine.GameDebug.GameConsole.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class Parser
	{
		public static int[] SupportedVersions => new int[] { 5 };
		public static void Parse(string file, Type commandsClass)
		{
			if (!IsValid(file, commandsClass))
				GameConsole.Error("Couldn't Parse commands. File is not valid.");
			ParserState state = ParserState.Command;
			List<ConsoleCommand> result = new List<ConsoleCommand>();


			string commandName = string.Empty;
			MethodInfo commandCallback = null;
			List<Parameter> commandParameters = new List<Parameter>();
			List<CommandExample> commandExamples = new List<CommandExample>();



			string type = string.Empty;
			string value = string.Empty;

			CommandTextReader reader = new CommandTextReader(file);

			while (!reader.IsDone)
			{
				string currentLine = reader.NextLine();
				if (string.IsNullOrWhiteSpace(currentLine)) continue;
				if (currentLine.StartsWith("#")) continue;

				switch (state)
				{
					case ParserState.Command:
						value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
						type = currentLine.Remove(currentLine.IndexOf(':'));
						switch (type)
						{
							case "name":
								commandName = value;
								break;
							case "callback":
								string methodName = value;
								commandCallback = commandsClass.GetMethod(methodName);
								break;
							case "parameter":
								state = ParserState.Parameter;
								break;
							case "metaParameter":
								state = ParserState.MetaParameter;
								break;
							case "example":
								state = ParserState.Example;
								break;
							case "cmd":
								state = ParserState.Command;
								break;
							default:
								throw new NotImplementedException("Didn't expect " + type + " here.");
						}
						break;
					case ParserState.MetaParameter:
					case ParserState.Parameter:
						{
							commandParameters.Add(loadParameter(reader, state, out ParserState newState));
							state = newState;
							if (state == ParserState.Command)
							{
								result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));
								commandParameters.Clear();
							}
						}
						break;
					case ParserState.Example:
						{
							var exmp = loadExample(reader, state, out ParserState newState);
							state = newState;
							if (exmp.HasValue)
								commandExamples.Add(exmp.Value);
							if (state == ParserState.Command)
							{
								result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray(), commandExamples.ToArray()));
								commandParameters.Clear();
								commandExamples.Clear();
							}
						}
						break;
					default:
						throw new NotImplementedException();
				}
			}

			//Empty FILES
			result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));

			foreach (var cmd in result)
			{
				GameConsole.AddCommand(cmd);
			}
		}
		
		//TODO: Use string.Format();
		private static string RegexVariable => "[a-zA-Z][a-zA-Z_0-9]*";
		private static string RegexCommand => @"cmd\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\):";
		private static string RegexParameter=> CreateOptionRegex("parameter","metaparameter") + @"\(" + CreateMulitpleRegex(RegexVariable) + @"\):";
		private static string RegexExample=> @"example\(" + CreateOptionRegex(RegexVariable + @"(\s*,\s*" + RegexVariable + @")*","#empty") + @"\):";
		private static string RegexCommandContents => @"callback:\s?" + RegexVariable;
		private static string RegexParameterContents => CreateOptionRegex(
					CreateKeyValueRegex("flags", CreateMulitpleRegex(CreateOptionRegex("short", "exclusive"))),
					CreateKeyValueRegex("type", CreateOptionRegex("bool", "string", "file", "int", "float", "command")), //bool, string, file, command, int, or float
					CreateRegex("com", true)        //TODO: Implement List and Use list of possible values -> See SecondRunIsValid
				);
		private static string RegexExampleContents => CreateOptionRegex(
					CreateRegex("line", false, true),
					CreateRegex(CreateOptionRegex("exp", "explanation"), false, true) 
				);

		private static bool IsValid(string file, Type commandsClass)
		{
			CommandTextReader reader = new CommandTextReader(file);
			if(FirstRunIsValid(reader, out CommandStructure[] commands))
			{
				reader.Reset();
				return SecondRunIsValid(reader, commands);
			}
			return false;
		}

		private static bool SecondRunIsValid(CommandTextReader reader, CommandStructure[] commandStructures)
		{
			string line;
			int cmdIndex = 0;
			int parameterIndex = 0;
			while (!reader.IsDone)
			{
				line = reader.NextLine();

				if (Regex.IsMatch(line, RegexCommand))
				{
					int nameCount = line.Count(c => c == ',') + 1;
					string[] names = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
						.Trim('(', ')').Split(','); 

					foreach (var name in names)
					{
						if (commandStructures.Take(cmdIndex).Any(c => c.Command == name)) //TODO: Support multiple names for commands
							return false; //TODO: throw Exception
					}
					cmdIndex++;
					parameterIndex = 0;

					while (!reader.IsDone)
					{
						line = reader.NextLine();
						if (Regex.IsMatch(line, RegexParameter))
						{
							nameCount = line.Count(c => c == ',') + 1;
							names = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
								.Trim('(', ')').Split(','); //Magic. Basicly getting rid of everthing til Brackets. Then getting rid of the Brackets and taking the First Element
																	
							foreach (var name in names)
							{
								foreach(var cmd in commandStructures.Take(cmdIndex))
								{
									foreach (var p in cmd.GetParameters().Take(parameterIndex))
									{
										if (p == name) //TODO: ShortNames are Missing..
											return false;
									}
								}
							}

							parameterIndex++;


							while (!reader.IsDone)
							{
								line = reader.NextLine();
								if (!Regex.IsMatch(line, RegexParameterContents))
								{
									reader.SkipLines(-1);
									break;
								}
							}
						}
						else if (Regex.IsMatch(line, RegexExample))
						{
							int parameterCount = line.Count(c => c == ',') + 1;
							if (line.Contains('#'))
								parameterCount = 0;
							//TODO: Check if parameters named here are actually allowed to be used with each other

							while (!reader.IsDone)
							{
								line = reader.NextLine();
								if (!Regex.IsMatch(line, RegexExampleContents))
								{
									reader.SkipLines(-1);
									break;
								}
							}
						}
						else if (!Regex.IsMatch(line, RegexCommandContents))
						{
							reader.SkipLines(-1);
							break;
						}
					}
				}
			}
			return true;
		}

		private static bool FirstRunIsValid(CommandTextReader reader, out CommandStructure[] commandStructures)
		{
			List<CommandStructure> commands = new List<CommandStructure>();
			commandStructures = null;
			string line;
			while (!reader.IsDone)
			{
				line = reader.NextLine();

				if (line.StartsWith("#ver"))
				{
					if (int.TryParse(line.Substring("#ver".Length), out int version))
					{
						if (!SupportedVersions.Contains(version))
							return false;
					}
					else return false;
				}
				else if (Regex.IsMatch(line, RegexCommand))
				{
					int nameCount = line.Count(c => c == ',') + 1;
					string firstName = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
						.Trim('(', ')').Split(',')[0].Trim(); //Magic. Basicly getting rid of everthing til Brackets. Then getting rid of the Brackets and taking the First Element
					commands.Add(new CommandStructure(firstName));

					while (!reader.IsDone)
					{
						line = reader.NextLine();
						if (Regex.IsMatch(line, RegexParameter))
						{
							nameCount = line.Count(c => c == ',') + 1;
							firstName = Regex.Match(line, @"\(" + RegexVariable + @"(\s*,\s*" + RegexVariable + @")*\)").Value
								.Trim('(', ')').Split(',')[0].Trim(); //Magic. Basicly getting rid of everthing til Brackets. Then getting rid of the Brackets and taking the First Element
							commands.Last().AddParameter(firstName);

							while (!reader.IsDone)
							{
								line = reader.NextLine();
								if (!Regex.IsMatch(line, RegexParameterContents))
								{
									reader.SkipLines(-1);
									break;
								}
							}
						}
						else if (Regex.IsMatch(line, RegexExample))
						{
							int parameterCount = line.Count(c => c == ',') + 1;
							if (line.Contains('#'))
								parameterCount = 0;
							//TODO: Check if parameters named here are actually allowed to be used with each other

							while (!reader.IsDone)
							{
								line = reader.NextLine();
								if (!Regex.IsMatch(line, RegexExampleContents))
								{
									reader.SkipLines(-1);
									break;
								}
							}
						}
						else if (!Regex.IsMatch(line, RegexCommandContents))
						{
							reader.SkipLines(-1);
							break;
						}
					}
				}
				else
				{
					return false;
				}
			}
			commandStructures = commands.ToArray();
			return true;

		}

		private static string CreateRegex(string name, bool multiple = false, bool allowStringValues = false)
		{
			string value = "[a-zA-Z][a-zA-Z_0-9]*";
			if(allowStringValues)
				value = ".*";
			if(multiple)
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

		private static CommandExample? loadExample(CommandTextReader reader, ParserState state, out ParserState newState)
		{

			string exampleFormatted = string.Empty;
			List<string> exampleParameterNames = new List<string>();
			reader.SkipLines(-1);
			string currentLine;

			while (!reader.IsDone)
			{
				currentLine = reader.NextLine();
				if (string.IsNullOrWhiteSpace(currentLine))
					continue;

				string type = currentLine.Remove(currentLine.IndexOf(':'));
				string value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
				value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
				type = currentLine.Remove(currentLine.IndexOf(':'));
				switch (type)
				{
					case "line":
						exampleFormatted += "l" + value + "\n";
						break;
					case "exp":
					case "explanation":
						exampleFormatted += "e" +  value + "\n";
						break;
					case "usingparameter":
						exampleParameterNames.Add(value);
						break;
					case "parameter":
						newState = ParserState.Parameter;
						return new CommandExample(exampleFormatted, exampleParameterNames.ToArray());
					case "example":
						newState = ParserState.Example; //Just to make it obvious
						return new CommandExample(exampleFormatted, exampleParameterNames.ToArray());
					case "cmd":
						newState = ParserState.Command;
						return new CommandExample(exampleFormatted, exampleParameterNames.ToArray());
					default:
						throw new NotImplementedException("Didn't expect " + type + " here.");
				}


				currentLine = reader.NextLine();
			}
			newState = state;
			return null;
		}

		private static Parameter loadParameter(CommandTextReader reader, ParserState state, out ParserState newState)
		{
			string parameterName = string.Empty;
			string parameterShortName = string.Empty;
			bool parameterHasValue = false;
			ParameterType parameterType = ParameterType.Boolean;

			reader.SkipLines(-1);
			string currentLine;

			while (!reader.IsDone)
			{
				currentLine = reader.NextLine();
				if (string.IsNullOrWhiteSpace(currentLine))
					continue;

				string type = currentLine.Remove(currentLine.IndexOf(':'));
				string value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();

				switch (type)
				{
					case "name":
						parameterName = value;
						break;
					case "shortName":
						parameterShortName = value;
						break;
					case "optional":
						throw new Exception("Old Using of optional argument. Is not supported anymore, every parameter is optional.");
					case "type":
						string pType = value;
						parameterHasValue = true;
						switch (pType)
						{
							case "bool":
								parameterHasValue = false;
								parameterType = ParameterType.Boolean;
								break;
							case "string":
								parameterType = ParameterType.String;
								break;
							case "file":
								parameterType = ParameterType.File;
								break;
							case "int":
								parameterType = ParameterType.Integer;
								break;
							case "float":
								parameterType = ParameterType.Float;
								break;
							case "command":
								parameterType = ParameterType.Command;
								break;
							default:
								throw new NotSupportedException("No ParameterType named " + pType + ". Try bool, string, file, command, int, or float");
						}
						break;
					case "parameter":
						newState = ParserState.Parameter;
						if (state == ParserState.Parameter)
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterType);
						else
							throw new NotSupportedException("Wrong State at this point..");
					case "example":
						newState = ParserState.Example;
						if (state == ParserState.Parameter)
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterType);
						else
							throw new NotSupportedException("Wrong State at this point..");
					case "cmd":
						newState = ParserState.Command;
						if (state == ParserState.Parameter)
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterType);
						break;
					default:
						throw new NotImplementedException("Didn't expect " + type + " here.");
				}
				currentLine = reader.NextLine();
			}
			newState = state;
			if (state == ParserState.Parameter)
				return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterType);
			else if (state == ParserState.MetaParameter)
				return new MetaParameter(parameterName, parameterShortName, parameterType);
			else throw new Exception();
		}

		private static string Sanitize(string currentLine)
		{
			if(currentLine.Contains("//"))
				return currentLine.Remove(currentLine.IndexOf("//")).Trim();
			return currentLine.Trim();
		}
	}

	enum ParserState
	{
		Command,
		Parameter,
		Example,
		MetaParameter
	}

	struct CommandStructure
	{
		public string Command { get; private set; }
		List<string> parameters;

		public CommandStructure(string name)
		{
			Command = name;
			parameters = new List<string>();
		}

		public void AddParameter(string parameter)
		{
			parameters.Add(parameter);
		}

		public IEnumerable<string> GetParameters()
		{
			return parameters;
		}
	}
}
