using StealthyGame.Engine.GameObjects.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameObjects.NPCs
{
	class RightfulNPC
	{
		public int Rights { get; private set; }
		public NPC NPC { get; private set; }

		public RightfulNPC(NPC npc, int rights)
		{
			NPC = npc;
			Rights = rights;
		}
	}
}
