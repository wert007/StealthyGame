using StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.View.Lighting;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Geometrics;

namespace StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps
{
	public class StaticLightMap : BasicLightmap
	{
		Texture2D texture;
		LightArea lightArea;
		List<Texture2D> obstacleTextures;

		public StaticLightMap(int width, int height, float drawOrder) : base(width, height, drawOrder)
		{
			lightArea = new LightArea(Position.X, Position.Y, Width, Height);
			obstacleTextures = new List<Texture2D>();
		}

		public void AddObstacles(Texture2D layer) => obstacleTextures.Add(layer);

		public override void Draw(SpriteBatch batch)
		{
			batch.Draw(texture, new Rectangle(Position.X, Position.Y, Width, Height), Color.White);
		}

		public override HSVColor GetAt(int x, int y)
		{
			return lightArea.GetAt(x, y);
		}

		public override void SetAt(int x, int y, HSVColor color)
		{
			lightArea.SetAt(x, y, color);
		}

		private float[,] GatherObstaclesF()
		{
			Color[] data = new Color[Width * Height];
			float[,] obstacles = new float[Width, Height];
			for (int i = 0; i < obstacleTextures.Count; i++)
			{
				obstacleTextures[i].GetData(data);
				for (int x = 0; x < Width; x++)
					for (int y = 0; y < Height; y++)
					{
						if (data[y * Width + x].A < 255)
							continue;
						obstacles[x, y] = (255 - data[y * Width + x].B) / 255f;
					}
			}
			return obstacles;
		}
		
		private void CreateTexture(HSVColor ambient, GraphicsDevice graphicsDevice)
		{
			texture = new Texture2D(graphicsDevice, Width, Height);
			Color[] data = new Color[Width * Height];
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var l = GetAt(x, y);
					if (ambient.Value > l.Value)
						data[y * Width + x] = ambient.RGB;
					else
						data[y * Width + x] = l.RGB;
				}
			}
			texture.SetData(data);
		}

		protected override void _Generate(HSVColor ambient, GraphicsDevice graphicsDevice)
		{
			float[,] obstacles = GatherObstaclesF();
			var res = FieldOfView.ComputeLighting(Lights.ToArray(), obstacles,  new bool[obstacles.GetLength(0), obstacles.GetLength(1)], FOVComputingAlgorithm.Spiral);
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if(res[x,y] != Color.TransparentBlack)
						SetAt(x, y, new HSVColor(res[x,y]));
				}
			}
			CreateTexture(ambient, graphicsDevice);
		}
	}
}
