﻿using Microsoft.Xna.Framework;
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
		
		Dictionary<Keys, KeyData> keyData;
		const int MinMillisecondsPressed =   20;
		const int MaxMillisecondsPressed =  400;

		public KeyboardManager()
		{
			keyData = new Dictionary<Keys, KeyData>();
		}

		public void Update(GameTime time)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			UpdateKeyData(keyboardState.GetPressedKeys(), time);
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
				Console.WriteLine(keyData[key].MillisecondsDown + " " + keyData[key].IsPressed);
			}
			return result; 
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
