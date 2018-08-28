using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameObjects.NPCs.Classes
{
	public abstract class CharacterClass : NPC
	{
		public CharacterClass(Node spawn, Map map) : base(spawn, map)
		{
		}
		

		//TODO:
		//- Locksmith
		//- Lookout
		//- Chavalier
		//- Muscle
		//- Assasinate
	}
}
