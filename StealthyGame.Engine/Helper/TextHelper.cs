using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class TextHelper
	{
		public static string Sanitize(this string str)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				switch (str[i])
				{
					case '\t':
						int c = i % 4;
						builder.Append(' ', 4 - c);
						break;
					default:
						builder.Append(str[i]);
						break;
				}
			}
			return builder.ToString();
		}
	}
}
