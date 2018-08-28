using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiTest.GameMechanics.Phases
{
	class TeamSelectingPhase : Phase
	{
		public TeamSelectingPhase(PhaseContainer container) : base(PhaseType.PreGame, typeof(GamePhase))
		{
			
		}

		public override void Draw(SpriteBatch batch, GameTime time)
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
