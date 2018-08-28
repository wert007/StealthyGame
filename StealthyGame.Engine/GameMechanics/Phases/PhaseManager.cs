using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.Engine.GameMechanics.Phases
{
	public class PhaseManager
	{
		Phase current;

		public PhaseManager(Phase start)
		{
			current = start;
		}
		
		public Phase GetCurrentPhase()
		{
			return current;
		}

		public void Load(ContentManager content)
		{
			GetCurrentPhase().Load(content);
		}

		public void Update(GameTime time)
		{
			GetCurrentPhase().Update(time);
		}

		public void Draw(SpriteBatch batch, GameTime time)
		{
			GetCurrentPhase().Draw(batch, time);
		}
	}
}
