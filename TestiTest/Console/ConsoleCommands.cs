using StealthyGame.Engine.Debug.Console;
using StealthyGame.Engine.Debug.UI;
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
		public static ClassTree ClassTree { get; set; }

		public static void ConsoleLoop(object[] args)
		{
			PlayLoop = !PlayLoop;
		}

		public static void ConsoleInspect(object[] args)
		{
			if (args.Length == 0)
			{
				if (ClassTree.GetRoot().Children == null)
					ClassTree.GetRoot().GenerateChildren();
				foreach (var child in ClassTree.GetRoot().Children)
				{
					InGameConsole.Log(child.ClassName + (child.HasName ? " " + child.Name : string.Empty));
				}
			}
			else if (args.Length == 1)
			{
				string obj = args[0] as string;
				ClassTreeItem target = null;
				if (ClassTree.GetRoot().Children == null)
					ClassTree.GetRoot().GenerateChildren();
				foreach (var child in ClassTree.GetRoot().Children)
				{
					if (child.HasName && child.Name == obj)
						target = child;
				}
				if (target == null)
				{
					InGameConsole.Log("No object named " + obj + " is a child of the Game class.");
					return;
				}

				if (target.Children == null)
					target.GenerateChildren();
				foreach (var child in target.Children)
				{
					InGameConsole.Log(child.ClassName + (child.HasName ? " " + child.Name : string.Empty));
				}
			}
		}

		public static void ConsoleFreeze(object[] args)
		{
			FreezeGame = !FreezeGame;
		}
	}
}
