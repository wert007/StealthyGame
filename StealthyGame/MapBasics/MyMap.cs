using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
using StealthyGame.Engine.DataTypes;
using StealthyGame.MapBasics.Tiles.InteractiveTiles;
using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.MapBasics
{
	class MyMap : Map
	{
		public MyMap(int width, int height, TiledObjectGroup[] objectGroups, MapLayer[] layers) : base(width, height, objectGroups, layers)
		{

		}
		
		public MyMap() { }

		protected override void LoadTiles(int x, int y, int z, TiledProperties properties, bool interactive, bool animation)
		{
			base.LoadTiles(x, y, z, properties, interactive, animation);
			if(interactive && animation)
			{
				switch(properties.GetProperty("Type"))
				{
					case "CellDoor":
						tiles[x, y] = new CellDoor("cellDoor", new Index3(x, y, z), GetLayer(z).DrawOrder);
						break;

				}
			}
			tiles[x, y].AddProperties(properties);
		}
	}
}
