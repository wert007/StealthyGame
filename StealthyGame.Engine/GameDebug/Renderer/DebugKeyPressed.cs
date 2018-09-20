using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.UI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.Renderer
{
	public class DebugKeyPressed : IDebugObject
	{
		KeyboardManager keyboard;

		public DebugKeyPressed(KeyboardManager keyboard)
		{
			this.keyboard = keyboard;
		}

		public void Draw(Renderer2D renderer, GameTime time)
		{
			Keys[] keys = keyboard.GetPressedKeys();
			for (int i = 0; i < keys.Length; i++)
			{
				renderer.Draw(Font.Arial16, keys[i].ToString(), new Vector2(0, Font.Arial16.PixelHeight * i), Color.White);
				
			}
		}

		public void Update(UpdateContainer container)
		{
		}
	}
}
