﻿using System;
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
					case 'Ä':
					case 'ä':
						builder.Append("ae");
						break;
					case 'Ö':
					case 'ö':
						builder.Append("oe");
						break;
					case 'Ü':
					case 'ü':
						builder.Append("ue");
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
