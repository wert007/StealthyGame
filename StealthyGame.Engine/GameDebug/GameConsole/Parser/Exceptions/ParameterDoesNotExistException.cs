using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class ParameterDoesNotExistException : Exception
	{
		public ParameterDoesNotExistException(ParameterStructure parameter, string other)
		{
		}

		public ParameterDoesNotExistException(string[] example, string v)
		{
		}
	}
}
