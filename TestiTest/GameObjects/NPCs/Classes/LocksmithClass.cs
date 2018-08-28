using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.Pathfinding;

namespace TestiTest.GameObjects.NPCs.Classes
{
	public abstract class LocksmithClass : CharacterClass
	{
		public LocksmithClass(Node spawn, Map map) : base(spawn, map)
		{
		}
	}
}
