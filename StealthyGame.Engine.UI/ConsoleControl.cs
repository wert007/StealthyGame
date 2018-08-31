using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Debug.Console;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI
{
	public class ConsoleControl : Control
	{
		TextField input;

		public ConsoleControl(Control parent) : base(parent)
		{
			input = new TextField(this);
			input.EnterPressed += TextReceived;
			input.TextColor = Color.White;
			input.Font = Font.Arial16;
			Unfocus();
		}
		
		private void TextReceived(object sender, string text)
		{
			InGameConsole.SendText(text);
			input.Clear();
		}

		public override void Focus()
		{
			Visible = true;
			input.Visible = true;
			input.Focus();
			base.Focus();
		}

		public override void Unfocus()
		{
			Visible = false;
			input.Visible = false;
			input.Unfocus();
			base.Unfocus();
		}

		protected override void _Draw(SpriteBatch batch)
		{
			batch.DrawFilledRectangle(new Rectangle(AbsoluteX, AbsoluteY, Width, Height), new Color(Color.Black, 0.7f));
			input.Draw(batch);
			int yDif = (int)input.Font.SpriteFont.MeasureString(input.Text).Y;
			foreach (var msg in InGameConsole.MessagesToPrint())
			{
				batch.DrawString(Font.Arial11.SpriteFont, msg, new Vector2(AbsoluteX, AbsoluteY + yDif), Color.Green);
				yDif += 11;
			}
		}

		protected override void _Update(GameTime time)
		{
		}
	}
}
