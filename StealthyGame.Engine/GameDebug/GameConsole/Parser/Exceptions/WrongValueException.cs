using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class WrongValueException : Exception
	{
		public WrongValueException(string received, string[] expected) : base(CreateMessage(received, expected))
		{

		}

		public WrongValueException(string received, string regex): base(CreateMessage(received, regex))
		{

		}

		private static string CreateMessage(string received, string[] expected)
		{
			throw new NotImplementedException();
		}

		private static string CreateMessage(string received, string regex)
		{
			throw new NotImplementedException();
		}
	}
}
