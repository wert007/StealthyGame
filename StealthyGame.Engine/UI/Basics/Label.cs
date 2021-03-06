﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI.Basics
{
	public class Label : Control
	{
		private string _content;
		public string Content
		{
			get { return _content; }
			set
			{
				_content = value;
				if (Font == null)
					return;
				MinWidth = (int)(Font.SpriteFont.MeasureString(_content).X);
				MinHeight = Font.PixelHeight;
			}
		}
		public Font Font { get; set; }
		public Color TextColor { get; set; }

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
			TextColor = Color.Black;
			Font = Font.Arial11;
			MinWidth = (int)Font.SpriteFont.MeasureString(Content).X;
			MinHeight = (int)Font.SpriteFont.MeasureString(Content).Y;
			VerticalAlignment = VerticalAlignment.Top;
			HorizontalAlignment = HorizontalAlignment.Left;
		}

		protected override void _Draw(Renderer2D renderer)
		{
			renderer.Draw(Font, Content, new Vector2(AbsoluteX, AbsoluteY), TextColor);
		}
		

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
		}
	}
}
