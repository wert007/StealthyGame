using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class LightHelper
	{
		public static float Angle => (float)(Math.PI / 180.0f);
		public static BlendState Multiply => new BlendState()
		{
			ColorSourceBlend = Blend.DestinationColor,
			ColorDestinationBlend = Blend.Zero,
			ColorBlendFunction = BlendFunction.Add
		};

		public static void ShrinkRaycast(Raycast raycast, byte[,] obstacles)
		{
			for (int i = 0; i < raycast.Count; i++)
			{
				if (obstacles[raycast[i].X, raycast[i].Y] == 255)
					raycast.CutEnd(i);
			}
		}


		public static float GetObstacle(Index2 pos, byte[,] obstacles)
		{
			if (pos.X < 0 || pos.Y < 0 || pos.X >= obstacles.GetLength(0) || pos.Y >= obstacles.GetLength(1))
				return float.PositiveInfinity;
			return obstacles[pos.X, pos.Y] / 255.0f;
		}

		public static void SimplisticComputeRaycast(LightRaycast raycast, ILightStorage target, byte[,] obstacles)
		{
			float brightness = raycast.Light.Brightness;
			for (int i = 0; i < raycast.Count; i++)
			{
				float obstacle = GetObstacle(raycast[i], obstacles);
				brightness -= obstacle;
				if (raycast.Count - i < raycast.Light.Radius * 0.3f)
					brightness /= 1.05f;
				if (brightness <= 0) return;

				if (raycast[i].X - target.Position.X < 0 || raycast[i].X - target.Position.X >= target.Width ||
							raycast[i].Y - target.Position.Y < 0 || raycast[i].Y  - target.Position.Y >= target.Height)
					continue;
				target.SetAt(raycast[i].X - target.Position.X, raycast[i].Y  - target.Position.Y, raycast.Light.Color * brightness);
			}
		}

		public static void ComputeRaycast(LightRaycast raycast, bool[,] animations, ILightStorage target, byte[,] obstacle, bool checkIsAnimated = true)
			=>	ComputeRaycast(raycast, raycast.Light, animations, target, obstacle, checkIsAnimated);
		public static void ComputeRaycast(Raycast raycast, Light l, bool[,] animations, /*Map map,*/ ILightStorage lmap, byte[,] obstacles, bool checkIsAnimated = true)
		{
			float b = 1.0f;
			for (int i = 0; i < raycast.Count; i++)
			{
				float obstacle = GetObstacle(raycast[i], obstacles);
				b -= obstacle;
				if (raycast.Count - i < 50)
					b /= 1.05f;
				if (b <= 0)
					break;
				if (checkIsAnimated)
				{
					//bool? animated = map?.IsAnimated(raycast[i]);
					if (animations[raycast[i].X, raycast[i].Y])
						break;
				}
				int s = 3;
				if (i + s + 2 >= raycast.Count || GetObstacle(raycast[i + s + 2], obstacles) >= 1)
					break;
				for (int x = -s; x <= s; x++)
				{
					for (int y = -s; y <= s; y++)
					{
						int len = x * x + y * y;
						float factor = 1f;

						float o = 0.5f;
						float gaus = (float)((1.0f / (2.0f * Math.PI * o * o)) * Math.Pow(Math.E, -(x * x + y * y) / 2.0f * o * o));
						factor = gaus / (float)(1.0f / (2.0f * Math.PI * o * o));
						if (raycast[i].X + x - lmap.Position.X < 0 || raycast[i].X + x - lmap.Position.X >= lmap.Width ||
							raycast[i].Y + y - lmap.Position.Y < 0 || raycast[i].Y + y - lmap.Position.Y >= lmap.Height)
							continue;
						lmap.SetAt(raycast[i].X + x - lmap.Position.X, raycast[i].Y + y - lmap.Position.Y, l.Color * factor * b);
					}
				}
			}
		}
	}
}
