using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole
{
	public class CommandExample
	{
		private string formatted;
		private Parameter[] used;

		public CommandExample(string formatted, string[] usedParameters)
		{
			this.formatted = formatted;
			used = null;
		}

		public CommandExample()
		{
		}

		public void AddLine(string text)
		{
			formatted += "l" + text + "\n";
		}

		public void AddExplanation(string text)
		{
			formatted += "e" + text + "\n";
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

		public void SetUsed(Parameter[] used)
		{
			this.used = used;
		}
	}
}
