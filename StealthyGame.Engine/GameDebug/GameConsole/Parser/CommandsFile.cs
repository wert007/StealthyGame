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

		public CommandsFile(int version)
		{
			Version = version;
		}
	}
}
