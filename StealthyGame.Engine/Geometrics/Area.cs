using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	public class Area
	{
		public IEnumerable<Index2> All { get { return Points; } }
		Index2 this[int index] { get { return Points[index]; } }
		public Index2 Min { get; }
		public Index2 Max { get; }

		List<Index2> Points { get; }
		public virtual bool IsInside(Index2 point) 
			=> Points.Contains(point);

		public virtual bool IsOutside(Index2 point)
			=> !IsInside(point);
		public virtual bool IsEdge(Index2 point)
			=> false;

		public Area(Index2[] points)
		{
			Points = new List<Index2>();
			Points.AddRange(points);
		}

		public void Add(Index2 index)
		{
			Points.Add(index);
		}
	}

}
