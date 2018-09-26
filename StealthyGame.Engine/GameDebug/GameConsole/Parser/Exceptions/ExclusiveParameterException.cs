using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class ExclusiveParameterException : Exception
	{
		public ExclusiveParameterException(ParameterStructure result) : base(CreateMessage(result))
		{
		}

		private static string CreateMessage(ParameterStructure result)
		{
			string message = "Parameter " + result.Names[0] + " is Exclusive. Therefore you can't use it with other parameters.";
			string location = new PositionException(result.Flags, null).Message;
			string solution = "Remove the exclusive flag in line " + result.Flags.Line + " or don't set others here.";
			return message + Environment.NewLine + location + Environment.NewLine + solution + Environment.NewLine;
		}
	}
}
