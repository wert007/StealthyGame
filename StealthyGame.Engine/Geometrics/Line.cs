using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace StealthyGame.Engine.Geometrics
{
	public class Line : IEnumerable<Index2>, ILine
	{
		protected Index2[] Points { get; set; }
		public Index2 Min { get { return new Index2(Points.Min(p => p.X), Points.Min(p => p.Y)); } }
		public Index2 Max { get { return new Index2(Points.Max(p => p.X), Points.Max(p => p.Y)); } }
		public int Count { get { return Points.Length; } }

		public Index2 this[int index] => Points[index];
		public IEnumerable<Index2> All => Points;

		public Line(Index2 start, Index2 end) 
			: this(Bresenham.LineBetweenTwoPoints(start, end))	{	}

		protected Line(Index2[] points)
		{
			Points = points;
		}

		public int LengthSquared() => (Max.X * Max.X) + (Max.Y * Max.Y);

		public float Length() => (float)Math.Sqrt(LengthSquared());

		public bool Contains(Index2 index) => IndexOf(index) >= 0;

		public int IndexOf(Index2 point)
		{
			if (point == First())
				return 0;
			Vector2 guessed = Last() - point;
			int len = (int)Math.Max(Math.Abs(guessed.X), Math.Abs(guessed.Y));
			if (len >= Count)
				return -1;

			Vector2 t = guessed.RetNormalize();
			Vector2 dir = Last() - First();
			dir.Normalize();
			if (t != dir) return -1;
			int index = Count - len;
			for (int i = Math.Max(index - 3, 0); i < Math.Max(index, 0) + 3 && i < Count; i++)
			{
				if (point == this[i]) return i;
			}
			throw new NotImplementedException();
		}




		public Index2 First() => this[0];
		public Index2 Last() => this[Count - 1];

		public IEnumerator<Index2> GetEnumerator() => ((IEnumerable<Index2>)Points).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Index2>)Points).GetEnumerator();
	}
}
