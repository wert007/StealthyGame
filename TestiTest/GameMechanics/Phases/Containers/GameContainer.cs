using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameMechanics.Phases.Containers
{
	public class GameContainer : PhaseContainer
	{
		public Team Team { get; private set; }
		public GameContainer(GraphicsDevice graphicsDevice, Team team) : base(graphicsDevice)
		{
			Team = team;
		}
	}
}
