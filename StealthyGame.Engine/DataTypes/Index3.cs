using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public struct Index3
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public Index3(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		
		public Index3(Index2 pos, int z) : this(pos.X, pos.Y, z) { }
		public Index3(Index2 pos) : this(pos, 0) { }
		public Index3(Index3 clone) : this(clone.X, clone.Y, clone.Z) { }

		public static Index3 operator +(Index3 a, Index3 b) => new Index3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		public static Index3 operator -(Index3 a, Index3 b) => new Index3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		public static Index3 operator *(Index3 a, Index3 b) => new Index3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		public static Index3 operator /(Index3 a, Index3 b) => new Index3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

		public static bool operator ==(Index3 a, Index3 b) => a.Equals(b);
		public static bool operator !=(Index3 a, Index3 b) => !a.Equals(b);

		public static implicit operator Vector3(Index3 a) => new Vector3(a.X, a.Y, a.Z);
		public static explicit operator Index3(Vector3 a) => new Index3((int) a.X, (int) a.Y, (int) a.Z);

		public Index2 ToIndex2() => new Index2(X, Y);

		public override bool Equals(object obj)
		{
			if (obj is Index3 index)
				return X == index.X &&
						 Y == index.Y &&
						 Z == index.Z;
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var hashCode = -307843816;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			hashCode = hashCode * -1521134295 + Z.GetHashCode();
			return hashCode;
		}
	}
}
