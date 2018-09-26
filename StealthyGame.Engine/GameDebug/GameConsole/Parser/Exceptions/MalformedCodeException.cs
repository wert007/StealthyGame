using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class MalformedCodeException : Exception
	{
		public MalformedCodeException(string regex) : base(CreateMessage(regex))
		{

		}

		public MalformedCodeException(params string[] regex) : base(CreateMessage(regex))
		{
		}

		private static string CreateMessage(string regex)
		{
			string expected = "Expected code (Regex): " + regex;
			return expected;
		}

		private static string CreateMessage(string[] regex)
		{			
			string expected = "No code expected here. Maybe check with github.com/wert007/StealthyGame to see if the parser is right here.";
			if(regex.Length > 0)
			{
				if(regex.Length == 1)
				{
					expected = "Expected code (Regex): " + regex[0];
				}
				else
				{
					expected = "Expected code (Regex): " + regex[0];
					foreach (var r in regex.Skip(1))
					{
						expected += "\n Or this code as well: " + r;
					}
				}
			}
			return expected;
		}
	}
}
