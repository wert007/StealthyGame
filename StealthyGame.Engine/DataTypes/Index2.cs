using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public struct Index2
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Index2(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public int LengthSq() => X * X + Y * Y;

		public float Length() => (float) Math.Sqrt(LengthSq());

		public override string ToString() => "X: " + X + ", Y: " + Y;
		public override bool Equals(object obj)
		{
			if (obj is Index2 i)
				return X == i.X && Y == i.Y;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var hashCode = 0x6EF2E3D3;
			hashCode = hashCode * -0x5AAAAAD7 + base.GetHashCode();
			hashCode = hashCode * -0x5AAAAAD7 + X.GetHashCode();
			hashCode = hashCode * -0x5AAAAAD7 + Y.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Index2 a, Index2 b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(Index2 a, Index2 b) => !(a.X == b.X && a.Y == b.Y);

		public static Index2 operator +(Index2 a, Index2 b) => new Index2(a.X + b.X, a.Y + b.Y);
		public static Index2 operator -(Index2 a, Index2 b) => new Index2(a.X - b.X, a.Y - b.Y);
		public static Index2 operator *(Index2 a, Index2 b) => new Index2(a.X * b.X, a.Y * b.Y);
		public static Index2 operator /(Index2 a, Index2 b) => new Index2(a.X / b.X, a.Y / b.Y);

		public static Index2 operator +(Index2 a, int b) => new Index2(a.X + b, a.Y + b);
		public static Index2 operator -(Index2 a, int b) => new Index2(a.X - b, a.Y - b);
		public static Index2 operator *(Index2 a, int b) => new Index2(a.X * b, a.Y * b);
		public static Index2 operator /(Index2 a, int b) => new Index2(a.X / b, a.Y / b);

		public static Index2 operator +(Index2 a, float b) => new Index2((int)(a.X + b), (int)(a.Y + b));
		public static Index2 operator -(Index2 a, float b) => new Index2((int)(a.X - b), (int)(a.Y - b));
		public static Index2 operator *(Index2 a, float b) => new Index2((int)(a.X * b), (int)(a.Y * b));
		public static Index2 operator /(Index2 a, float b) => new Index2((int)(a.X / b), (int)(a.Y / b));

		public static implicit operator Vector2(Index2 a) => new Vector2(a.X, a.Y);
		public static explicit operator Index2(Vector2 a) => new Index2((int) a.X, (int) a.Y);
	}

	public static class Index2Helper
	{
		public static bool Intersects(this Rectangle rect, Index2 index) => rect.Intersects(new Rectangle(index.X, index.Y, 1, 1));

		public static bool Intersects(this Index2 index, Rectangle rect) => rect.Intersects(index);

		public static Index2 ToIndex2(this Vector2 vector) => new Index2((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
	}
}
