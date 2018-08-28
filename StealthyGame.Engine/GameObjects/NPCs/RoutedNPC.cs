using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.Pathfinding;
using StealthyGame.Engine.MapBasics.Tiles;

namespace StealthyGame.Engine.GameObjects.NPCs
{
	public class RoutedNPC : NPC
	{
		Route route;
		float pause;
		float paused;
		bool targetReached;

		public RoutedNPC(Route route, Map map) : base(route[0], map)
		{
			this.route = route;
			TargetReached += RoutedNPC_TargetReached;
			WalkTo(route.GetTarget());
		}

		private void RoutedNPC_TargetReached(Node target)
		{
			pause = route.GetDuration();
			route.TargetReached();
			targetReached = true;
		}

		protected override void InnerUpdate(GameTime time)
		{
			base.InnerUpdate(time);
			if(paused < pause)
			{
				paused += (float)time.ElapsedGameTime.TotalSeconds;
			}
			else if(targetReached)
			{
				WalkTo(route.GetTarget());
				pause = 0;
				paused = 0;
				targetReached = false;
			}
		}
	}
}
