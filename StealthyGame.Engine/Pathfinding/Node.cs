using Microsoft.Xna.Framework;
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
	public class Node
	{
		public Node[] Neighbours { get; set; }
		public float Walkspeed { get; set; } = 1f;
		public Vector2 Position => (Index * 32 + 16);
		public Index2 Index { get; private set; }

		public Node(Index2 index)
		{
			Index = index;
		}

		public void SetNeighbours(params Node[] neighbours	)
		{
			this.Neighbours = neighbours;
		}

		internal float DistanceTo(Node neighbour)
		{
			return (neighbour.Position - Position).Length();
		}

		public override bool Equals(object obj)
		{
			if(obj is Node)
			{
				var n = obj as Node;
				bool result = false;
				result |= Walkspeed != n.Walkspeed;
				result |= Position != n.Position;
				result |= Neighbours.Length != n.Neighbours.Length;
				return !result;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var hashCode = -1933359007;
			hashCode = hashCode * -1521134295 + EqualityComparer<Node[]>.Default.GetHashCode(Neighbours);
			hashCode = hashCode * -1521134295 + Walkspeed.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Node node1, Node node2)
		{
			return EqualityComparer<Node>.Default.Equals(node1, node2);
		}

		public static bool operator !=(Node node1, Node node2)
		{
			return !(node1 == node2);
		}
	}
}
