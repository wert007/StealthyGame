﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug.GameConsole;
using StealthyGame.Engine.MapBasics.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public struct AnimatedTileLighting
	{
		public Index2 Position { get; private set; }
		public Index2 Index { get; private set; }
		AnimationCollection animations;
		public int Size => BasicTile.Size;

		public AnimatedTileLighting(BasicTile tile)
		{
			Position = tile.Position;
			Index = tile.Index;
			var array = tile.Animations.GetAnimations().Where(a => a.Name.EndsWith("shadow")).ToArray();
			animations = new AnimationCollection(tile.Animations.StartAnimation + "shadow");
			if (array.Length <= 0)
			{
				GameConsole.Warning("Warning: No shadow animations found.");
				return;
			}
			animations.AddAnimations(array);
			tile.Animations.AnimationChanged += AnimationChanged;
		}

		private void AnimationChanged(string name)
		{
			animations.Play(name + "shadow");
		}

		public void Update(GameTime time)
		{
			animations.Update(time);
		}

		public Texture2D Current()
		{
			return animations.GetCurrent().GetCurrentTexture();
		}
	}
}
