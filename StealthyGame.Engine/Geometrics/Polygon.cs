using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.Geometrics
{
	public class Polygon : Area
	{
		public Polygon(Polynom orig) : base(orig.Vertices)
		{

		}

		public Polygon(Index2[] points) : base(points)
		{
		}

		public static Polygon operator-(Polygon a, Polygon b)
		{
			return null;
		}
	}
}
