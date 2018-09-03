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
		private static ClassTreeItem current;

		public static void ConsoleLoop(ParameterValue[] args)
		{
			PlayLoop = !PlayLoop;
		}

		public static void ConsoleInspect(ParameterValue[] args)
		{
			if (args.Length == 0)
			{
				current = ClassTree.GetRoot();
				if (current.Children == null)
					current.GenerateChildren();
				foreach (var child in current.Children)
				{
					InGameConsole.Log(child.ToString());
				}
			}
			else if (args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "object":
						ConsoleInspectObject(args);
						break;
					case "reset":
						current = ClassTree.GetRoot();
						InGameConsole.Log("Current ClassTreeItem reset to root.");
						break;
					case "current":
						InGameConsole.Log(current.ToString());
						break;
					default:
						throw new NotImplementedException();

				}
			}
		}

		private static void ConsoleInspectObject(ParameterValue[] args)
		{
			string obj = args[0].GetAsString();
			ClassTreeItem target = null;
			if (current.Children == null)
				current.GenerateChildren();
			foreach (var child in current.Children)
			{
				if (child.HasName && child.Name == obj)
					target = child;
			}
			if (target == null)
			{
				InGameConsole.Log("No object named " + obj + " is a child of the Game class.");
				return;
			}
			current = target;
			if (target.Children == null)
				target.GenerateChildren();
			foreach (var child in target.Children)
			{
				InGameConsole.Log(child.ToString());
			}
		}

		public static void ConsoleFreeze(ParameterValue[] args)
		{
			FreezeGame = !FreezeGame;
		}
	}
}
