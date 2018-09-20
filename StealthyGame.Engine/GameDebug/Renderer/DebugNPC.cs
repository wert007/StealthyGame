using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	internal class DebugNPC : IDebugObject
	{
		private NPC nPC;
		private Color color;

		public DebugNPC(NPC nPC, Color color)
		{
			this.nPC = nPC;
			this.color = color;
		}

		public Color? ShowVelocity { get; set; }
		public Color? ShowAcceleration { get; set; }

		public void Draw(Renderer2D renderer, GameTime time)
		{
			renderer.DrawFilledRectangle(new Rectangle((int)nPC.Position.X, (int)nPC.Position.Y, (int)nPC.Size.X, (int)nPC.Size.Y), color);
			if (ShowVelocity.HasValue)
				renderer.DrawVector(nPC.Velocity, nPC.Center, ShowVelocity.Value, 2);
			if (ShowAcceleration.HasValue)
				renderer.DrawVector(nPC.Acceleration, nPC.Center, ShowAcceleration.Value, 1);
		}

		public void Update(UpdateContainer container)
		{
		}
	}
}