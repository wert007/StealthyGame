using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class PositionException : Exception
	{
		public PositionException(CommandFileLine position, Exception innerException) : base(CreateMessage(position, innerException), innerException)
		{

		}

		private static string CreateMessage(CommandFileLine line, Exception innerException)
		{
			string position = "An error occured at line " + line.Line + " in " + line.Parent.FileName + ":";
			if (innerException != null)
				position = innerException.GetType().Name + " occured at Line " + line.Line + " in " + line.Parent.FileName + ":";
			string code = (line.Line - 1) + "::" + line.Before.Content + Environment.NewLine +
								line.Line + ":: " + line.Content + Environment.NewLine +
								(line.Line + 1) + "::" + line.After.Content;
			if (innerException != null)
			{
				string exception = Environment.NewLine + innerException.Message;

				return position + Environment.NewLine + code + Environment.NewLine + exception;
			}
			return position + Environment.NewLine + code;
		}
	}
}
