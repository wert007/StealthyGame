using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Wordpress.Fields
{
	class RaycastStoringField : Field
	{
		Queue<Index2> points;

		public RaycastStoringField(string name) : base(name)
		{
			points = new Queue<Index2>();
		}

		protected override void PreCompute()
		{
			Light l = new Light(lightPosition, radius, 0.5f, new HSVColor(Color.PaleTurquoise));
			foreach (var endPoint in circle.All)
			{
				foreach (var point in new LightRaycast(l, lightPosition, endPoint))
				{
					if (obstacles[point.X, point.Y] > 0) break;
					if(!points.Contains(point))
					points.Enqueue(point);
				}
			}
		}

		protected override void ComputeSingleStep()
		{
			if(points.Count <= 0)
			{
				Done = true;
				return;
			}
			Current = points.Dequeue();
		}
	}
}
