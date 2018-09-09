using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console.Parser
{
	public class Parser
	{
		public static void Parse(string file, Type commandsClass)
		{
			ParserState state = ParserState.Command;
			List<ConsoleCommand> result = new List<ConsoleCommand>();


			string commandName = string.Empty;
			MethodInfo commandCallback = null;
			List<Parameter> commandParameters = new List<Parameter>();
			List<CommandExample> commandExamples = new List<CommandExample>();

			bool addCommand = false;


			string type = string.Empty;
			string value = string.Empty;

			using (FileStream fileStream = new FileStream(file, FileMode.Open))
			using (StreamReader reader = new StreamReader(fileStream))
			{
				while(!reader.EndOfStream)
				{
					string currentLine = reader.ReadLine();
					currentLine = Sanitize(currentLine);
					if (string.IsNullOrWhiteSpace(currentLine)) continue;
					System.Console.WriteLine(state + ": " + currentLine);

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
								commandParameters.Add(loadParameter(reader, currentLine, state, out ParserState newState));
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
								var exmp = loadExample(reader, currentLine, state, out ParserState newState);
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
			}
			//Empty FILES
			result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));

			foreach (var cmd in result)
			{
				GameConsole.AddCommand(cmd);
			}
		}

		private static CommandExample? loadExample(StreamReader reader, string current, ParserState state, out ParserState newState)
		{

			string exampleLine = string.Empty;
			string exampleText = string.Empty;
			List<string> exampleParameterNames = new List<string>();
			string currentLine = current;

			while (!reader.EndOfStream)
			{
				if (string.IsNullOrWhiteSpace(currentLine))
				{
					currentLine = reader.ReadLine();
					currentLine = Sanitize(currentLine);
					continue;
				}

				string type = currentLine.Remove(currentLine.IndexOf(':'));
				string value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
				value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
				type = currentLine.Remove(currentLine.IndexOf(':'));
				switch (type)
				{
					case "line":
						exampleLine = value;
						break;
					case "exp":
					case "explanation":
						exampleText += value + "\n";
						break;
					case "usingparameter":
						exampleParameterNames.Add(value);
						break;
					case "parameter":
						newState = ParserState.Parameter;
						return new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray());
					case "example":
						newState = ParserState.Example; //Just to make it obvious
						return new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray());
					case "cmd":
						newState = ParserState.Command;
						return new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray());
					default:
						throw new NotImplementedException("Didn't expect " + type + " here.");
				}


				currentLine = reader.ReadLine();
				currentLine = Sanitize(currentLine);
			}
			newState = state;
			return null;
		}

		private static Parameter loadParameter(StreamReader reader, string current, ParserState state, out ParserState newState)
		{
			string parameterName = string.Empty;
			string parameterShortName = string.Empty;
			bool parameterIsOptional = false;
			bool parameterHasValue = false;
			ParameterType parameterType = ParameterType.Boolean;

			string currentLine = current;

			while (!reader.EndOfStream)
			{
				if (string.IsNullOrWhiteSpace(currentLine))
				{
					currentLine = reader.ReadLine();
					currentLine = Sanitize(currentLine);
					continue;
				}

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
						parameterIsOptional = bool.Parse(value);
						break;
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
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType);
						else
							throw new NotSupportedException("Wrong State at this point..");
					case "example":
						newState = ParserState.Example;
						if (state == ParserState.Parameter)
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType);
						else
							throw new NotSupportedException("Wrong State at this point..");
					case "cmd":
						newState = ParserState.Command;
						if (state == ParserState.Parameter)
							return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType);
						else if (state == ParserState.MetaParameter)
							return new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType);
						break;
					default:
						throw new NotImplementedException("Didn't expect " + type + " here.");
				}
				currentLine = reader.ReadLine();
				currentLine = Sanitize(currentLine);
			}
			newState = state;
			if (state == ParserState.Parameter)
				return new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType);
			else if (state == ParserState.MetaParameter)
				return new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType);
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
}
