using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class MultipleDeclarationException : Exception
	{
		public MultipleDeclarationException(CommandFileLine first, CommandFileLine second) : base(CreateMessage(first, second))
		{
		}

		private static string CreateMessage(CommandFileLine first, CommandFileLine second)
		{
			string firstDecl = "First Declaration:" +
				Environment.NewLine + "\t" + first.Line + ":: " + first.Content + Environment.NewLine;
			string secondDecl = "Second Declaration:" + Environment.NewLine +
				"\t" + second.Line + ":: " + second.Content + Environment.NewLine;
			return firstDecl + Environment.NewLine + secondDecl;
		}
	}
}
