using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Input
{
	public class KeyboardManager
	{
		KeyboardState lastKeyboardState;
		Dictionary<Keys, KeyData> keyData;
		List<char> pressedSinceLastUpdate;
		const int MinMillisecondsPressed =   20;
		const int MaxMillisecondsPressed =  400;

		public KeyboardManager()
		{
			pressedSinceLastUpdate = new List<char>();
			keyData = new Dictionary<Keys, KeyData>();
		}

		public Keys[] GetPressedKeys()
		{
			return keyData.Keys.ToArray();
		}

		public void Update(GameTime time)
		{
			lastKeyboardState = Keyboard.GetState();
			UpdateKeyData(lastKeyboardState.GetPressedKeys(), time);
		}

		public void EndUpdate()
		{
			pressedSinceLastUpdate.Clear();
		}

		private void UpdateKeyData(Keys[] keys, GameTime time)
		{
			var unpressedKeys = keyData.Keys.Except(keys);
			foreach (var key in keys)
			{
				if (!keyData.ContainsKey(key))
				{
					keyData.Add(key, new KeyData(0, 0));
				}
				else
				{
					keyData[key] = keyData[key].Update(time.ElapsedGameTime.Milliseconds);
				}
			}
			for (int i = unpressedKeys.Count() - 1; i >= 0; i--)
			{ 
				keyData.Remove(unpressedKeys.ElementAt(i));
			}
			
		}

		public bool IsKeyPressed(Keys key)
		{
			bool result = keyData.ContainsKey(key)
				&& keyData[key].MillisecondsDown >= MinMillisecondsPressed
				&& keyData[key].MillisecondsDown <= MaxMillisecondsPressed
				&& !keyData[key].IsPressed;
			if (result)
			{
				keyData[key] = keyData[key].Pressed();
				//Console.WriteLine(keyData[key].MillisecondsDown + " " + keyData[key].IsPressed);
			}
			return result; 
		}

		public void TextInput(object sender, TextInputEventArgs e)
		{
			pressedSinceLastUpdate.Add(e.Character);
		}

		public bool IsCtrlKeyPressed(Keys key)
		{
			if (lastKeyboardState.IsKeyDown(Keys.LeftControl) ||
				lastKeyboardState.IsKeyDown(Keys.RightControl))
				return IsKeyPressed(key);
			return false;
		}

		public bool IsCharPressed(char c)
		{
			return pressedSinceLastUpdate.Contains(c);
		}
	}

	struct KeyData
	{
		public bool IsDown;
		public int TicksDown;
		public int MillisecondsDown;
		public bool IsPressed;

		public KeyData(int ticks, int milliseconds)
		{
			IsDown = true;
			TicksDown = ticks;
			MillisecondsDown = milliseconds;
			IsPressed = false;
		}

		public KeyData(int ticks, int milliseconds, bool pressed)
		{
			IsDown = true;
			TicksDown = ticks;
			MillisecondsDown = milliseconds;
			IsPressed = pressed;
		}

		public KeyData Update(int milliseconds)
		{
			return new KeyData(TicksDown + 1, MillisecondsDown + milliseconds, IsPressed);
		}

		public KeyData Pressed()
		{
			return new KeyData(TicksDown, MillisecondsDown, true);
		}
	}
}
