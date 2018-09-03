using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI.Basics
{
	public class TextField : Control
	{
		public Color TextColor { get; set; }
		public Font Font { get; set; }
		public string Text
		{
			get { return textInput.TypedText; }
			set { textInput.SetText(value); }
		}

		public bool IsEmpty => Text.Length <= 0;

		TextInput textInput;
		public delegate void EnterPressedEventHandler(object sender, string text);
		public event EnterPressedEventHandler EnterPressed;

		public TextField(Control parent): base(parent)
		{
			
			textInput = new TextInput();
			textInput.EnterPressed += (txt) =>
			{
				EnterPressed?.Invoke(this, txt);
			};
			TextColor = Color.Black;
			Font = Font.Arial11;
		}

		public void Clear()
		{
			textInput.ClearText();
		}

		protected override void _TextInput(object sender, TextInputEventArgs e)
		{
			textInput.Add(e);
		}

		protected override void _Draw(SpriteBatch batch)
		{
			batch.DrawString(Font.SpriteFont, textInput.TypedText, new Vector2(AbsoluteX, AbsoluteY), TextColor);
		}

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
		}
	}
}