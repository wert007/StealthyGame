using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.GameObjects;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.Pathfinding;
using StealthyGame.Engine.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine
{
	public class World
	{
		public Map map;
		Camera cam;
		public List<NPC> npcs;

		public World(Camera cam)
		{
			this.cam = cam;
			npcs = new List<NPC>();
		}

		public void Load(string file, GraphicsDevice graphicsDevice)
		{
			map = LoadMap(file, graphicsDevice);
			DebugSpriteBatch.SetMap(map);
			var a = map.ObjectGroups.Where(g => g.Name.StartsWith("NPC")).ToList();
			foreach (var b in a)
			{
				string type = b.Name.Split(' ')[1];
				LoadNPC(type, b.Objects);
				DebugSpriteBatch.AddGameObject(new DebugNPC(npcs.Last(), Color.Blue)
				{
					ShowVelocity = true,
					ShowAcceleration = true,
				});
			}
		}

		protected virtual Map LoadMap(string file, GraphicsDevice graphicsDevice)
		{
			Map result = new Map();
			result.Load(file, graphicsDevice);
			return result;
		}

		protected virtual void LoadNPC(string type, TiledMapObject[] objects)
		{
			switch (type)
			{
				case "Route":
					List<WayPoint> wayPoints = new List<WayPoint>();
					foreach (var o in objects)
					{
						switch (o.Type)
						{
							case "WayPoint":
								wayPoints.Add(new WayPoint(map.GetTile((Index2)o.Position / BasicTile.Size).PathfindingNode, o.Properties.GetPropertyAsFloat("Duration"), o.Properties.GetPropertyAsInt("Index")));
								break;
							default:
								throw new NotSupportedException("Unknown Type " + o.Type);
						}
					}
					npcs.Add(new RoutedNPC(new Route(true, wayPoints.ToArray())));
					break;
				case "Wander":
					foreach (var o in objects)
					{
						switch (o.Type)
						{
							case "Spawn":
								npcs.Add(new WanderingNPC(map.GetTile((Index2)o.Position / BasicTile.Size), o.Properties.GetPropertyAsFloat("Tempo")));
								break;
							default:
								throw new NotSupportedException("Unknown Type " + o.Type);
						}
					}
					break;
				default:
					Console.WriteLine("Unknown Type: " + type);
					break;
			}
		}

		public void Update(GameTime time)
		{
			map.Update(time);
			foreach (var npc in npcs)
			{
				npc.Update(time);
			}
		}
	}
}
