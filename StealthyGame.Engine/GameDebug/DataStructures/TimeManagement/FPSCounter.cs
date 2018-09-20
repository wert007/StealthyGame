using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameMechanics.Phases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures.TimeManagement
{
	public class FPSCounter
	{
		Game game;
		public int DrawFPS { get; private set; }
		public int UpdateFPS { get; private set; }

		public FPSCounter(Game game)
		{
			this.game = game;
		}

		public void Update(UpdateContainer container)
		{
			UpdateFPS = (int)(1 / container.Time.ElapsedGameTime.TotalSeconds);
		}

		public void UpdateDraw(GameTime time)
		{
			DrawFPS = (int)(1 / time.ElapsedGameTime.TotalSeconds);
		}
	}
}
