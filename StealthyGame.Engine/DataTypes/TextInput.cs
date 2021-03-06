﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public class TextInput
	{
		public string TypedText { get; private set; }
		List<TextInputEventArgs> textInputEventArgs;

		public delegate void TextEventHandler(string text);
		public event TextEventHandler EnterPressed;
		public event TextEventHandler TextChanged;
		public event TextEventHandler TextTyped;

		public TextInput()
		{
			TypedText = string.Empty;
			textInputEventArgs = new List<TextInputEventArgs>();
		}

		public void Add(TextInputEventArgs e)
		{	
			textInputEventArgs.Add(e);
			if (e.Character != 13 &&
				e.Character != 8 &&
				e.Character >= 1 &&
				e.Character <= 31)
				return;
			switch (e.Character)
			{
				case '\b': //DEL
					if (TypedText.Length > 0)
						TypedText = TypedText.Remove(TypedText.Length - 1);
					break;
				case '\t': //TAB
					int c = TypedText.Length % 4;
					TypedText += new string(' ', 4 - c);
					break;
				case '\r': //ENTER
					EnterPressed?.Invoke(TypedText);
					break;
				case '\u007f': //CTRL+DEL
					if (TypedText.Length > 0)
						TypedText = TypedText.Remove(Math.Max(TypedText.LastIndexOf(' '), 0));
					break;
				default:
					TypedText += e.Character;
					break;
			}
			TextChanged?.Invoke(TypedText);
			TextTyped?.Invoke(TypedText);
		}

		public void SetText(string text)
		{
			TypedText = string.Empty;
			textInputEventArgs.Clear();
			TypedText = text;
			TextChanged?.Invoke(TypedText);
		}

		public void ClearText()
		{
			TypedText = string.Empty;
			textInputEventArgs.Clear();
			TextChanged?.Invoke(TypedText);
		}
	}
}
