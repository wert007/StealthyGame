using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameObjects.Collisionables
{
	class CollisionCircle : ICollisionable
	{
		public Vector2 Position { get; }
		public float Radius { get; }

		public CollisionCircle(Vector2 position, float radius)
		{
			this.Position = position;
			this.Radius = radius;
		}

		public CollisionCircle(Circle circle)
		{
			Position = circle.Center;
			Radius = circle.Radius;
		}

		public bool Collide(ICollisionable other)
		{
			if (other is CollisionCircle circle)
			{
				Vector2 dif = circle.Position - Position;
				return dif.LengthSquared() == Radius * Radius + circle.Radius * Radius;
			}
			else if (other is CollisionBox box)
			{
				if (Collide(box.LeftTop))
					return true;
				if (Collide(box.RightTop))
					return true;
				if (Collide(box.LeftBottom))
					return true;
				if (Collide(box.RightBottom))
					return true;
				if (Position.X > box.X && Position.X < box.Right)
				{
					return box.Collide(Position + new Vector2(0, Radius)) ||
						box.Collide(Position - new Vector2(0, Radius));
				}
				if (Position.Y > box.Y && Position.Y < box.Bottom)
				{
					return box.Collide(Position + new Vector2(Radius, 0)) ||
						box.Collide(Position - new Vector2(Radius, 0));
				}
				return false;
			}
			else throw new NotImplementedException("Unknown type: " + other.GetType().Name);
		}

		public bool Intersects(ICollisionable other)
		{
			if (other is CollisionCircle circle)
			{
				Vector2 dif = circle.Position - Position;
				return dif.LengthSquared() < Radius * Radius + circle.Radius * Radius;
			}
			else if (other is CollisionBox box)
			{
				if (box.Intersects(Position))
					return true;
				if (Intersects(box.LeftTop))
					return true;
				if (Intersects(box.RightTop))
					return true;
				if (Intersects(box.LeftBottom))
					return true;
				if (Intersects(box.RightBottom))
					return true;
				if (Position.X > box.X && Position.X < box.Right)
				{
					return box.Intersects(Position + new Vector2(0, Radius)) ||
						box.Intersects(Position - new Vector2(0, Radius));
				}
				if (Position.Y > box.Y && Position.Y < box.Bottom)
				{
					return box.Intersects(Position + new Vector2(Radius, 0)) ||
						box.Intersects(Position - new Vector2(Radius, 0));
				}
				return false;
			}
			else throw new NotImplementedException("Unknown Type: " + other.GetType().Name);
		}

		public bool Collide(Vector2 vector)
		{
			return (Position - vector).LengthSquared() < Radius * Radius;
		}

		public bool Intersects(Vector2 vector)
		{
			return (Position - vector).LengthSquared() == Radius * Radius;
		}
	}
}
