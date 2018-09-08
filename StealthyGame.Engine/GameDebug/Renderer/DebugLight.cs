using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.View.Lighting;

namespace StealthyGame.Engine.GameDebug
{
	public class DebugLight : IDebugObject
	{
		Circle circle;
		int thickness = 3;
		Color color;

		public DebugLight(Light light, Color color)
		{
			circle = light.Area;
			this.color = color;
		}

		public void Draw(Renderer2D renderer)
		{
			renderer.DrawCircle(circle, color, thickness);
			renderer.DrawFilledRectangle(new Rectangle(circle.Center.X - 5, circle.Center.Y - 5, 10, 10), color);
		}
	}
}
