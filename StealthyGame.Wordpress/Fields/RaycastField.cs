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
	public class RaycastField : Field
	{
		List<Raycast> raycasts;

		public RaycastField(string name) : base(name)
		{
			raycasts = new List<Raycast>();
		}

		protected override void PreCompute()
		{
			Light l = new Light(lightPosition, radius, 0.5f, new HSVColor(Color.PaleTurquoise));
			foreach (var point in circle.All)
			{
				raycasts.Add(new LightRaycast(l, lightPosition, point));
			}
		}

		protected override void ComputeSingleStep()
		{
			if (raycasts.Count == 0)
			{
				Done = true;
				return;
			}
			var cur = raycasts.First();
			if (cur.Count > 0)
			{
				if (obstacles[cur.First().X, cur.First().Y] > 0)
				{
					raycasts.RemoveAt(0);
					return;
				}
				Current = cur.First();
				cur.CutStart(1);
			}
			else
				raycasts.RemoveAt(0);
		}
	}
}
