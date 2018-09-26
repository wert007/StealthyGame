using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class CommandFileLine
	{
		public int Line { get; private set; }
		public string Content { get; private set; }
		public string Sanitized => Sanitize(Content);
		public bool ContaintsCode { get; private set; }
		public CommandFileReader Parent { get; private set; }
		public CommandFileLine Before => Parent.GetRelativeLine(this, -1);
		public CommandFileLine After => Parent.GetRelativeLine(this, 1);

		public CommandFileLine(string content, int line, CommandFileReader parent)
		{
			this.Content = content;
			this.Line = line;
			this.Parent = parent;
			ContaintsCode = true;
			if (string.IsNullOrWhiteSpace(Sanitized))
				ContaintsCode = false;
		}

		private static string Sanitize(string content)
		{
			if (content.Contains("//"))
				return content.Remove(content.IndexOf("//")).Trim();
			return content.Trim();
		}
	}
}
