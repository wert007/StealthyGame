using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class CommandFileReader
	{
		CommandFileLine[] lines;

		public bool IsDone => Position + 1 >= lines.Length;
		public string FileName { get; private set; }
		public int Start { get; private set; }
		public int Position { get; private set; }

		public CommandFileReader(string file)
		{
			FileName = file;
			Position = -1;
			Start = 0;
			int lineCount = 0;
			List<CommandFileLine> tmpLines = new List<CommandFileLine>();
			using (FileStream fs = new FileStream(file, FileMode.Open))
			using (StreamReader sr = new StreamReader(fs))
			{
				while (!sr.EndOfStream)
				{
					lineCount++;
					string actualLine = sr.ReadLine();
					if (string.IsNullOrWhiteSpace(actualLine)) continue;
					tmpLines.Add(new CommandFileLine(actualLine, lineCount, this));
				}
			}
			lines = tmpLines.ToArray();
		}

		public CommandFileLine GetRelativeLine(CommandFileLine line, int direction)
		{
			return lines[Array.IndexOf(lines, line) + direction];
		}

		private CommandFileReader(string fileName, CommandFileLine[] lines, int index)
		{
			FileName = fileName;
			this.lines = lines;
			this.Position = index;
			Start = index;
		}

		public CommandFileReader Copy()
		{
			int oldIndex = Position;
			Back();
			int copyIndex = Position;
			Position = oldIndex;
			return new CommandFileReader(FileName, lines, copyIndex);
		}

		public void Reset()
		{
			Position = -1;
		}

		public void Back()
		{
				Position = Math.Max(Position - 1, 0);
			while (!lines[Position].ContaintsCode)
			{
				Position = Math.Max(Position - 1, 0);
			}
		}

		public void Skip(int skip)
		{
			Position = Position + skip;
			while (!lines[Position].ContaintsCode && !IsDone)
			{
				Position++;
			}
		}

		public string PeekLine()
		{
			if (Position >= lines.Length)
				throw new IndexOutOfRangeException();
			return lines[Position].Sanitized;
		}

		public CommandFileLine GetLineObject()
		{
			return lines[Position];
		}

		public string NextLine()
		{
			Skip(1);
			if (Position >= lines.Length)
				throw new IndexOutOfRangeException();
			return lines[Position].Sanitized;
		}

	}
}
