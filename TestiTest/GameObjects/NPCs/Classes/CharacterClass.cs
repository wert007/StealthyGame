using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameObjects.NPCs.Classes
{
	public abstract class CharacterClass : NPC
	{
		public CharacterClass(BasicTile spawn) : base(spawn)
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
