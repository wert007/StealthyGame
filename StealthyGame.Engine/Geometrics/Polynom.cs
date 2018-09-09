using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StealthyGame.Engine.Helper;
using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.Engine.Geometrics
{
	public class Polynom : Area
	{
		public Index2[] Vertices { get; private set; }
		protected Index2[] Points { get; set; }

		public IEnumerable<Index2> All => Points;
		public Index2 this[int index] => Points[index];

		public Index2 Min { get { return new Index2(Points.Min(p => p.X), Points.Min(p => p.Y)); } }
		public Index2 Max { get { return new Index2(Points.Max(p => p.X), Points.Max(p => p.Y)); } }
		public Index2[] Area { get; private set; }

		public Polynom(params Index2[] points) : base(ToArea(points))
		{
			this.Vertices = points;
		}

		private static Index2[] ToArea(Index2[] points)
		{
			List<Index2> allPoints = new List<Index2>();
			for (int i = 0; i < points.Length; i++)
			{
				if (i == points.Length - 1)
				{
					allPoints.AddRange(Bresenham.LineBetweenTwoPoints(points[i], points[0]));
					break;
				}
				allPoints.AddRange(Bresenham.LineBetweenTwoPoints(points[i], points[i + 1]));
			}
			return allPoints.ToArray();
		}

		public IEnumerator<Index2> GetEnumerator() => ((IEnumerable<Index2>)Points).GetEnumerator();
	//	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Index2>)Points).GetEnumerator();

		public override bool IsInside(Index2 point)
		{
			if (IsEdge(point) ||
				point.X <= Min.X ||
				point.X >= Max.X ||
				point.Y <= Min.Y ||
				point.Y >= Max.Y)
				return false;
			var yVals = Points.Where(p => p.X == point.X).Select(p => p.Y).ToArray();
			var xVals = Points.Where(p => p.Y == point.Y).Select(p => p.X).ToArray();
			
			int minY = int.MaxValue;
			int maxY = int.MinValue;
			int minX = int.MaxValue;
			int maxX = int.MinValue;
			for (int i = 0; i < Math.Max(xVals.Length, yVals.Length); i++)
			{
				if (i < xVals.Length)
				{
					if (minX > xVals[i]) minX = xVals[i];
					if (maxX < xVals[i]) maxX = xVals[i];
				}
				if (i < yVals.Length)
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
			return !IsInside(point) && !IsEdge(point);
		}

		public override bool IsEdge(Index2 point)
		{
			return All.Contains(point);
		}

		public Index2[] FloodFill()
		{
			if (Area != null)
				return Area;
			Index2 start = RandomPointInside();
			List<Index2> result = new List<Index2>();
			Stack<Index2> stack = new Stack<Index2>();
			stack.Push(start);

			while (stack.Count > 0)
			{
				Index2 cur = stack.Pop();

				if (result.Contains(cur) || IsEdge(cur))
				{
					continue;
				}
				result.Add(cur);
				foreach (var p in GetNeighbours(cur))
				{
					stack.Push(p);
				}
			}
			Area = result.ToArray();
			return result.ToArray();
		}

		public Index2[] FloodFill(Index2 point, List<Index2> result)
		{
			if (result.Contains(point) || IsEdge(point))
				return result.ToArray();
			result.Add(point);
			foreach (var p in GetNeighbours(point))
			{
				FloodFill(p, result);
			}
			
			return result.ToArray();
		}

		private Index2[] GetNeighbours(Index2 point)
		{
			List<Index2> result = new List<Index2>();
			if (point.X - 1 > Min.X)
				result.Add(new Index2(point.X - 1, point.Y));
			if (point.X + 1 < Max.X)
				result.Add(new Index2(point.X + 1, point.Y));
			if (point.Y - 1 > Min.Y)
				result.Add(new Index2(point.X, point.Y - 1));
			if (point.Y + 1 < Max.X)
				result.Add(new Index2(point.X, point.Y + 1));
			return result.ToArray();
		}

		private Index2 RandomPointInside()
		{
			Random rand = new Random();
			List<Index2> candidates = new List<Index2>();
			candidates.Add((Min + Max) / 2);
			candidates.AddRange(GetNeighbours((Min + Max) / 2));
			foreach (var point in Vertices)
			{
				candidates.AddRange(GetNeighbours(point));
				candidates.Add(point);
			}
			foreach (var point in candidates)
			{
				if (IsInside(point))
					return point;
			}
			Index2 result = new Index2();
			while (!IsInside(result))
				result = rand.NextIndex2(Min, Max);
			return result;
		}
	}
}
