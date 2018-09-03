﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Debug.Console;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.UI.Basics;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI
{
	public class ConsoleControl : Control
	{
		TextField input;
		Label label;

		public ConsoleControl(Control parent) : base(parent)
		{
			Margin = new Thickness(5);

			label = new Label(this, ">");
			label.TextColor = Color.White;
			label.Font = Font.CourierNew16;

			input = new TextField(this);
			input.EnterPressed += TextReceived;
			input.TextColor = Color.White;
			input.Font = Font.CourierNew16;
			input.Margin = new Thickness(label.Width, 0, 0, 0);
			Unfocus();
			InGameConsole.SetBufferSize(new Index2(0, (MaxHeight - Font.CourierNew16.PixelHeight) / Font.CourierNew14.PixelHeight));
		}
		
		private void TextReceived(object sender, string text)
		{
			InGameConsole.SendText(text);
			input.Clear();
		}

		public override void Focus()
		{
			if (input.IsEmpty)
				input.Text = "/";
			Visible = true;
			input.Visible = true;
			label.Visible = true;
			input.Focus();
			label.Focus();
			base.Focus();
		}

		public override void Unfocus()
		{
			Visible = false;
			input.Visible = false;
			label.Visible = false;
			input.Unfocus();
			label.Unfocus();
			base.Unfocus();
		}

		protected override void _Draw(SpriteBatch batch)
		{
			batch.DrawFilledRectangle(new Rectangle(AbsoluteX, AbsoluteY, Width, Height), new Color(Color.Black, 0.7f));
			input.Draw(batch);
			label.Draw(batch);
			int yDif = input.Font.PixelHeight;
			foreach (var msg in InGameConsole.MessagesToPrint())
			{
				if (msg.HasBackground)
					batch.DrawFilledRectangle(new Rectangle(AbsoluteX, AbsoluteY + yDif, Width, Font.CourierNew14.PixelHeight), msg.BackgroundColor);
				batch.DrawString(Font.CourierNew14.SpriteFont, msg.Message, new Vector2(AbsoluteX, AbsoluteY + yDif), msg.Color);
				yDif += Font.CourierNew14.PixelHeight;
			}
		}

		protected override void _Update(GameTime time)
		{
		}
	}
}