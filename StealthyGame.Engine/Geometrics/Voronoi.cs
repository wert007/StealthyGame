using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	public class Voronoi
	{
		Index2[] points;
		Polynom[] areas;
		static Random random = new Random();

		public Voronoi(params Index2[] points)
		{
			this.points = points;
			List<Polynom> areas = new List<Polynom>();
			foreach (var point in points)
			{
				List<Index2> middles = new List<Index2>();
				foreach (var otherPoint in points)
				{
					if(otherPoint != point)
						middles.Add((point + otherPoint) / 2);
				}
				areas.Add(new Polynom(middles.ToArray()));
			}
			this.areas = areas.ToArray();
		}

		public void FunDraw(Renderer2D renderer)
		{
			foreach (var area in areas)
			{
				Color c = random.NextColor();
				renderer.DrawFilledPolygon(area, c);
			}
		}

		public static Area[] Generate(Area area, Index2[] points)
		{
			int minY = area.Min.Y;
			int maxY = area.Max.Y;
			int minX = area.Min.X;
			int maxX = area.Max.X;
			Queue<Index2> oPoints = new Queue<Index2>();
			foreach (var p in points.OrderBy(p => p.X))
				oPoints.Enqueue(p);
			Queue<int> pointsOfInterest = new Queue<int>();
			Area[] areas = new Area[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				areas[i] = new Area(new Index2[0]);
				areas[i].Add(oPoints.ToArray()[i]);
			}
			int sweepline = minX;
			int oldsweepline = minX;
			while (oPoints.Count > 0)
			{
				oldsweepline = sweepline;
				sweepline = oPoints.Dequeue().X;
				int minNotEnq = int.MaxValue;
				for (int i = 0; i < points.Length; i++)
				{
					if (points[i].X <= sweepline && !pointsOfInterest.Contains(i))
						pointsOfInterest.Enqueue(i);
					else
						minNotEnq = Math.Min(minNotEnq, points[i].X);
				}
				foreach (var p in area.All.Where(a => a.X < sweepline && a.X >= minNotEnq))
					foreach (var poi in pointsOfInterest)
						if (Distance(p, points[poi]) < Math.Pow(sweepline - p.X, 2))
							areas[poi].Add(p);
			}
			sweepline = maxX;
			for (int i = 0; i < points.Length; i++)
				if (points[i].X <= sweepline && !pointsOfInterest.Contains(i))
					pointsOfInterest.Enqueue(i);
			foreach (var p in area.All.Where(a => a.X < sweepline))
				foreach (var poi in pointsOfInterest)
					if (Distance(p, points[poi]) < Math.Pow(sweepline - p.X, 2))
						areas[poi].Add(p);
			return areas;
		}

		public static float Distance(Index2 a, Index2 b)
		{
			return (a - b).LengthSq();
		}
	}
}
