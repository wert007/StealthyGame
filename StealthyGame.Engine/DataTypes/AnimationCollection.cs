using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameDebug.GameConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public class AnimationCollection
	{
		List<Animation2D> animations;
		Animation2D current;
		public int Length => (animations?.Count).HasValue ? animations.Count : 0;
		public string StartAnimation { get; private set; }
		public delegate void AnimationChangedHandler(string name);
		public event AnimationChangedHandler AnimationChanged;

		public AnimationCollection(string startAnimation)
		{
			animations = new List<Animation2D>();
			StartAnimation = startAnimation;
		}

		public void Play(string name)
		{
			current = GetByName(name);
			if (current == null)
				GameConsole.Error("No animation named " + name);
			current.Reset();
			AnimationChanged?.Invoke(name);
		}

		public Animation2D GetCurrent(int i = -1)
		{
			if (i < 0)
				return current;
			return animations[i];
		}

		public void Update(GameTime time) => current.Update(time);

		public void AddAnimation(Animation2D animation)
		{
			animations.Add(animation);
			Play(StartAnimation);
		}

		public void AddAnimations(Animation2D[] animations)
		{
			this.animations.AddRange(animations);
			Play(StartAnimation);
		}

		public Animation2D GetByName(string name)
		{
			foreach (var animation in animations)
			{
				if (animation.Name == name)
					return animation;
			}
			return null;
		}

		public Animation2D[] GetAnimations() => animations.ToArray();

	}
}
