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
	public class Icon : Control
	{
		Texture2D texture;

		public Icon(Control parent, Texture2D texture) : base(parent)
		{
			this.texture = texture;
			MinHeight = texture.Height;
			MinWidth = texture.Width;
			VerticalAlignment = VerticalAlignment.Top;
			HorizontalAlignment = HorizontalAlignment.Left;
		}

		protected override void _Draw(SpriteBatch batch)
		{
			batch.Draw(texture, new Vector2(AbsoluteX, AbsoluteY), Color.White);
		}

		protected override void _Update(GameTime time)
		{
		}
	}
}
