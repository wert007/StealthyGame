using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.MapBasics.Tiles
{
	public class BasicTile
	{
		public string Name { get; private set; }
		public Index3 Index { get; private set; }
		public Index3 Position => new Index3(Index.ToIndex2() * Size, Index.Z);
		public Vector2 Center => Position.ToIndex2() + Size / 2;
		public static int Size { get; private set; } = 32;
		public Node PathfindingNode { get; private set; }
		public TiledProperties Properties { get; private set; }
		public AnimationCollection Animations { get; protected set; }
		public float DrawOrder { get; }

		public static void SetTileSize(int size) => Size = size;

		public BasicTile(string name, Index3 index, float drawOrder)
		{
			this.Name = name;
			DrawOrder = drawOrder;
			this.Index = index;
			this.PathfindingNode = new Node(this);
			Animations = null;
		}

		public void AddProperties(TiledProperties properties) => Properties += properties;
	}
}
