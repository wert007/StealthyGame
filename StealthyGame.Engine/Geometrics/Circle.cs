using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	public class Circle
	{
		public int Radius { get; protected set; }
		public Index2 Center { get; set; }
		public Index2 Min { get { return Center - Radius; } }
		public Index2 Max { get { return Center + Radius; } }


		public Circle(Index2 center, int radius)
		{
			Center = center;
			Radius = radius;
		}
		
		//public Index2[] SpiralArea(bool justInnerCircle)
		//{
		//	if (justInnerCircle && Area != null)
		//		return Area;
		//	List<Index2> result = new List<Index2>();
		//	Index2 tile = Center;
		//	int steps = 1;
		//	int dir = 0;
		//	result.Add(tile);
		//	while(steps < 2 * Radius)
		//	{
		//		for (int j = 0; j < 2; j++)
		//		{
		//			for (int i = 0; i < steps; i++)
		//			{
		//				tile += GetDirection(dir);
		//				if(justInnerCircle)
		//					if (IsOutside(tile))
		//						continue;
		//				result.Add(tile);
		//			}
		//			dir = (dir + 1) % 4;
		//		}
		//		steps++;
				
		//	}
		//	result.Remove(result.Last());
		//	if (justInnerCircle)
		//		Area = result.ToArray();
		//	return result.ToArray();
		//}

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

		public static Index2[] CalculateEdge(Index2 position, int radius)
		{
			return Bresenham.Circle(position, radius);
		}

		public Index2[] CalculateEdge()
		{
			return CalculateEdge(Center, Radius);
		}
	}
}
