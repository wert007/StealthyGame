using Microsoft.Xna.Framework;
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
		public BasicTile CurrentTile { get; private set; }
		public BasicTile NextTile { get; private set; }
		Path path;
		public float Speed { get; protected set; } = 40;

		bool move;

		/*
		 * speed	offset
		 * 200		3
		 * 400		10
		 * 1000		100
		 */

		public delegate void TargetReachedHandler(BasicTile target);
		public event TargetReachedHandler TargetReached;

		public NPC(BasicTile spawn)
		{
			CurrentTile = spawn;
		}

		protected override void InnerUpdate(GameTime time)
		{
			if (path != null && move)
			{
				if ((NextTile.Center - Center).LengthSquared() < WalkAccuracy())
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
					path.Reached(NextTile.PathfindingNode);
					NextTile = path.Next().Parent;
					if(NextTile is InteractiveTile)
					{
						var inter = NextTile as InteractiveTile;
						inter.Interact(this);
						move = false;
					}
				}
				Vector2 dir = (NextTile.Center - Center);
				dir.Normalize();
				Accelerate(Speed * dir);
			}
			else if(path != null && !move)
			{
				move = (NextTile as InteractiveTile).InteractionDone;
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

		public void WalkTo(BasicTile target)
		{
			if (path == null)
			{
				path = Pathfinder.findPath(CurrentTile.PathfindingNode, target.PathfindingNode);
				move = true; //may not found a valid path
			}
			if (path.IsFailure)
			{
				Console.WriteLine("ERROR: NOT POSSIBLE TO REACH");
			}
			else
				NextTile = path.Next().Parent;
		}
	}
}
