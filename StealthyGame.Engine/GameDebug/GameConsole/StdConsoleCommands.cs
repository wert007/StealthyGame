using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameDebug.DataStructures;
using StealthyGame.Engine.GameDebug.DataStructures.TimeManagement;
using StealthyGame.Engine.GameDebug.DataStructures.VariableSystem;
using StealthyGame.Engine.GameDebug.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole
{
	public static class StdConsoleCommands
	{
		public static FileTree FileTree { get; set; }
		static List<CmdValue> Values;
		public static ClassTree ClassTree { get; set; }

		public delegate void ExitCalledEventHandler();
		public static event ExitCalledEventHandler ExitCalled;
		public delegate void SaveFramesEventHandler(string directory);
		public static event SaveFramesEventHandler SaveFrames;

		public static void Init()
		{
			FileTree = new FileTree();
			Values = new List<CmdValue>();
			ClassTree = new ClassTree();
		}

		public static void Help(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				string commandName = args[0].GetAsString();
				var cmd = GameConsole.GetCommands().FirstOrDefault(c => c.Name == commandName);
				if (cmd.Name == null)
					GameConsole.Error("No Command named " + commandName);
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
			GameConsole.Error("Currently not implemented");
		}

		public static void Save(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "gif":
						string dir = args[0].GetAsFile();
						if (!Directory.Exists(dir))
							Directory.CreateDirectory(dir);
						SaveFrames?.Invoke(dir);
						break;
					default:
						throw new NotSupportedException();
				}
			}
		}

		public static void Filter(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "toggle":
						MessageType messageType;
						switch (args[0].GetAsString().ToLower())
						{
							case "m":
							case "message":
								messageType = MessageType.Message;
								break;
							case "w":
							case "warning":
								messageType = MessageType.Warning;
								break;
							case "e":
							case "error":
								messageType = MessageType.Error;
								break;
							case "t":
							case "terminatingerror":
								messageType = MessageType.TerminatingError;
								break;
							default:
								GameConsole.Error(args[0].GetAsString() + " is no valid argument. Use /help filter to see examples.");
								return;
						}
						GameConsole.ShownMessages ^= messageType;
						GameConsole.Log(GameConsole.ShownMessages.ToString());
						break;
					case "level":
						switch (args[0].GetAsInt())
						{
							case 0:
								GameConsole.ShownMessages = MessageType.Level0;
								break;
							case 1:
								GameConsole.ShownMessages = MessageType.Level1;
								break;
							case 2:
								GameConsole.ShownMessages = MessageType.Level2;
								break;
							case 3:
								GameConsole.ShownMessages = MessageType.Level3;
								break;
							default:
								GameConsole.Error("Only Values between 0 and 3 are valid. Use /help filter to see examples.");
								return;
						}
						break;
					default:
						throw new NotSupportedException();
				}
			}
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
						if (FileTree.Current.Children == null)
							FileTree.Current.GenerateChildren();
						GameConsole.Log(FileTree.Current.ShortName);
						foreach (var child in FileTree.Current.Children)
						{
							GameConsole.Log("-\t" + child.ShortName);
						}
						break;
					case "home":
						FileTree = new FileTree(args[0].GetAsFile());
						GameConsole.Log("Home set to " + FileTree.Root.ShortName);
						break;
					case "reset":
						FileTree = new FileTree();
						GameConsole.Log("Home (re-)set to " + FileTree.Root.ShortName);
						break;
					default:
						throw new NotImplementedException();
				}

			}
		}

		
		public static void Value(ParameterValue[] args)
		{
			GameConsole.Error("Not implemented");
			if(args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "name":
						Values.Add(new CmdValue(args[0].GetAsString(), FileTree.Current.Load()));
						break;
					default:
						GameConsole.Error("Not implemented");
						break;

				}
			
			}
		}

		public static void Set(ParameterValue[] args)
		{

			GameConsole.Error("Not implemented");
			if (args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "name":
						var val = Values.FirstOrDefault(v => v.Name == args[0].GetAsString());
						if (val == null)
							GameConsole.Error("No Value is named like this..");
						ClassTree.Current.SetValue(val.Value);
						break;
					default:
						GameConsole.Error("Not implemented");
						break;

				}

			}
		}

		public static void Inspect(ParameterValue[] args)
		{
			if (args.Length == 0)
			{
				ClassTree.Current = ClassTree.Root;
				if (ClassTree.Current.Children == null)
					ClassTree.Current.GenerateChildren();
				foreach (var child in ClassTree.Current.Children)
				{
					GameConsole.Log(child.ToString());
				}
			}
			else if (args.Length == 1)
			{
				switch (args[0].Parameter.Names[0])
				{
					case "object":
						InspectObject(args);
						break;
					case "reset":
						ClassTree.Current = ClassTree.Root;
						GameConsole.Log("Current ClassTreeItem reset to root.");
						break;
					case "current":
						GameConsole.Log(ClassTree.Current.ToString());
						break;
					default:
						throw new NotImplementedException();

				}
			}
		}

		private static void InspectObject(ParameterValue[] args)
		{
			string obj = args[0].GetAsString();
			ClassTreeItem target = null;
			if (ClassTree.Current.Children == null)
				ClassTree.Current.GenerateChildren();
			foreach (var child in ClassTree.Current.Children)
			{
				if (child.HasName && child.Name == obj)
					target = child;
			}
			if (target == null)
			{
				GameConsole.Error("No object named " + obj + " is a child of the " + ClassTree.Current.ClassName + " class.");
				return;
			}
			ClassTree.Current = target;
			if (target.Children == null)
				target.GenerateChildren();
			foreach (var child in target.Children)
			{
				GameConsole.Log(child.ToString());
			}
		}

		public static void TimeWatcherCmd(ParameterValue[] args)
		{
			if(args.Length == 1)
			{
				switch(args[0].Parameter.Names[0])
				{
					case "list":
						//foreach (var entry in TimeWatcher.GetEntries())
						//{
						//	GameConsole.Log(entry.ToString());
						//}
						//break;
					default:
						GameConsole.Error("Not implemented");
						break;
				}
			}
		}
	}
}
