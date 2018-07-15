using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class GeometricsHelper
	{
		public static Area ToArea(this Rectangle rectangle)
		{
			Index2[] area = new Index2[rectangle.Width * rectangle.Height];
			int i = 0;
			for (int x = 0; x < rectangle.Width; x++)
			{
				for (int y = 0; y < rectangle.Height; y++)
				{
					area[i] = new Index2(x, y);
					i++;
				}
			}
			return new Area(area);
		}
	}
}
