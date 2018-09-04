using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View
{
	public class FieldOfView
	{
		float scale;
		LightArray lightArray;
		public Color[] DoSomething(Light[] lights, Color ambient, bool[,] obstacles, Rectangle renderArea, Rectangle influenceArea, int downscale)
		{
			scale = 1f / downscale;
			lightArray = new LightArray(renderArea.Width * renderArea.Height, ambient);
			Light[] smallerLights = new Light[lights.Length];
			Rectangle smallerRenderArea = new Rectangle((int)(renderArea.X * scale), (int)(renderArea.Y * scale), (int)(renderArea.Width * scale), (int)(renderArea.Height * scale));
			Rectangle smallerInfluenceArea = new Rectangle((int)(influenceArea.X * scale), (int)(influenceArea.Y * scale), (int)(influenceArea.Width * scale), (int)(influenceArea.Height * scale));
			for (int i = 0; i < smallerLights.Length; i++)
			{
				smallerLights[i] = new Light(lights[i].Position * scale, (short)(lights[i].Radius * scale), lights[i].Brightness, lights[i].Color);
				DebugRenderer.AddDebugObjectSingleCall(new DebugLight(lights[i], Color.AliceBlue));
			}
			Index2[][] pointsToCheck = new Index2[lights.Length][];
			for (int i = 0; i < smallerLights.Length; i++)
			{
				pointsToCheck[i] = Circle.CalculateEdge(smallerLights[i].Position, smallerLights[i].Radius);
				foreach (var currentPoint in pointsToCheck[i])
				{
					if (smallerRenderArea.Contains(currentPoint) || smallerRenderArea.Contains(smallerLights[i].Position))
						foreach (var raycast in new LightRaycast(smallerLights[i], smallerLights[i].Position, currentPoint))
						{
							if (!smallerInfluenceArea.Contains(raycast)) //TODO: Use binarysearch
								continue;
							int x = raycast.X - smallerRenderArea.X;
							int y = raycast.Y - smallerRenderArea.Y;
							if (obstacles[x, y]) break;
							if (!smallerRenderArea.Contains(raycast))
								continue;
							for (int xOff = 0; xOff < downscale; xOff++)
							{
								for (int yOff = 0; yOff < downscale; yOff++)
								{
									lightArray.Set((downscale * y + yOff) * renderArea.Width + (downscale * x + xOff), smallerLights[i].Color);
								}
							}
						}
				}
			}

			return lightArray.ToColorArray();
		}
	}
}
