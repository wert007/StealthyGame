using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public class Light
	{
		public Index2 Position { get; private set; }
		public HSVColor Color { get; private set; }
		public int Radius { get; private set; }
		public float Brightness { get; private set; }

		public Light(Index2 position, int radius, float brightness, HSVColor color)
		{
			Position = position;
			Color = color;
			Radius = radius;
			Brightness = brightness;
		}
	}
}
