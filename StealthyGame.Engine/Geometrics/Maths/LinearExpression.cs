using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics.Maths
{
	public class LinearExpression
	{
		float m;
		float t;

		public LinearExpression(Line line): this(line.A, line.B)
		{

		}

		public LinearExpression(Vector2 start, Vector2 end)
		{
			Vector2 dif = end - start;
			m = dif.Y / dif.X;
			t = -(m * start.X - start.Y);
		}

		public float f(float x)
		{
			return m * x + t;
		}

		public Vector2? IntersectionPoint(LinearExpression other)
		{
			if (m == other.m)
				return null;
			float x = (other.t - t) / (m - other.m);
			float y = m * x + t;
			return new Vector2(x, y);
		}
	}
}
