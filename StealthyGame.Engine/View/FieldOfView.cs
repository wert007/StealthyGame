using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
//using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View
{
	public static class FieldOfView
	{

		//TODO: Make it unstatic
		public static Color[,] ComputeLighting(Light[] lights, float[,] obstacles, bool[,] animated, FOVComputingAlgorithm algorithm)
		{
			Color[,] result = new Color[obstacles.GetLength(0), obstacles.GetLength(1)];
			foreach (var light in lights)
			{
			//	DebugSpriteBatch.AddLight(light);
				Color[,] lighting = new Color[0, 0];
				Rectangle area = new Rectangle(light.Position.X - light.Radius, light.Position.Y - light.Radius, 2 * light.Radius, 2 * light.Radius);
				switch (algorithm)
				{
					case FOVComputingAlgorithm.Raycast:
						lighting = ComputeRaycast(light, GetSubArea(obstacles, area), GetSubArea(animated, area));
						break;
					case FOVComputingAlgorithm.Spiral:
						lighting = ComputeSpiral(light, GetSubArea(obstacles, area), GetSubArea(animated, area));
						break;
					default:
						throw new NotImplementedException();
				}

				for (int x = 0; x < light.Radius * 2; x++)
					for (int y = 0; y < light.Radius * 2; y++)
					{
						if (lighting[x, y] != Color.TransparentBlack)
						{
							int xGlobal = x + light.Position.X - light.Radius;
							int yGlobal = y + light.Position.Y - light.Radius;
							if (xGlobal < 0 || xGlobal >= result.GetLength(0) || yGlobal < 0 || yGlobal >= result.GetLength(1))
								continue;
							result[xGlobal, yGlobal] = lighting[x, y];
						}
					}
			}

			return result;
		}

		private static Color[,] ComputeRaycast(Light light, float[,] obstacles, bool[,] animated)
		{
			Color[,] result = new Color[obstacles.GetLength(0), obstacles.GetLength(1)];
			//Queue<Index2> toCheck = new Queue<Index2>();
			foreach (var endPoint in new Circle(light.Position, light.Radius).All)
			{
				foreach (var point in new LightRaycast(light, light.Position, endPoint))
				{
					if (obstacles[point.X - light.Position.X + light.Radius, point.Y - light.Position.Y + light.Radius] > 0) break;
					result[point.X - light.Position.X + light.Radius, point.Y - light.Position.Y + light.Radius] = light.Color.RGB;
				}
			}
			return result;
		}

		private static Color[,] ComputeSpiral(Light light, float[,] obstacles, bool[,] animated)
		{
			//Console.WriteLine(DebugSpriteBatch.ToString(obstacles));
			AngleCollection obstructedArea = new AngleCollection();
			AngleCollection animatedArea = new AngleCollection();
			Index2[] toCheck = new Circle(light.Position, light.Radius).SpiralArea(true);
			Color[,] result = new Color[obstacles.GetLength(0), obstacles.GetLength(1)];
			foreach (var current in toCheck)
			{
				Index2 dif = current - light.Position;
				Angle angle = new Angle((float)Math.Atan2(dif.Y, dif.X));
				if (dif.X + light.Radius >= obstacles.GetLength(0) || dif.Y + light.Radius >= obstacles.GetLength(1))
					continue;
				result[dif.X + light.Radius, dif.Y + light.Radius] = Color.TransparentBlack;
				if (obstacles[dif.X + light.Radius, dif.Y + light.Radius] > 0)
				{
					float increment = MathHelper.TwoPi / (8 * dif.Length());
				obstructedArea.Shorten();
					obstructedArea.Add(new AnglePair(angle - 0.5f * increment, angle + 0.5f * increment));
					continue;
				}
				if (obstructedArea.ContainsAny(angle))
				{
					continue;
				}
				if(animated[dif.X + light.Radius, dif.Y + light.Radius])
				{
					float increment = MathHelper.TwoPi / (8 * dif.Length());
					animatedArea.Add(new AnglePair(angle - 0.5f * increment, angle + 0.5f * increment));
					continue;
				}
				result[dif.X + light.Radius, dif.Y + light.Radius] = light.Color.RGB;
			}
			animatedArea.Shorten();
			return result;
		}

		private static T[,] GetSubArea<T>(T[,] source, Rectangle area)
		{
			if (area.Width > source.GetLength(0) || area.Height > source.GetLength(1) || area.X < 0 || area.Y < 0)
				Console.WriteLine("Area bigger than Array");
			if (area.X < 0)
				area.X = 0;
			if (area.Y < 0)
				area.Y = 0;
			if (area.Width > source.GetLength(0))
				area.Width = source.GetLength(0);
			if (area.Height > source.GetLength(1))
				area.Height = source.GetLength(1);
			T[,] result = new T[area.Width, area.Height];
			for (int x = 0; x < result.GetLength(0); x++)
			{
				for (int y = 0; y < result.GetLength(1); y++)
				{
					result[x, y] = source[x + area.X, y + area.Y];
				}
			}
			return result;
		}
	}

	public enum FOVComputingAlgorithm
	{
		Raycast,
		Spiral
	}
}
