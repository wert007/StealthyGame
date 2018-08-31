using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Helper;

namespace StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles
{
	public class InteractiveTile : BasicTile
	{
		protected float durationOfInteraction;
		float currentDuration;
		public bool InteractionDone => currentDuration <= 0;
		public Animation2D CurrentAnimation => Animations.GetCurrent();

		public InteractiveTile(string name, Index2 index) : base(name, index)
		{
			durationOfInteraction = 1f;
			Animations = new AnimationCollection("idle");
		}

		public void Load(string file, GraphicsDevice graphicsDevice)
		{
			Animations.AddAnimations(AnimationFile.Load(file, graphicsDevice));
		}

		public void Interact(object sender)
		{
			currentDuration = durationOfInteraction;
			InnerInteract(sender);
		}

		protected virtual void InnerInteract(object sender)
		{

		}

		public override void Update(GameTime time)
		{

			Animations.Update(time);
			if (currentDuration > 0)
			{
				currentDuration -= (float)time.ElapsedGameTime.TotalSeconds;
			}
			InnerUpdate(time);
		}

		protected virtual void InnerUpdate(GameTime time)
		{

		}

		public void PlayAnimation(string name)
		{
			Animations.GetCurrent().Reset();
			Animations.Play(name);
		}

		public void Draw(SpriteBatch batch, Color color)
		{
			batch.Draw(Animations, new Rectangle(Position.X, Position.Y, Size, Size), color);
		}
	}
}
