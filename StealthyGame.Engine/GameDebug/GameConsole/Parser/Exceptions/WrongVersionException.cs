using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser.Exceptions
{
	public class WrongVersionException : Exception
	{
		public int[] SupportedVersions { get; private set; }
		public int ReceivedVersion { get; private set; }
		public WrongVersionException(int[] supported, int received) : base(CreateMessage(supported, received))
		{
			SupportedVersions = supported;
			ReceivedVersion = received;
		}

		private static string CreateMessage(int[] supported, int received)
		{
			string noSupport = "This parser doesn't support version " + received + ".";
			string shouldUse = "This parser doesn't support any version. Please check out github.com/wert007/StealthyGame since this is obviousle an error.";
			if(supported.Length > 0)
			{
				if (supported.Length == 1)
					shouldUse = "Only version " + supported[0] + " is supported by this parser.";
				else
					shouldUse = "This parser supports the following versions: " + string.Join(", ", supported);
			}
			return noSupport + Environment.NewLine + shouldUse;
		}
	}
}
