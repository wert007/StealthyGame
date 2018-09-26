using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class NameAlreadyUsedException : Exception
	{
		public NameAlreadyUsedException(string message, ParameterStructure parameter, ParameterStructure parameterStructure) : base(message)
		{
		}
	}
}
