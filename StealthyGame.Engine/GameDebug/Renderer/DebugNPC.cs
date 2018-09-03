using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameObjects.NPCs;
using StealthyGame.Engine.Helper;

namespace StealthyGame.Engine.Debug
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

		public void Draw(SpriteBatch batch)
		{
			batch.DrawFilledRectangle(new Rectangle((int)nPC.Position.X, (int)nPC.Position.Y, (int)nPC.Size.X, (int)nPC.Size.Y), color);
			if (ShowVelocity.HasValue)
				batch.DrawVector(nPC.Velocity, nPC.Center, ShowVelocity.Value, 2);
			if (ShowAcceleration.HasValue)
				batch.DrawVector(nPC.Acceleration, nPC.Center, ShowAcceleration.Value, 1);
		}
	}
}