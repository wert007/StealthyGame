using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.GameDebug.Console;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.UI.Basics;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI
{
	public class ConsoleControl : Control
	{
		TextField input;
		Label label;
		Label waitingMessages;

		public ConsoleControl(Control parent) : base(parent)
		{
			Margin = new Thickness(15);
			Background = new Color(Color.Black, 0.7f);

			label = new Label(this, ">");
			label.TextColor = Color.White;
			label.Font = Font.CourierNew16;

			waitingMessages = new Label(this);
			waitingMessages.TextColor = Color.Wheat;
			waitingMessages.Font = Font.Arial16;
			waitingMessages.HorizontalAlignment = HorizontalAlignment.Center;
			waitingMessages.VerticalAlignment = VerticalAlignment.Bottom;
			waitingMessages.Background = new Color(Color.Black, 0.7f);

			input = new TextField(this);
			input.EnterPressed += TextReceived;
			input.TextTyped += TextUpdated;
			input.TextColor = Color.White;
			input.Font = Font.CourierNew16;
			input.Margin = new Thickness(label.Width, 0, 0, 0);
			Unfocus();
			GameConsole.SetBufferSize(new Index2(0, (MaxHeight - Font.CourierNew16.PixelHeight) / Font.CourierNew14.PixelHeight - 1));
		}

		private void TextUpdated(object sender, string text)
		{
			GameConsole.UpdateText(text);
		}

		private void TextReceived(object sender, string text)
		{
			GameConsole.SendText(text);
			input.Clear();
		}

		public override void Focus()
		{
			if (input.IsEmpty)
				input.Text = "/";
			Visible = true;
			input.Visible = true;
			label.Visible = true;
			waitingMessages.Visible = true;
			input.Focus();
			label.Focus();
			base.Focus();
		}

		public override void Unfocus()
		{
			Visible = false;
			input.Visible = false;
			label.Visible = false;
			waitingMessages.Visible = false;
			input.Unfocus();
			label.Unfocus();
			base.Unfocus();
		}

		protected override void _Draw(Renderer2D renderer)
		{
			input.Draw(renderer);
			label.Draw(renderer);
			if (GameConsole.WaitingMessages > 0)
			{
				waitingMessages.Content = "(" + GameConsole.WaitingMessages + ") Messages waiting. Press Enter to continue";
				waitingMessages.Draw(renderer);
			}
			int yDif = input.Font.PixelHeight;
			foreach (var msg in GameConsole.MessagesToPrint())
			{
				if (msg.HasBackground)
					renderer.DrawFilledRectangle(new Rectangle(AbsoluteX, AbsoluteY + yDif, Width, Font.CourierNew14.PixelHeight), msg.BackgroundColor);
				renderer.Draw(Font.CourierNew14, msg.Message, new Vector2(AbsoluteX, AbsoluteY + yDif), msg.Color);
				yDif += Font.CourierNew14.PixelHeight;
			}
		}

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
			if (!Focused)
				return;
			if(keyboardManager.IsKeyPressed(Keys.Up))
			{
				input.Text = GameConsole.GetPreviousTyped();
			}
			else if (keyboardManager.IsKeyPressed(Keys.Down))
			{ 
				input.Text = GameConsole.GetNextTyped();
			}
			else if (keyboardManager.IsKeyPressed(Keys.Tab))
			{
				input.Text = GameConsole.GetNextSuggestion();
			}
		}
	}
}
