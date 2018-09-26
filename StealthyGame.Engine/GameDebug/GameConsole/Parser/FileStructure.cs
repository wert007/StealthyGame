using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class FileStructure : IEnumerable
	{
		List<CommandStructure> commands;

		public FileStructure()
		{
			commands = new List<CommandStructure>();
		}
		public IEnumerator GetEnumerator()
		{
			return commands.GetEnumerator();
		}

		internal void Add(CommandStructure commandStructure)
		{
			commands.Add(commandStructure);
		}
	}
}
