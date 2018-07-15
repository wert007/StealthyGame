using Microsoft.Xna.Framework;
using StealthyGame.Engine.GameObjects.Collisionables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameObjects
{
    public abstract class GameObject
    {
		public ICollisionable collision;
		public Vector2 Position { get; private set; }
		public Vector2 Center { get { return Position + Size / 2; } }
		public Vector2 Size { get; private set; } = new Vector2(32);
		public Vector2 Velocity { get; private set; }
		public Vector2 Acceleration { get; private set; }
		private float friction = 0.25f;

		public GameObject()
		{

		}

		protected GameObject(GameObject clone)
		{
			this.collision = clone.collision;
			this.Position = clone.Position;
			this.Size = clone.Size;
			this.Velocity = clone.Velocity;
			this.Acceleration = clone.Acceleration;
			this.friction = clone.friction;
		}

		public void Update(GameTime time)
		{
			Velocity += Acceleration;
			Acceleration = Vector2.Zero;
			Position += Velocity * (float)time.ElapsedGameTime.TotalSeconds;
			Velocity *= (1 - friction);
			InnerUpdate(time);
		}

		protected abstract void InnerUpdate(GameTime time);

		public void Accelerate(Vector2 direction)
		{
			Acceleration += direction;
		}

		public bool Collide(GameObject other)
		{
			return collision.Collide(other.collision);
		}

		public bool Intersects(GameObject other)
		{
			return collision.Intersects(other.collision);
		}
	}
}
