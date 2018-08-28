using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public class Light
	{
		public Circle Area { get; private set; }
		public Index2 Position
		{
			get { return Area.Center; }
			set { Area.Center = value; }
		}

		public int Radius { get { return Area.Radius; } }
		public HSV Color { get; private set; }				
		public float Brightness { get; private set; }

		public Light(Index2 position, int radius, float brightness, Color color)
		{
			Area = new Circle(position, radius);
			Color = new HSV(color);
			Brightness = brightness;
		}

		public Light(Index2 position, int radius, float brightness, HSV color)
		{
			Area = new Circle(position, radius);
			Color = color;
			Brightness = brightness;
		}
	}
}
