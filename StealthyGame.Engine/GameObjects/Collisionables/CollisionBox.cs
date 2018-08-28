using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameObjects.Collisionables
{
	class CollisionBox : ICollisionable
	{
		public int X { get; }
		public int Y { get; }
		public int Width { get; }
		public int Height { get; }
		public int Right { get { return X + Width; } }
		public int Bottom { get { return Y + Height; } }
		public int CenterX { get { return X + Width / 2; } }
		public int CenterY { get { return Y + Height / 2; } }

		public Vector2 LeftTop { get { return new Vector2(X, Y); } }
		public Vector2 LeftBottom { get { return new Vector2(X, Bottom); } }
		public Vector2 RightTop { get { return new Vector2(Right, Y); } }
		public Vector2 RightBottom { get { return new Vector2(Right, Bottom); } }

		public CollisionBox(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public CollisionBox(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }
		public CollisionBox(Index2 position, Index2 size) : this(position.X, position.Y, size.X, size.Y) { }


		public bool Collide(ICollisionable other)
		{
			if(other is CollisionBox box)
			{
				if (Y > box.Y && Y < box.Bottom ||
					Bottom > box.Y && Bottom < box.Bottom) //Collision on the X-Axis
				{
					if (X == box.Right || Right == box.X)
						return true;
				}
				else if (X > box.X && X < box.Right ||
					Right > box.X && Right < box.Right) //Collision on the Y-Axis
				{
					if (Y == box.Bottom || Bottom == box.Y)
						return true;
				}
			}
			else if (other is CollisionCircle circle)
			{
				if (circle.Collide(LeftTop))
					return true;
				if (circle.Collide(RightTop))
					return true;
				if (circle.Collide(LeftBottom))
					return true;
				if (circle.Collide(RightBottom))
					return true;
				if (circle.Position.X > X && circle.Position.X < Right)
				{
					return Collide(circle.Position + new Vector2(0, circle.Radius)) ||
						Collide(circle.Position - new Vector2(0, circle.Radius));
				}
				if (circle.Position.Y > Y && circle.Position.Y < Bottom)
				{
					return Collide(circle.Position + new Vector2(circle.Radius, 0)) ||
						Collide(circle.Position - new Vector2(circle.Radius, 0));
				}
				return false;
			}
			else
			{
				throw new NotSupportedException("Unknown Type " + other.GetType().Name);
			}
			return false;
		}

		public bool Intersects(ICollisionable other)
		{
			if (other is CollisionBox box)
			{
				if (Y > box.Y && Y < box.Bottom ||
					Bottom > box.Y && Bottom < box.Bottom) //Collision on the X-Axis
				{
					if (X > box.X && X < box.Right ||
						Right > box.X && Right < box.Right)
						return true;
				}
				else if (X > box.X && X < box.Right ||
					Right > box.X && Right < box.Right) //Collision on the Y-Axis
				{
					if (Y > box.Y && Y < box.Bottom ||
						Bottom > box.Y && Bottom < box.Bottom)
						return true;
				}
			}
			else if (other is CollisionCircle circle)
			{
				if (Intersects(circle.Position))
					return true;
				if (circle.Intersects(LeftTop))
					return true;
				if (circle.Intersects(RightTop))
					return true;
				if (circle.Intersects(LeftBottom))
					return true;
				if (circle.Intersects(RightBottom))
					return true;
				if (circle.Position.X > X && circle.Position.X < Right)
				{
					return Intersects(circle.Position + new Vector2(0, circle.Radius)) ||
						Intersects(circle.Position - new Vector2(0, circle.Radius));
				}
				if (circle.Position.Y > Y && circle.Position.Y < Bottom)
				{
					return Intersects(circle.Position + new Vector2(circle.Radius, 0)) ||
						Intersects(circle.Position - new Vector2(circle.Radius, 0));
				}
				return false;
			}
			else
			{
				throw new NotSupportedException("Unknown Type " + other.GetType().Name);
			}
			return false;
		}

		public bool Collide(Vector2 vector)
		{
			return vector.X > X && vector.Y > Y &&
				vector.X < Right && vector.Y < Bottom;
		}

		public bool Intersects(Vector2 vector)
		{
			return ((vector.X == X  || vector.X == Right) && (vector.Y > Y && vector.Y < Bottom))
				|| ((vector.Y == Y || vector.Y == Bottom) && (vector.X > X && vector.X < Right));
				
		}
	}
}
