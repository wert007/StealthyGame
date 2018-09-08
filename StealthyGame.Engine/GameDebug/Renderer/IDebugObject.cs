using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	public interface IDebugObject
	{
		void Draw(Renderer2D renderer);
	}
}