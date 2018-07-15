using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Pathfinding
{
#pragma warning disable CS0659 // Typ überschreibt Object.Equals(object o), überschreibt jedoch nicht Object.GetHashCode()
	public class Node
#pragma warning restore CS0659 // Typ überschreibt Object.Equals(object o), überschreibt jedoch nicht Object.GetHashCode()
	{
		public Node[] Neighbours { get; set; }
		public float Walkspeed { get; set; } = 1f;
		public BasicTile Parent { get; private set; }


		public Node(BasicTile parent)
		{
			this.Parent = parent;
		}

		public void SetNeighbours(params Node[] neighbours	)
		{
			this.Neighbours = neighbours;
		}

		internal float DistanceTo(Node neighbour)
		{
			return (neighbour.Parent.Center - Parent.Center).Length();
		}

		public override bool Equals(object obj)
		{
			if(obj is Node)
			{
				var n = obj as Node;
				bool result = false;
				result |= Walkspeed != n.Walkspeed;
				result |= Parent.Index != n.Parent.Index;
				result |= Neighbours.Length != n.Neighbours.Length;
				return !result;
			}
			return base.Equals(obj);
		}
	}
}
