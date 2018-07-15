using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Pathfinding
{
	public class WayPoint
	{
		public Node Node { get; private set; }
		public float Duration { get; private set; }
		public int Index { get; private set; }

		public WayPoint(Node node, float duration, int index)
		{
			this.Node = node;
			this.Duration = duration;
			this.Index = index;
		}

	}
}
