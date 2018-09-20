using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameDebug.DataStructures.TimeManagement;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.UI;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.GameDebug.Renderer
{
	public class DebugFPS : IDebugObject
	{
		FPSCounter counter;

		public DebugFPS(FPSCounter counter)
		{
			this.counter = counter;
		}

		public void Update(UpdateContainer container)
		{
		}

		public void Draw(Renderer2D renderer, GameTime time)
		{
			renderer.Draw(Font.CourierNew14, "UpdateFPS: " + counter.UpdateFPS + "\nDrawFPS: " + counter.DrawFPS, new Vector2(), Color.Green);
			if(time.IsRunningSlowly)
			{
				renderer.DrawRectangle(new Rectangle(0, 0, Control.RenderWidth - 5, Control.RenderHeight - 5), Color.Red, 5);
			}
		}
	}
}
