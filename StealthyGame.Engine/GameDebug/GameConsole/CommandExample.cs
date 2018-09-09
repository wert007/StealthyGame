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
		private string line;
		private string explanation;
		private string[] usedParameters;

		public CommandExample(string line, string explanation, string[] usedParameters)
		{
			this.line = line;
			this.explanation = explanation;
			this.usedParameters = usedParameters;
		}

		public void Print()
		{
			InGameConsole.Log("\t" + line, Color.LightGray);
			InGameConsole.Log(ConsoleMessage.Parse(explanation));
		}
	}
}
