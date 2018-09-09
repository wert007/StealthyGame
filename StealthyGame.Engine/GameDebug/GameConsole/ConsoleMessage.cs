using Microsoft.Xna.Framework;
using StealthyGame.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Console
{
	public struct ConsoleMessage
	{
		public string Message { get; set; }
		public Color Color { get; set; } 
		public Color BackgroundColor { get; set; }
		public bool HasBackground => BackgroundColor != Color.TransparentBlack;


		public ConsoleMessage(string message)
		{
			Message = message.Sanitize();
			Color = Color.Gray;
			BackgroundColor = Color.TransparentBlack;
		}

		public ConsoleMessage(string message, Color color)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = Color.TransparentBlack;
		}

		public ConsoleMessage(string message, Color color, Color background)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = background;
		}

		public static ConsoleMessage[] Parse(string text)
		{
			List<ConsoleMessage> result = new List<ConsoleMessage>();
			StringBuilder builder = new StringBuilder();
			Color color = Color.Gray;
			Color background = Color.TransparentBlack;
			for (int i = 0; i < text.Length; i++)
			{
				switch (text[i])
				{
					case '<':
						i++;
						switch (text[i])
						{
							case 'c':
								i += 2;
								int length = text.IndexOf('>', i) - i;
								switch (length)
								{
									case 3:
										{
											int r = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											int g = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											int b = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											color = new Color(r, g, b);
											break;
										}
									case 4:
										{
											int r = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											int g = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											int b = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											int a = 15 + 16 * int.Parse(text[i++].ToString(), NumberStyles.HexNumber);
											color = new Color(r, g, b, a);
											break;
										}
									case 6:
										{
											int r = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											int g = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											int b = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											color = new Color(r, g, b);
											break;
										}
									case 8:
										{
											int r = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											int g = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											int b = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											int a = int.Parse(text[i++].ToString() + text[i++].ToString(), NumberStyles.HexNumber);
											color = new Color(r, g, b, a);
											break;
										}
									default:
										throw new NotSupportedException();
								}
								break;
							default:
								throw new NotSupportedException();
						}
						break;
					case '\n':
						result.Add(new ConsoleMessage(builder.ToString(), color, background));
						builder.Clear();
						color = Color.Gray;
						background = Color.TransparentBlack;
						break;
					default:
						builder.Append(text[i]);
						break;
				}
			}
			return result.ToArray();
		}

		public void Intend(int intendation)
		{
			Message = (new string('\t', intendation) + Message).Sanitize();
		}
	}
}
