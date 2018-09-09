using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameDebug.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console
{
	public static class StdConsoleCommands
	{
		public static FileTree FileTree { get; set; }
		private static FileTreeItem current;

		public delegate void ExitCalledEventHandler();
		public static event ExitCalledEventHandler ExitCalled;

		public static void Init()
		{
			FileTree = new FileTree();
			current = FileTree.Root;
		}

		public static void Help(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				string commandName = args[0].GetAsString();
				var cmd = GameConsole.GetCommands().FirstOrDefault(c => c.Name == commandName);
				if (cmd.Name == null)
					GameConsole.Log("No Command named " + commandName, Color.Red);
				else
					cmd.PrintExamples();
			}
			else if (args.Length == 0)
			{
				foreach (var cmd in GameConsole.GetCommands())
				{
					GameConsole.Log(cmd.Name);
				}
				GameConsole.Log("Type /help <command> to see examples of the usage.");
			}
		}

		public static void Size(ParameterValue[] args)
		{
			GameConsole.Log("Currently not implemented");
		}

		public static void Exit(ParameterValue[] args)
		{
			ExitCalled?.Invoke();	
		}

		public static void Clear(ParameterValue[] args)
		{
			GameConsole.Clear();
		}

		public static void FileManager(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "list":
						if (current == null)
							current = FileTree.Root;
						if (current.Children == null)
							current.GenerateChildren();
						GameConsole.Log(current.ShortName);
						foreach (var child in current.Children.Reverse())
						{
							GameConsole.Log("-\t" + child.ShortName);
						}
						break;
					default:
						throw new NotImplementedException();
				}

			}
		}
	}
}
