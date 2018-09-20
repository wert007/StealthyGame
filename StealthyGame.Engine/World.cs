using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug;
//using StealthyGame.Engine.Debug;
using StealthyGame.Engine.GameObjects;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.Pathfinding;
using StealthyGame.Engine.View;
using StealthyGame.Engine.View.Lighting;
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
			DebugRenderer.SetMap(map);
			var npcGroups = map.ObjectGroups.Where(g => g.Name.StartsWith("NPC")).ToList();
			foreach (var npc in npcGroups)
			{
				string type = npc.Name.Split(' ')[1];
				LoadNPC(type, npc.Objects);
				DebugRenderer.AddDebugObjectWorld(new DebugNPC(npcs.Last(), Color.Blue)
				{
					ShowVelocity = Color.CornflowerBlue,
					ShowAcceleration = Color.IndianRed,
				});
			}
		}

		protected virtual Map LoadMap(string file, GraphicsDevice graphicsDevice)
		{
			Map result = new Map();
			result.Load(file, graphicsDevice);
			return result;
		}

		protected virtual bool LoadNPC(string type, TiledMapObject[] objects)
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
								wayPoints.Add(new WayPoint(map.GetNode((Index2)o.Position / BasicTile.Size), o.Properties.GetPropertyAsFloat("Duration"), o.Properties.GetPropertyAsInt("Index")));
								break;
							default:
								throw new NotSupportedException("Unknown Type " + o.Type);
						}
					}
					npcs.Add(new RoutedNPC(new Route(true, wayPoints.ToArray()), map));
					break;
				case "Wander":
					foreach (var o in objects)
					{
						switch (o.Type)
						{
							case "Spawn":
								npcs.Add(new WanderingNPC(map.GetNode((Index2)o.Position / BasicTile.Size), map, o.Properties.GetPropertyAsFloat("Tempo")));
								break;
							default:
								throw new NotSupportedException("Unknown Type " + o.Type);
						}
					}
					break;
				default:
					return false;
			}
			return true;
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
