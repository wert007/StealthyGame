using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.Pathfinding;
using StealthyGame.Engine.View;
using StealthyGame.MapBasics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.MapBasics;
using StealthyGame.GameObjects.NPCs;

namespace StealthyGame
{
	class MyWorld : World
	{
		List<RightfulNPC> rnpcs;
		public MyWorld(Camera cam) : base(cam)
		{
			rnpcs = new List<RightfulNPC>();
		}

		protected override Map LoadMap(string file, GraphicsDevice graphicsDevice)
		{
			MyMap result = new MyMap();
			result.Load(file, graphicsDevice);
			return result;
		}

		protected override void LoadNPC(string type, TiledMapObject[] objects)
		{
			base.LoadNPC(type, objects);
			switch (type)
			{
				default:
					Console.WriteLine("Unknown Type: " + type);
					break;
			}
		}
	}
}
