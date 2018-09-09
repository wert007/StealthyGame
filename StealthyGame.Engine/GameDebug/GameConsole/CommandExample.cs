using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console
{
	public struct CommandExample
	{
		private string formatted;
		private string[] usedParameters;

		public CommandExample(string formatted, string[] usedParameters)
		{
			this.formatted = formatted;
			this.usedParameters = usedParameters;
		}

		public void Print()
		{
			foreach (var line in formatted.Split('\n'))
			{
				if (line.Length <= 0)
					continue;
				if (line[0] == 'l')
				{
					GameConsole.Log("\t" + line.Substring(1), Color.LightGray);
				}
				else if (line[0] == 'e')
				{
					GameConsole.Log(ConsoleMessage.Parse(line.Substring(1) + "\n"));
				}
				else throw new NotSupportedException();
			}
		}
	}
}
