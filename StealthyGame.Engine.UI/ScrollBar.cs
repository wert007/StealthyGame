using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI
{
	public class ScrollBar : Control
	{
		public int MinValue { get; set; }
		public int MaxValue { get; set; }
		public int Value { get; set; }
		public Orientation Orientation { get; set; }
		private float ValuePercent => (float)(Value - MinValue) / (MaxValue - MinValue);

		public ScrollBar(Control parent, Orientation orientation) : base(parent)
		{
			MinValue = 0;
			MaxValue = 100;
			Value = 0;
			Orientation = orientation;
			MinWidth = 20;
			MinHeight = 20;
		}

		public override void Draw(SpriteBatch batch)
		{
			batch.DrawFilledRectangle(new Rectangle(AbsoluteX, AbsoluteY, Width, Height), Color.LightCoral);
			switch (Orientation)
			{
				case Orientation.Vertical:
					batch.DrawFilledRectangle(new Rectangle(AbsoluteX, (int)(AbsoluteY + RenderHeight * ValuePercent - 0.2f * Height), Width, (int)(0.2f * Height)), Color.Coral);
					break;
				case Orientation.Horizontal:
					batch.DrawFilledRectangle(new Rectangle((int)(AbsoluteX + RenderWidth * ValuePercent), AbsoluteY, (int)(0.2f * (Width - 0.2f * Width)), Height), Color.Coral);
					break;
				default:
					throw new NotImplementedException();
			}
		}

		protected override void _Update(GameTime time)
		{
		}
	}
}
