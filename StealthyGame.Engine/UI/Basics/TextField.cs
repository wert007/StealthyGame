using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.Renderer;
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
		public delegate void TextEventHandler(object sender, string text);
		public event TextEventHandler EnterPressed;
		public event TextEventHandler TextChanged;
		public event TextEventHandler TextTyped;

		public TextField(Control parent): base(parent)
		{
			
			textInput = new TextInput();
			textInput.EnterPressed += (txt) =>
			{
				EnterPressed?.Invoke(this, txt);
			};
			textInput.TextChanged += (txt) =>
			{
				TextChanged?.Invoke(this, txt);
			};
			textInput.TextTyped += (txt) =>
			{
				TextTyped?.Invoke(this, txt);
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

		protected override void _Draw(Renderer2D renderer)
		{
			renderer.Draw(Font, textInput.TypedText, new Vector2(AbsoluteX, AbsoluteY), TextColor);
		}

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
		}
	}
}