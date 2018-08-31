using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI.Basics
{
	public class Label : Control
	{
		public string Content { get; set; }
		public Font Font { get; set; }
		public Color Color { get; set; }

		public Label(Control parent) : base(parent) 
		{
			Content = String.Empty;
			Initialize();
		}

		public Label(Control parent, string content) : base(parent)
		{
			Content = content;
			Initialize();
		}

		protected void Initialize()
		{
			Color = Color.Black;
			Font = Font.Arial11;
			MinWidth = (int)Font.SpriteFont.MeasureString(Content).X;
			MinHeight = (int)Font.SpriteFont.MeasureString(Content).Y;
			VerticalAlignment = VerticalAlignment.Top;
			HorizontalAlignment = HorizontalAlignment.Left;
		}

		protected override void _Draw(SpriteBatch batch)
		{
			batch.DrawString(Font.SpriteFont, Content, new Vector2(AbsoluteX, AbsoluteY), Color);
		}

		protected override void _Update(GameTime time)
		{
		}
	}
}
