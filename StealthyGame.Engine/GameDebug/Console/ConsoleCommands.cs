using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console
{
	public static class ConsoleCommands
	{
		public static void Help(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				string commandName = args[0].GetAsString();
				var cmd = InGameConsole.GetCommands().FirstOrDefault(c => c.Name == commandName);
				if (cmd.Name == null)
					InGameConsole.Log("No Command named " + commandName, Color.Red);
				else
					cmd.PrintExamples();
			}
			else if (args.Length == 0)
			{
				foreach (var cmd in InGameConsole.GetCommands())
				{
					InGameConsole.Log(cmd.Name);
				}
				InGameConsole.Log("Type /help <command> to see examples of the usage.");
			}
		}
	}
}
