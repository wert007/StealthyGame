﻿using Microsoft.Xna.Framework;
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
	public static class GameConsole
	{
		static List<string> lastTyped;
		static List<string> suggestions;
		static Queue<ConsoleMessage> output;
		static Queue<ConsoleMessage> waiting;
		static List<ConsoleCommand> commands;
		public delegate void TextReceivedHandler(string text);
		public static event TextReceivedHandler TextReceived;
		public static event TextReceivedHandler TextUpdated;

		static bool intend;
		static int intendation;
		static int height;
		static int width;
		static int typedIndex;
		static int suggestionIndex;

		static int messagesOnScreen;
		public static int WaitingMessages => waiting.Count;

		public static void Init()
		{
			lastTyped = new List<string>();
			output = new Queue<ConsoleMessage>();
			waiting = new Queue<ConsoleMessage>();
			commands = new List<ConsoleCommand>();
			suggestions = new List<string>();
			intend = false;
			intendation = 0;
			width = 0;
			height = 0;
			typedIndex = 0;
			suggestionIndex = -1;
			messagesOnScreen = 0;
		}

		private static void Enqueue(ConsoleMessage consoleMessage)
		{
			if(output.Count - messagesOnScreen >= height)
			{
				waiting.Enqueue(consoleMessage);
			}
			else
			{
				output.Enqueue(consoleMessage);
			}
			CleanOutput();
		}

		public static void Clear()
		{
			output.Clear();
			waiting.Clear();
			suggestions.Clear();
			suggestionIndex = -1;
			typedIndex = -1;
			messagesOnScreen = 0;
		}

		private static void CleanOutput()
		{
			int deletableMessages = Math.Min(messagesOnScreen, output.Count - height);
			for (int i = 0; i < deletableMessages; i++)
			{
				output.Dequeue();

			}
			messagesOnScreen -= deletableMessages;
//			while (output.Count > height)
			{
			}
		}

		public static void Log(string message)
		{
			Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize()));
		}

		public static void Log(string message, Color color)
		{
			Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize(), color));
		}

		public static void Log(string message, Color color, Color background)
		{
			Enqueue(new ConsoleMessage((
				(intend ? new string('\t', intendation) : string.Empty)
				+ message)
				.Sanitize(), color, background));
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
			if (intend)
				consoleMessage.Intend(intendation);
			Enqueue(consoleMessage);
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
			messagesOnScreen = output.Count;
			return output;
		}

		public static void UpdateText(string text)
		{
			suggestionIndex = -1;
			suggestions.Clear();
			foreach (var cmd in commands)
			{
				suggestions.AddRange(cmd.GetSuggestions(text));
			}
			TextUpdated?.Invoke(text);
		}

		public static void SendText(string text)
		{
			System.Console.WriteLine("Enter received.");
			if(text == string.Empty)
			{
				for (int i = 0; i < height && waiting.Count >0; i++)
				{
					Enqueue(waiting.Dequeue());
				}
				return;
			}
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
			if (lastTyped.Count <= 0)
				return string.Empty;
			typedIndex = Math.Max (typedIndex - 1, 0);
			return lastTyped[typedIndex];
		}

		public static string GetNextSuggestion()
		{
			if (suggestions.Count == 0)
				return string.Empty;
			suggestionIndex = (suggestionIndex + 1) % suggestions.Count;
			return suggestions[suggestionIndex];
		}

		internal static ConsoleCommand[] GetCommands()
		{
			return commands.ToArray();
		}
	}
}