using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole
{
	public class ConsoleCommand
	{
		public string[] Names { get; set; }
		List<Parameter> parameters;
		public Parameter[] Parameters => parameters.ToArray();
		List<CommandExample> examples;
		public CommandExample[] Examples => examples.ToArray();
		MethodInfo callback;
		
		static string Regex => "\\/" + CommandRegex + "( " + CommandRegex +"| (" + Parameter.Regex + ")+)*";
		static string CommandRegex => @"[a-zA-Z][a-zA-Z_0-9]*";

		public ConsoleCommand()
		{
			parameters = new List<Parameter>();
			examples = new List<CommandExample>();
		}

		public ConsoleCommand(string[] names, MethodInfo callback, params Parameter[] parameters)
		{
			this.Names = names;
			this.parameters = new List<Parameter>();
			this.parameters.AddRange(parameters);
			this.callback = callback;
			examples = new List<CommandExample>();
			if(parameters.Length > 1 && parameters.Count(p => p.IsMeta) >= 1) //TODO: Ugly
			{
				throw new ArgumentException("MetaParameter may be the only one in a list of parameters!");
			}
		}

		public ConsoleCommand(string[] names, MethodInfo callback, Parameter[] parameters, CommandExample[] examples) : this(names, callback, parameters)
		{
			this.examples = new List<CommandExample>();
			this.examples.AddRange(examples);
		}

		public void SetCallback(MethodInfo callback)
		{
			this.callback = callback;
		}

		internal void Add(Parameter parameter)
		{
			parameters.Add(parameter);
		}

		internal void Add(CommandExample commandExample)
		{
			examples.Add(commandExample);
		}

		public bool IsMatch(string line)
		{
			if (!System.Text.RegularExpressions.Regex.IsMatch(line, Regex))
				return false;
			line = line.Trim('/');
			if(Names.Contains(line) || Names.Any(n => line.StartsWith(n + " ")))
			{
				if (!line.Contains(' '))
					return true;
				line = line.Substring(line.IndexOf(' '));
				line = line.Trim('-');
				for (int i = 0; i < Parameters.Length; i++)
				{
					if (Parameters[i] is Parameter)
					{
						for (int j = 0; j < Parameters[i].Names.Length; j++)
						{
							if (line.StartsWith(Parameters[i].Names[j]))
							{
								if (Parameters[i].HasValue)
								{
									line = line.Substring(Parameters[i].Names[j].Length + 1);
									if (Parameters[i].Type == ParameterType.String ||
										Parameters[i].Type == ParameterType.File)
									{
										var match = System.Text.RegularExpressions.Regex.Match(line, Parameter.String);

									}
									else
										line = line.Substring(line.IndexOf(' '));

								}
							}
						}
					}
					else
					{
						
					}
				}
				return true;
			}
			return false;
		}

		internal IEnumerable<string> GetSuggestions(string text)
		{
			string line = text.Trim('/');
			List<string> result = new List<string>();
			if (Names.Any(n => line.Trim() == n))
			{
				foreach (var p in Parameters)
				{
					result.Add(text.Trim() + " -" + p.Names[0]);
				}
			}
			else if (Names.Any(n => line.StartsWith(n)) && false)
			{
				line = line.Substring(Names.Length + 1);
				throw new IndexOutOfRangeException();
				var parameter = Parameters.Where(p => p.Names.Contains(line.Trim('-').Trim()));
				if (parameter != null)
					foreach (var para in parameter)
						foreach (var sug in para.GetSuggestions(text))
							result.Add(text.Trim() + " " + sug);
			}
			return result;
		}

		public void Invoke(string line)
		{
			List<ParameterValue> args = new List<ParameterValue>();
			line = line.TrimStart('/');
			if (Names.Any(n => line.StartsWith(n)))
			{

				if (!line.Contains(' '))
				{
					callback.Invoke(null, new object[1] { new ParameterValue[0] });
					return;
				}
				if (line.Contains('-'))
				{
					line = line.Substring(line.IndexOf('-'));
					line = line.Trim('-');
					for (int i = 0; i < Parameters.Length; i++)
					{
						for (int j = 0; j < Parameters[i].Names.Length; j++)
						{
							if (line.StartsWith(Parameters[i].Names[j]))
							{
								if (Parameters[i].HasValue)
								{
									line = line.Substring(Parameters[i].Names[j].Length + 1);
									string parameter;
									Match match;
									switch (Parameters[i].Type)
									{
										case ParameterType.String:
											match = System.Text.RegularExpressions.Regex.Match(line, Parameter.String);
											parameter = match.Value;
											args.Add(Parameters[i].CreateValue(parameter));
											break;
										case ParameterType.File:
											match = System.Text.RegularExpressions.Regex.Match(line, Parameter.File);
											parameter = match.Value;
											args.Add(Parameters[i].CreateValue(parameter.Trim('\'')));
											break;
										case ParameterType.Integer:
											match = System.Text.RegularExpressions.Regex.Match(line, Parameter.Integer);
											parameter = match.Value;
											if (int.TryParse(parameter, out int parsedInt))
												args.Add(Parameters[i].CreateValue(parsedInt));
											else
												throw new ArgumentException();
											break;
										case ParameterType.Float:
											match = System.Text.RegularExpressions.Regex.Match(line, Parameter.Float);
											parameter = match.Value;
											if (float.TryParse(parameter, out float parsedFloat))
												args.Add(Parameters[i].CreateValue(parsedFloat));
											else
												throw new ArgumentException();
											break;
										case ParameterType.Boolean:
											throw new Exception("Something went wrong: ParameterType Boolean implicits no Value; Parameter.HasValue can not be true!");
										default:
											throw new NotImplementedException();
									}
									line = line.Substring(match.Index + match.Length);
									break;
								}
								else
								{
									args.Add(Parameters[i].CreateValue(true));
									break;
								}

							}
						}
					}
				}
				else
				{
					line = line.Substring(line.IndexOf(' ')).Trim();
					args.Add(Parameters.FirstOrDefault(p => p.IsMeta).CreateValue(line));
				}
			}
			callback.Invoke(null, new object[1] { args.ToArray() });
		}

		public override string ToString()
		{
			return "/" + Names;
		}

		public void PrintExamples()
		{
			foreach (var example in Examples)
			{
				example.Print();
			}
		}
	}
}
