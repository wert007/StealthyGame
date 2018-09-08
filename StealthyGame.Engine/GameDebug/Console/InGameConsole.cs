using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console
{
	public static class InGameConsole
	{
		static List<string> lastTyped;
		static Queue<ConsoleMessage> toOutput;
		static List<ConsoleCommand> commands;
		public delegate void TextReceivedHandler(string text);
		public static event TextReceivedHandler TextReceived;

		static bool intend;
		static int intendation;
		static int height;
		static int width;
		static int typedIndex;

		public static void Init()
		{
			lastTyped = new List<string>();
			toOutput = new Queue<ConsoleMessage>();
			commands = new List<ConsoleCommand>();
			intend = false;
			intendation = 0;
			width = 0;
			height = 0;
			typedIndex = 0;
			
		}

		public static void Log(string message)
		{
			toOutput.Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize()));
			while (toOutput.Count > height)
			{
				toOutput.Dequeue();
			}
		}

		public static void Log(string message, Color color)
		{
			toOutput.Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize(), color));
			while (toOutput.Count > height)
			{
				toOutput.Dequeue();
			}
		}


		public static void Log(string message, Color color, Color background)
		{
			toOutput.Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize(), color, background));
			while (toOutput.Count > height)
			{
				toOutput.Dequeue();
			}
		}

		//TODO: Optimize (Dequeue once and not everytime)
		public static void Log(ConsoleMessage[] consoleMessages)
		{
			foreach (var consoleMessage in consoleMessages)
			{
				Log(consoleMessage);
			}
		}

		//TODO: Optimize (BackroundColor = Transparent??)
		public static void Log(ConsoleMessage consoleMessage)
		{
			toOutput.Enqueue(consoleMessage);
			while (toOutput.Count > height)
			{
				toOutput.Dequeue();
			}
		}

		public static void SetBufferSize(Index2 size)
		{
			height = size.Y;
			width = size.X;
		}

		public static void AddCommand(ConsoleCommand command)
		{
			commands.Add(command);
		}
		public static IEnumerable<ConsoleMessage> MessagesToPrint()
		{
			return toOutput;
		}

		public static void SendText(string text)
		{
			lastTyped.Add(text);
			typedIndex = lastTyped.Count;
			if (commands.Any(c => c.IsMatch(text)))
			{
				var cmd = commands.FirstOrDefault(c => c.IsMatch(text));
				Log(">\t" + text);
				intend = true;
				intendation = 2;
				cmd.Invoke(text);
				intend = false;
			}
			else if(!string.IsNullOrWhiteSpace(text.Trim('/')))
			{
				Log("Typed:", Color.Red);
				Log("\t" + text, Color.Red, new Color(Color.DarkGray,0.7f)); //0.7f should be in ConsoleControl
				Log("Couldn't match to any command", Color.Red);
			}
			TextReceived?.Invoke(text);
		}

		public static string GetNextTyped()
		{
			typedIndex = Math.Min(typedIndex + 1, lastTyped.Count);
			if (typedIndex == lastTyped.Count)
				return string.Empty;
			return lastTyped[typedIndex];
		}

		public static string GetPreviousTyped()
		{
			typedIndex = Math.Max (typedIndex - 1, 0);
			return lastTyped[typedIndex];
		}

		internal static ConsoleCommand[] GetCommands()
		{
			return commands.ToArray();
		}
	}
}