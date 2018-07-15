using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Pathfinding
{
	public class Route
	{
		WayPoint[] wayPoints;
		int index;
		bool isLooped;

		public Node this[int key] { get { return wayPoints[key].Node; } }

		public Route(bool isLooped, params WayPoint[] wayPoints)
		{
			this.isLooped = isLooped;
			this.wayPoints = wayPoints;
			this.wayPoints.OrderBy(w => w.Index);
		}

		public Node GetTarget()
		{
			return wayPoints[index].Node;
		}

		public float GetDuration()
		{
			return wayPoints[index].Duration;
		}

		public void TargetReached()
		{
			index++;
			if(index >= wayPoints.Length)
			{
				if (isLooped)
					index = 0;
				else
					index = wayPoints.Length - 1;
			}
		}
	}
}
