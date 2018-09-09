using System;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.Input;

namespace StealthyGame.Engine.GameMechanics.Phases
{
	public class UpdateContainer
	{
		public KeyboardManager Keyboard { get; private set; }
		public GameTime Time { get; private set; }

		public UpdateContainer()
		{
			Keyboard = new KeyboardManager();
		}

		public void Update(GameTime time)
		{
			Keyboard.Update(time);
			Time = time;
		}

		public void EndUpdate()
		{
			Keyboard.EndUpdate();
		}

		public void TextInput(object sender, TextInputEventArgs e)
		{
			Keyboard.TextInput(sender, e);
		}
	}
}