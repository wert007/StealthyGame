using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.Engine.GameMechanics.Phases
{
	public class PhaseContainer
	{
		public PhaseContainer(GraphicsDevice graphicsDevice)
		{
			GraphicsDevice = graphicsDevice;
		}
		public GraphicsDevice GraphicsDevice { get; private set; }
	}
}