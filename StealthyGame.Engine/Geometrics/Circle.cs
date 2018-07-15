using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	public class Circle : Area
	{
		public int Radius { get; protected set; }
		public Index2 Center { get; protected set; }
		public Index2 Min { get { return Center - Radius; } }
		public Index2 Max { get { return Center + Radius; } }
		//protected Index2[] Points { get; set; }
		public Index2[] Area { get; private set; }
		

		public Circle(Index2 center, int radius)
			: this(Bresenham.Circle(center, radius))
		{
			Center = center;
			Radius = radius;
		}

		protected Circle(Index2[] points) : base(points)
		{

		}

		public override bool IsInside(Index2 point)
		{
			if (IsEdge(point) ||
				point.X <= Min.X ||
				point.X >= Max.X ||
				point.Y <= Min.Y ||
				point.Y >= Max.Y)
				return false;
			var xVals = All.Where(p => p.X == point.X).Select(p => p.X).ToArray();
			var yVals = All.Where(p => p.Y == point.Y).Select(p => p.Y).ToArray();
			int minY = int.MaxValue;
			int maxY = int.MinValue;
			int minX = int.MaxValue;
			int maxX = int.MinValue;
			for (int i = 0; i < Math.Max(xVals.Length, yVals.Length); i++)
			{
				if(i < xVals.Length)
				{
					if (minX > xVals[i]) minX = xVals[i];
					if (maxX < xVals[i]) maxX = xVals[i];
				}
				if(i < yVals.Length)
				{
					if (minY > yVals[i]) minY = yVals[i];
					if (maxY < yVals[i]) maxY = yVals[i];
				}
			}
			return point.X > minX && point.X < maxX
				&& point.Y > minY && point.Y < maxY;
		}

		public override bool IsOutside(Index2 point)
		{
			return (point - Center).LengthSq() > Radius * Radius;
		}

		public override bool IsEdge(Index2 point)
		{
			return All.Contains(point);
		}

		public Index2[] SpiralArea(bool justInnerCircle)
		{
			if (justInnerCircle && Area != null)
				return Area;
			List<Index2> result = new List<Index2>();
			Index2 tile = Center;
			int steps = 1;
			int dir = 0;
			result.Add(tile);
			while(steps < 2 * Radius)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int i = 0; i < steps; i++)
					{
						tile += GetDirection(dir);
						if(justInnerCircle)
							if (IsOutside(tile))
								continue;
						result.Add(tile);
					}
					dir = (dir + 1) % 4;
				}
				steps++;
				
			}
			result.Remove(result.Last());
			if (justInnerCircle)
				Area = result.ToArray();
			return result.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i">0 = Top, 1 = Right...</param>
		/// <returns></returns>
		private Index2 GetDirection(int i)
		{
			switch (i)
			{
				case 0:
					return new Index2(0, -1);
				case 1:
					return new Index2(1, 0);
				case 2:
					return new Index2(0, 1);
				case 3:
					return new Index2(-1, 0);
				default:
					throw new ArgumentException();
			}
		}
	}
}
