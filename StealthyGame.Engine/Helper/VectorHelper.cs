using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class VectorHelper
	{
		public static Vector2 RetNormalize(this Vector2 vector)
		{
			Vector2 ret = vector.Copy();
			ret.Normalize();
			return ret;
		}

		public static Vector2 Copy(this Vector2 vector)
		{
			Vector2 ret = new Vector2(vector.X, vector.Y);
			return ret;
		}
	}
}
