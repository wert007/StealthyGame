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
			ParserState state = ParserState.FileLoad;
			List<ConsoleCommand> result = new List<ConsoleCommand>();


			string commandName = string.Empty;
			MethodInfo commandCallback = null;
			List<IParameter> commandParameters = new List<IParameter>();
			List<CommandExample> commandExamples = new List<CommandExample>();

			string parameterName = string.Empty;
			string parameterShortName = string.Empty;
			bool parameterIsOptional = false;
			bool parameterHasValue = false;
			ParameterType parameterType = ParameterType.Boolean;

			string exampleLine = string.Empty;
			string exampleText = string.Empty;
			List<string> exampleParameterNames = new List<string>();

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

					switch (state)
					{
						case ParserState.FileLoad:
							System.Console.Write("Strange");
							state = ParserState.Command;
							break;
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
									result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));
									commandParameters.Clear();
									state = ParserState.Command;
									break;
								default:
									throw new NotImplementedException("Didn't expect " + type + " here.");
							}
							break;
						case ParserState.MetaParameter:
						case ParserState.Parameter:
							type = currentLine.Remove(currentLine.IndexOf(':'));
							value = currentLine.Substring(currentLine.IndexOf(':') + 1).Trim();
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
									if (state == ParserState.Parameter)
										commandParameters.Add(new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType));
									else if (state == ParserState.MetaParameter)
										commandParameters.Add(new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType));
									else
										throw new NotSupportedException("Wrong State at this point..");
									state = ParserState.Parameter; //Just to make it obvious
									break;
								case "example":
									if (state == ParserState.Parameter)
										commandParameters.Add(new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType));
									else if (state == ParserState.MetaParameter)
										commandParameters.Add(new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType));
									else
										throw new NotSupportedException("Wrong State at this point..");
									state = ParserState.Example;
									break;
								case "cmd":
									if (state == ParserState.Parameter)
										commandParameters.Add(new Parameter(parameterName, parameterShortName, parameterHasValue, parameterIsOptional, parameterType));
									else if (state == ParserState.MetaParameter)
										commandParameters.Add(new MetaParameter(parameterName, parameterShortName, parameterIsOptional, parameterType));
									else
										throw new NotSupportedException("Wrong State at this point..");
									result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));
									commandParameters.Clear();
									state = ParserState.Command;
									break;
								default:
									throw new NotImplementedException("Didn't expect " + type + " here.");
									
							}
							break;
						case ParserState.Example:
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
									commandExamples.Add(new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray()));
									exampleText = string.Empty;
									state = ParserState.Parameter;
									break;
								case "example":
									commandExamples.Add(new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray()));
									exampleText = string.Empty;
									state = ParserState.Example; //Just to make it obvious
									break;
								case "cmd":
									commandExamples.Add(new CommandExample(exampleLine, exampleText, exampleParameterNames.ToArray()));
									result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray(), commandExamples.ToArray()));
									commandParameters.Clear();
									exampleText = string.Empty;
									state = ParserState.Command;
									break;
								default:
									throw new NotImplementedException("Didn't expect " + type + " here.");
							}
							break;
						default:
							throw new NotImplementedException();
					}
				}
			}
			//May fail if file was empty..
			result.Add(new ConsoleCommand(commandName, commandCallback, commandParameters.ToArray()));

			foreach (var cmd in result)
			{
				InGameConsole.AddCommand(cmd);
			}
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
		FileLoad,
		Command,
		Parameter,
		Example,
		MetaParameter
	}
}
