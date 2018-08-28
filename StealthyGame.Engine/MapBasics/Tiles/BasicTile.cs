using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
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
		public Index2 Index { get; private set; }
		public Index2 Position => Index * Size;
		public Vector2 Center => Position + Size / 2f;
		public static int Size { get; private set; } = 32;
		public TiledProperties Properties { get; private set; }
		public AnimationCollection Animations { get; protected set; }

		public static void SetTileSize(int size) => Size = size;

		public BasicTile(string name, Index2 index)
		{
			this.Name = name;
			this.Index = index;
			Animations = null;
		}

		public void AddProperties(TiledProperties properties) => Properties += properties;


		/// <summary>
		/// Maybe we should use Reflections at some point. #justsayin
		/// </summary>
		/// <param name="interactive"></param>
		/// <param name="animation"></param>
		/// <param name="properties"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static BasicTile LoadTile(bool interactive, bool animation, TiledProperties properties, Index2 position)
		{
			BasicTile result = null;
			if (interactive && animation)
			{
				switch (properties.GetProperty("Type"))
				{
					case "CellDoor":
						//result = new CellDoor("cellDoor", position);
						//break;
					case "Door":
						result = new DoorTile("door", position);
						break;
					default:
						result = new InteractiveTile("door", position);
						break;
				}
			}
			else if (animation && !interactive)
			{
				result = new AnimatedTile("flower", position);
			}
			else if (!interactive && !animation)
			{
				result = new BasicTile("dirt", position);
			}
			else
				throw new NotSupportedException("No Tile should be interacitve but not animated");
			result.AddProperties(properties);
			return result;
		}
	}
}
