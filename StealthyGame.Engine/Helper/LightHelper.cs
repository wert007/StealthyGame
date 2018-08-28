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
		
		
		
		
	}
}
