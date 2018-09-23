using StealthyGame.Engine.GameDebug.GameConsole;
using StealthyGame.Engine.GameDebug.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.Console
{
	public static class ConsoleCommands
	{
		public static bool PlayLoop { get; set; }
		public static bool FreezeGame { get; set; }


		public static void ConsoleLoop(ParameterValue[] args)
		{
			PlayLoop = !PlayLoop;
		}

		

		public static void ConsoleFreeze(ParameterValue[] args)
		{
			FreezeGame = !FreezeGame;
		}
	}
}
