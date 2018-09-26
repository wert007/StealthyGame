using Microsoft.Xna.Framework;
using StealthyGame.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.GameConsole
{
	public struct ConsoleMessage
	{
		public string Message { get; set; }
		public Color Color { get; set; } 
		public Color BackgroundColor { get; set; }
		public bool HasBackground => BackgroundColor != Color.TransparentBlack;
		public MessageType Type { get; set; }

		public ConsoleMessage(string message)
		{
			Message = message.Sanitize();
			Color = Color.Gray;
			BackgroundColor = Color.TransparentBlack;
			Type = MessageType.Message;
		}

		public ConsoleMessage(string message, Color color)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = Color.TransparentBlack;
			Type = MessageType.Message;
		}

		public ConsoleMessage(string message, Color color, MessageType type)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = Color.TransparentBlack;
			Type = type;
		}

		public ConsoleMessage(string message, Color color, Color background, MessageType type)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = background;
			Type = type;
		}

		public ConsoleMessage(string message, Color color, Color background)
		{
			Message = message.Sanitize();
			Color = color;
			BackgroundColor = background;
			Type = MessageType.Message;
		}

		public ConsoleMessage(string message, MessageType type)
		{
			Type = type;
			Message = message.Sanitize();
			BackgroundColor = Color.TransparentBlack;
			switch (type)
			{
				case MessageType.Warning:
					Color = Color.Yellow;
					break;
				case MessageType.Error:
					Color = Color.Red;
					break;
				case MessageType.TerminatingError:
					Color = Color.LightGray;
					BackgroundColor = Color.Red;
					break;
				case MessageType.Message:
					Color = Color.Gray;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public static ConsoleMessage[] Create(string text, MessageType type)
		{
			string[] lines = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			ConsoleMessage[] result = new ConsoleMessage[lines.Length];
			for (int i = 0; i < lines.Length; i++)
			{
				result[i] = new ConsoleMessage(lines[i], type);
			}
			return result;
		}

		public static ConsoleMessage[] Parse(string text)
		{
			List<ConsoleMessage> result = new List<ConsoleMessage>();
			StringBuilder builder = new StringBuilder();
			Color color = Color.Gray;
			Color background = Color.TransparentBlack;
			MessageType type = MessageType.Message;
			bool usedColorCoding = false;
			for (int i = 0; i < text.Length; i++)
			{
				switch (text[i])
				{
					case '<':
						i++;
						switch (text[i])
						{
							case 'c':	//Color is in hex defined for this line.
								usedColorCoding = true;
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
							case 't':	//MessageType is defined for this line (w=Warning, e=Error, t=TerminatingError)
								i += 2;
								switch (text[i++])
								{
									case 'w':
										type = MessageType.Warning;
										break;
									case 'e':
										type = MessageType.Error;
										break;
									case 't':
										type = MessageType.TerminatingError;
										break;
									default:
										throw new NotSupportedException();
								}
								break;
							case 'a':	//Message has an action linked. Don't ask further questions, I don't know (n)either.	
								break;
							default:
								throw new NotSupportedException();
						}
						break;
					case '\n':
						if (usedColorCoding)
							result.Add(new ConsoleMessage(builder.ToString(), color, background, type));
						else
							result.Add(new ConsoleMessage(builder.ToString(), type));
						builder.Clear();
						color = Color.Gray;
						background = Color.TransparentBlack;
						type = MessageType.Message;
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

		public override string ToString()
		{
			return Message;
		}
	}

	[Flags]
	public enum MessageType
	{
		Warning				= 1 << 0,
		Error					= 1 << 1,
		TerminatingError	= 1 << 2,
		Message				= 1 << 3,

		Level0 = Message,
		Level1 = Level0 | Warning,
		Level2 = Level1 | Error,
		Level3 = Level2 | TerminatingError,
		All = Warning | Error | TerminatingError | Message,
	}
}
