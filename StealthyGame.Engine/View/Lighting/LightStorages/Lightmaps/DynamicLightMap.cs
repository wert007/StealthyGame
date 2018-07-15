using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.Helper;

namespace StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps
{
	public class DynamicLightMap : BasicLightmap
	{
		List<Obstacle> obstacles;
		LightArea area;
		HSVColor ambient;

		public DynamicLightMap(int width, int height, float drawOrder) : base(width, height, drawOrder)
		{
			obstacles = new List<Obstacle>();
			area = new LightArea(Position.X, Position.Y, width, height);
		}

		public void AddObstacle(Obstacle obstacle) 
			=> obstacles.Add(obstacle);

		public override void Draw(SpriteBatch batch)
		{
			Generate(null, ambient, null);
			Color current;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var l = GetAt(x, y);
					if (ambient.Value > l.Value)
						current = ambient.RGB;
					else
						current = l.RGB;

					batch.DrawPixel(x, y, current);
				}
			}
		}

		public override HSVColor GetAt(int x, int y) 
			=> area.GetAt(x, y);

		public override void SetAt(int x, int y, HSVColor color) 
			=> area.SetAt(x, y, color);

		public float[,] GatherObstaclesF()
		{
			float[,] result = new float[Width, Height];
			foreach (var obstacle in obstacles.Where(o => o.Changed))
			{
				for (int x = obstacle.Position.X; x < obstacle.Position.X + obstacle.Width; x++) 
				{
					for (int y = obstacle.Position.Y; y < obstacle.Position.Y + obstacle.Height; y++)
					{
						result[x, y] = obstacle.Shape[x - obstacle.Position.X, y - obstacle.Position.Y];
					}
				}
			}
			return result;
		}

		protected override void _Generate(HSVColor ambient, GraphicsDevice graphicsDevice)
		{
			this.ambient = ambient;
			area.Reset();
			float[,] obstacles = GatherObstaclesF();
			var res = FieldOfView.ComputeLighting(Lights.ToArray(), obstacles, new bool[obstacles.GetLength(0), obstacles.GetLength(1)], FOVComputingAlgorithm.Spiral);
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (res[x, y] != Color.TransparentBlack)
						SetAt(x, y, new HSVColor(res[x, y]));
					else
						SetAt(x, y, ambient);
				}
			}
		}
	}
}
