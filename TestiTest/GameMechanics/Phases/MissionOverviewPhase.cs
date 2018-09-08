using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameMechanics.Phases
{
	class MissionOverviewPhase : Phase
	{
		public MissionOverviewPhase(PhaseContainer container) : base(PhaseType.PreGame, typeof(TeamSelectingPhase))
		{
		}

		public override void Draw(Renderer2D renderer, GameTime time)
		{
			throw new NotImplementedException();
		}

		public override void Load(ContentManager content)
		{
			throw new NotImplementedException();
		}

		public override void Update(GameTime time)
		{
			throw new NotImplementedException();
		}
	}
}
