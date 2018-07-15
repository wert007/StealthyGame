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
	class ShadowCastingField : Field
	{
		List<LightRaycast> raycasts;
		public ShadowCastingField(string name) : base(name)
		{
			raycasts = new List<LightRaycast>();
			UseAnti = true;
			Reset();
		}

		protected override void PreCompute()
		{
			List<Index2> obstaclePositions = new List<Index2>();
			for (int x = 0; x < Size; x++)
				for (int y = 0; y < Size; y++)
					if (obstacles[x, y] == 255)
						obstaclePositions.Add(new Index2(x, y));
			Light l = new Light(lightPosition, radius, 0.5f, HSVColor.White);
			foreach (var obs in obstaclePositions)
			{
				Vector2 dif = obs - lightPosition;
				dif *= radius;
				var ray = new LightRaycast(l, lightPosition, (Index2)(lightPosition + dif));
				ray.CutEnd(Math.Min(radius * radius, ray.Count));
				ray.CutStart(ray.IndexOf(obs));
				raycasts.Add(ray);
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
				AntiCurrent = cur.First();
				cur.CutStart(1);
			}
			else
				raycasts.RemoveAt(0);
		}


	}
}
