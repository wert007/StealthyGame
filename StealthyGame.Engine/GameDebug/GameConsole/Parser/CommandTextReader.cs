using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class CommandTextReader
	{
		string[] contents;
		int index;
		public bool IsDone => index + 1 >= contents.Length;

		public CommandTextReader(string file)
		{
			index = -1;

			List<string> result = new List<string>();
			using (FileStream fs = new FileStream(file, FileMode.Open))
			using (StreamReader sr = new StreamReader(fs))
			{
				while (!sr.EndOfStream)
				{
					string line = Sanitize(sr.ReadLine());
					if (string.IsNullOrWhiteSpace(line)) continue;
					result.Add(line);
				}
			}
			contents = result.ToArray();
		}

		public void Reset()
		{
			index = -1;
		}

		public void SkipLines(int skip)
		{
			index = Math.Min(index + skip, contents.Length - 1);
		}

		public string PeekLine()
		{
			if (index >= contents.Length)
				throw new IndexOutOfRangeException();
			return contents[index];
		}

		public string NextLine()
		{
			index++;
			if (index >= contents.Length)
				throw new IndexOutOfRangeException();
			return contents[index];
		}


		private static string Sanitize(string currentLine)
		{
			if (currentLine.Contains("//"))
				return currentLine.Remove(currentLine.IndexOf("//")).Trim();
			return currentLine.Trim();
		}
	}
	}
