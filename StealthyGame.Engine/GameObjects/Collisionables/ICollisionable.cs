using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameObjects.Collisionables
{
	public interface ICollisionable
	{
		/// <summary>
		/// Checks if they two collide. No Intersection just touching.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool Collide(ICollisionable other);
		bool Intersects(ICollisionable other);
	}
}
