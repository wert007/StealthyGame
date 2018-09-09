using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameDebug.Console;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
using StealthyGame.Engine.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameObjects.NPCs
{
	public class NPC : GameObject
	{
		public Node CurrentTile { get; private set; }
		public Node NextTile { get; private set; }
		Path path;
		public float Speed { get; protected set; } = 40;
		Map map;

		bool move;

		/*
		 * speed	offset
		 * 200		3
		 * 400		10
		 * 1000		100
		 */

		public delegate void TargetReachedHandler(Node target);
		public event TargetReachedHandler TargetReached;

		public NPC(Node spawn, Map map)
		{
			CurrentTile = spawn;
			this.map = map;
		}

		protected override void InnerUpdate(GameTime time)
		{
			if (path != null && move)
			{
				if ((NextTile.Position - Center).LengthSquared() < WalkAccuracy())
				{
					if (path.IsDone)
					{
						CurrentTile = NextTile;
						path = null;
						move = false;
						TargetReached?.Invoke(NextTile);
						return;
					}
					CurrentTile = NextTile;
					path.Reached(NextTile);
					NextTile = path.Next();
					if(map.IsInteractive(NextTile))
					{
						map.GetInteractiveTiles(NextTile)[0].Interact(this);
						move = false;
					}
				}
				Vector2 dir = (NextTile.Position - Center);
				dir.Normalize();
				Accelerate(Speed * dir);
			}
			else if(path != null && !move)
			{
				move = map.GetInteractiveTiles(NextTile)[0].InteractionDone;
			}
		}

		private float WalkAccuracy()
		{
			if (Speed > 400)
				return Speed / 10;
			if (Speed > 200)
				return 10;
			if (Speed > 30)
				return 3;
			else
				return 1;
		}

		public void WalkTo(Node target)
		{
			if (path == null)
			{
				path = Pathfinder.findPath(CurrentTile, target);
				move = true; //may not found a valid path
			}
			if (path.IsFailure)
			{
				GameConsole.Error("ERROR: NOT POSSIBLE TO REACH");
			}
			else
				NextTile = path.Next();
		}
	}
}
