using System;
using System.Collections.Generic;
using System.Linq;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class CommandStructure
	{
		public string Command { get; private set; }
		List<ParameterStructure> parameters;
		List<string[]> examples;
		public ParameterStructure this[int index] => parameters[index];

		public CommandStructure(string name)
		{
			Command = name;
			parameters = new List<ParameterStructure>();
			examples = new List<string[]>();
		}

		public void Add(params string[] example)
		{
			examples.Add(example);
		}

		public IEnumerable<string[]> GetExamples()
		{
			return examples;
		}

		public void Add(ParameterStructure parameter)
		{
			parameters.Add(parameter);
		}

		public IEnumerable<ParameterStructure> GetParameters()
		{
			return parameters;
		}

		public bool HasParameter(string name)
		{
			foreach (var parameter in parameters)
			{
				if (parameter.Names.Contains(name))
					return true;
			}
			return false;
		}

		public ParameterStructure GetParameterByName(string name)
		{
			foreach (var parameter in parameters)
			{
				if (parameter.Names.Contains(name))
					return parameter;
			}
			throw new ArgumentException();
		}
	}
}
