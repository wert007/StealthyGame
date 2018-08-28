using Microsoft.Xna.Framework;
using StealthyGame.Engine.Geometrics.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	public class Line
	{
		public Vector2 A { get; protected set; }
		public Vector2 B { get; protected set; }

		public Line(Vector2 a, Vector2 b)
		{
			A = a;
			B = b;
			if(a.LengthSquared() < b.LengthSquared())
			{
				B = a;
				A = b;
			}
		}

		public Vector2? IntersectionPoint(Line other)
		{
			LinearExpression a = new LinearExpression(this);
			LinearExpression b = new LinearExpression(other);
			Vector2? intersection = a.IntersectionPoint(b);
			if(intersection.HasValue)
			{
				if (intersection.Value.X > A.X && intersection.Value.X < B.X &&
					intersection.Value.Y > A.Y && intersection.Value.Y < B.Y)
					return intersection.Value;
			}
			return null;
		}
	}
}
