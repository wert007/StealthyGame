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
		int x;
		int y;
		int w;
		int h;

		public int X { get { return x; } }
		public int Y { get { return y; } }
		public int Width { get { return w; } }
		public int Height { get { return h; } }
		public int Right { get { return x + w; } }
		public int Bottom { get { return y + h; } }
		public int CenterX { get { return x + w / 2; } }
		public int CenterY { get { return y + h / 2; } }

		public CollisionBox(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.w = width;
			this.h = height;
		}

		public CollisionBox(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }
		public CollisionBox(Index2 position, Index2 size) : this(position.X, position.Y, size.X, size.Y) { }


		public bool Collide(ICollisionable other)
		{
			if(other is CollisionBox)
			{
				CollisionBox c = other as CollisionBox;
				if (Y > c.Y && Y < c.Bottom ||
					Bottom > c.Y && Bottom < c.Bottom) //Collision on the X-Axis
				{
					if (X == c.Right || Right == c.X)
						return true;
				}
				else if (X > c.X && X < c.Right ||
					Right > c.X && Right < c.Right) //Collision on the Y-Axis
				{
					if (Y == c.Bottom || Bottom == c.Y)
						return true;
				}
			}
			else
			{
				throw new NotSupportedException("Unknown Type " + other.GetType().Name);
			}
			return false;
		}

		public bool Intersects(ICollisionable other)
		{
			if (other is CollisionBox)
			{
				CollisionBox c = other as CollisionBox;
				if (Y > c.Y && Y < c.Bottom ||
					Bottom > c.Y && Bottom < c.Bottom) //Collision on the X-Axis
				{
					if (X > c.X && X < c.Right ||
						Right > c.X && Right < c.Right)
						return true;
				}
				else if (X > c.X && X < c.Right ||
					Right > c.X && Right < c.Right) //Collision on the Y-Axis
				{
					if (Y > c.Y && Y < c.Bottom ||
						Bottom > c.Y && Bottom < c.Bottom)
						return true;
				}
			}
			else
			{
				throw new NotSupportedException("Unknown Type " + other.GetType().Name);
			}
			return false;
		}
	}
}
