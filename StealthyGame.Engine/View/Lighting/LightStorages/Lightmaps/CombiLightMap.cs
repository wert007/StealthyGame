using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps
{
	class CombiLightMap : BasicLightmap
	{
		StaticLightMap staticLightMap;
		DynamicLightMap dynamicLightMap;

		public CombiLightMap(int width, int height, float drawOrder) : base(width, height, drawOrder)
		{
			staticLightMap = new StaticLightMap(width, height, drawOrder);
			dynamicLightMap = new DynamicLightMap(width, height, drawOrder);
		}

		public override void Draw(SpriteBatch batch)
		{
			staticLightMap.Draw(batch);
		//	dynamicLightMap.Draw(batch);
		}

		public override HSVColor GetAt(int x, int y)
		{
			throw new NotImplementedException();
		}

		public override void SetAt(int x, int y, HSVColor color)
		{
			throw new NotImplementedException();
		}

		protected override void _Generate(HSVColor ambient, GraphicsDevice graphicsDevice)
		{
			staticLightMap.Generate(null, ambient, graphicsDevice);
		}

		public void AddStaticObstacles(Texture2D obstacles)
		{
			staticLightMap.AddObstacles(obstacles);
		}

		public void AddDynamicObstacle(Obstacle obstacle)
		{
			dynamicLightMap.AddObstacle(obstacle);
		}
	}
}
