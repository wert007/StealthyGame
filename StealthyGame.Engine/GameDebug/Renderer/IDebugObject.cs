using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	public interface IDebugObject
	{
		void Update(UpdateContainer container);
		void Draw(Renderer2D renderer, GameTime time);
	}
}