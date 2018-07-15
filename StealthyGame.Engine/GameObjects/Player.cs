using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics.Tiles;

namespace StealthyGame.Engine.GameObjects
{
	class Player : NPC
	{
		public Player(BasicTile spawn) : base(spawn)
		{
		}
	}
}
