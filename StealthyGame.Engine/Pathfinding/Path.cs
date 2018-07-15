using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Pathfinding
{
	class Path
	{
		List<Node> path;
		public bool IsFailure { get; private set; }
		public bool IsDone { get { return index == path.Count - 1; } }
		public int index;


		public Path(Node current)
		{
			path = new List<Node>();
			index = 0;
			IsFailure = current == null;

			Add(current);
		}

		public void Add(Node current)
		{
			path.Reverse();
			path.Add(current);
			path.Reverse();
		}

		public void Reached(Node node)
		{
			index++;
		}

		public Node Next()
		{
			return path[index];
		}
	}
}
