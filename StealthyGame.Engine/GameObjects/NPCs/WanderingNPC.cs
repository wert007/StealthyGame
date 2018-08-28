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
	public class WanderingNPC : NPC
	{
		static Random rand;
		float breakDuration;
		float timeWaited;
		float tempo;


		public WanderingNPC(Node spawn, Map map, float tempo) : base(spawn, map)
		{
			if (rand == null)
				rand = new Random();
			this.tempo = tempo;
			breakDuration = 0.5f;
			TargetReached += WanderingNPC_TargetReached;
			WanderingNPC_TargetReached(spawn);
		}

		private void WanderingNPC_TargetReached(Node target)
		{
			Speed = tempo * rand.Next(5) + 15;
			timeWaited = breakDuration / (10 * tempo);
			int len = rand.Next(8) + 5;
			List<Node> nodes = new List<Node>();
			Node start = target;
			Node fin = start;
			while (fin == start)
			{
				Node c = start;
				nodes.Add(c);
				while(start.DistanceTo(c) < 100 * tempo)
				{
					c = c.Neighbours[rand.Next(c.Neighbours.Length)];
					nodes.Add(c);
				}
				fin = nodes.Last();
				nodes.Clear();
			}
			WalkTo(fin);
		}

		protected override void InnerUpdate(GameTime time)
		{
			if(timeWaited > 0)
			{
				timeWaited -= (float)time.ElapsedGameTime.TotalSeconds;
				return;
			}
			base.InnerUpdate(time);
			if(rand.Next((int)(500 * tempo)) == 0) //1%
			{
				timeWaited = breakDuration;
				breakDuration = (rand.Next(20) / tempo) / 10f + 4f;
			}
		}
	}
}
