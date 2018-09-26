using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class CommandsFile
	{
		public int Version { get; private set; }
		public IEnumerable<ConsoleCommand> Commands => consoleCommands;

		List<ConsoleCommand> consoleCommands;
		Dictionary<Parameter, string[]> parameterOthers;
		Dictionary<CommandExample, string[]> exampleParameters;

		public CommandsFile(int version)
		{
			Version = version;
			consoleCommands = new List<ConsoleCommand>();
			parameterOthers = new Dictionary<Parameter, string[]>();
			exampleParameters = new Dictionary<CommandExample, string[]>();
		}

		public void Add(ConsoleCommand consoleCommand)
		{
			consoleCommands.Add(consoleCommand);
		}

		public void Add(Parameter parameter, string[] others)
		{
			parameterOthers.Add(parameter, others);
		}

		internal void Add(CommandExample example, string[] parameters)
		{
			exampleParameters.Add(example, parameters);
		}

		public void Load()
		{
			foreach (var cmd in consoleCommands)
			{
				foreach (var parameter in cmd.Parameters)
				{
					if (!parameterOthers.ContainsKey(parameter))
						continue;
					Parameter[] others = new Parameter[parameterOthers[parameter].Length];
					for (int i = 0; i < others.Length; i++)
					{
						others[i] = cmd.Parameters.FirstOrDefault(p => p.Name == parameterOthers[parameter][i]);
					}
					parameter.SetOthers(others);
				}
				foreach (var example in cmd.Examples)
				{
					if (!exampleParameters.ContainsKey(example))
						continue;
					Parameter[] used = new Parameter[exampleParameters[example].Length];
					for (int i = 0; i < used.Length; i++)
					{
						used[i] = cmd.Parameters.FirstOrDefault(p => p.Name == exampleParameters[example][i]);
					}
					example.SetUsed(used);
				}
			}
		}
	}
}
