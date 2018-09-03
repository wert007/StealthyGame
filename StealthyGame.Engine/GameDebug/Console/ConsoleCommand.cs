using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Debug.Console
{
	public struct ConsoleCommand
	{
		public string Name { get; private set; }
		Parameter[] parameters;
		CommandExample[] examples;
		//Action<ParameterValue[]> callback;
		MethodInfo callback;
		readonly int necessaryParameters;

		//TODO:							This will be accepted,	  vvvvvvvvvvvv		but not used in code
		static string Regex => "\\/" + CommandRegex + "( " + CommandRegex +"| (" + Parameter.Regex + ")+)*";
		static string CommandRegex => @"[a-zA-Z][a-zA-Z_0-9]*";


		public ConsoleCommand(string name, MethodInfo callback, params Parameter[] parameters)
		{
			if (!System.Text.RegularExpressions.Regex.IsMatch(name, CommandRegex))
				throw new ArgumentException("InGameConsole: " + name + " is no valid CommandName. Should only start with letters (a-z, A-Z) and only use letters, numbers and _ afterwards.");
			this.Name = name;
			this.parameters = parameters;
			this.callback = callback;
			examples = new CommandExample[0];
			necessaryParameters = parameters.Count(c => !c.IsOptional);
		}

		public ConsoleCommand(string name, MethodInfo callback, Parameter[] parameters, CommandExample[] examples) : this(name, callback, parameters)
		{
			this.examples = examples;
		}

		public bool IsMatch(string line)
		{
			if (!System.Text.RegularExpressions.Regex.IsMatch(line, Regex))
				return false;
			line = line.Trim('/');
			if(line == Name || line.StartsWith(Name + " "))
			{
				if (!line.Contains(' ') && necessaryParameters <= 0)
					return true;
				line = line.Substring(line.IndexOf(' '));
				line = line.Trim('-');
				for (int i = 0; i < parameters.Length; i++)
				{
					for (int j = 0; j < parameters[i].Names.Length; j++)
					{
						if (line.StartsWith(parameters[i].Names[j]))
						{
							if (parameters[i].HasValue)
							{
								line = line.Substring(parameters[i].Names[j].Length + 1);
								if (parameters[i].Type == ParameterType.String ||
									parameters[i].Type == ParameterType.File)
								{
									var match = System.Text.RegularExpressions.Regex.Match(line, Parameter.String);

								}
								else
									line = line.Substring(line.IndexOf(' '));

							}
						}
						else if (!parameters[i].IsOptional)
							return false;
					}
				}
				return true;
			}
			return false;
		}

		public void Invoke(string line)
		{
			List<ParameterValue> args = new List<ParameterValue>();
			line = line.TrimStart('/');
			if (line.StartsWith(Name))
			{

				if (!line.Contains(' ') && necessaryParameters <= 0)
				{
					callback.Invoke(null, new object[0]);
					return;
				}
				line = line.Substring(line.IndexOf('-'));
				line = line.Trim('-');
				for (int i = 0; i < parameters.Length; i++)
				{
					for (int j = 0; j < parameters[i].Names.Length; j++)
					{
						if (line.StartsWith(parameters[i].Names[j]))
						{
							if (parameters[i].HasValue)
							{
								line = line.Substring(parameters[i].Names[j].Length + 1);
								string parameter;
								Match match;
								switch (parameters[i].Type)
								{
									case ParameterType.String:
										match = System.Text.RegularExpressions.Regex.Match(line, Parameter.String);
										parameter = match.Value;
										args.Add(new ParameterValue(parameters[i], parameter));
										break;
									case ParameterType.File:
										match = System.Text.RegularExpressions.Regex.Match(line, Parameter.File);
										parameter = match.Value;
										args.Add(new ParameterValue(parameters[i], parameter.Trim('\'')));
										break;
									case ParameterType.Integer:
										match = System.Text.RegularExpressions.Regex.Match(line, Parameter.Integer);
										parameter = match.Value;
										if (int.TryParse(parameter, out int parsedInt))
											args.Add(new ParameterValue(parameters[i], parsedInt));
										else
											throw new ArgumentException();
										break;
									case ParameterType.Float:
										match = System.Text.RegularExpressions.Regex.Match(line, Parameter.Float);
										parameter = match.Value;
										if (float.TryParse(parameter, out float parsedFloat))
											args.Add(new ParameterValue(parameters[i], parsedFloat));
										else
											throw new ArgumentException();
										break;
									case ParameterType.Boolean:
										throw new Exception("Something went wrong: ParameterType Boolean implicits no Value; Parameter.HasValue can not be true!");
									default:
										throw new NotImplementedException();
								}
								line = line.Substring(match.Index + match.Length);
							}
							else
							{
								args.Add(new ParameterValue(parameters[i], true));
							}

						}
						else if (!parameters[i].IsOptional)
							throw new ArgumentException();
					}
				}
			}
			callback.Invoke(null, args.Cast<object>().ToArray());
		}

		public void PrintExamples()
		{
			foreach (var example in examples)
			{
				example.Print();
			}
		}
	}
}
