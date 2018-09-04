using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Input;
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

		public void Draw(SpriteBatch batch)
		{
			Keys[] keys = keyboard.GetPressedKeys();
			for (int i = 0; i < keys.Length; i++)
			{
				batch.DrawFilledRectangle(new Rectangle(0, Font.Arial16.PixelHeight * i, 100, Font.Arial16.PixelHeight), Color.Red);
				batch.DrawString(Font.Arial16.SpriteFont, keys[i].ToString(), new Vector2(0, Font.Arial16.PixelHeight * i), Color.White);
			}
		}
	}
}
