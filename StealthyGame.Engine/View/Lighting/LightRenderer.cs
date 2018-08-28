using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.GameObjects.Collisionables;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	//Grab Screenize
	//Grab Shadowmap
	//Scale Down
	//Is Equal to Old Shadows?
	//Else Calculate Lightmap
	//Draw Lightmap
	public class LightRenderer
	{
		Texture2D texture;
		Camera camera;
		bool[,] obstacles;
		List<Light> lights;
		List<MapLayer> layers;
		Rectangle renderSize;
		Rectangle influenceArea;
		Lightmap lightmap;
		GraphicsDevice graphicsDevice;

		public LightRenderer(GraphicsDevice graphicsDevice, Map map)
		{
			camera = Camera.Instance;
			lights = new List<Light>();
			layers = new List<MapLayer>();
			lightmap = new Lightmap(map);
			this.graphicsDevice = graphicsDevice;
		}

		public void LoadTileSets(TileSetManager tileSetManager)
		{
			lightmap.LoadTileSets(tileSetManager, graphicsDevice);
		}

		private bool[,] GrabObstacles(SpriteBatch batch, Matrix cam)
		{
			lightmap.Draw(batch, cam, new Rectangle(influenceArea.X / 2, influenceArea.Y / 2, influenceArea.Width / 2, influenceArea.Height / 2));
			return lightmap.GetObstacles();
		}

		public void CalculateLightmap(SpriteBatch batch, Matrix cam, Index2 mapSize, Color ambient)
		{
			var tempRender = CalculateRenderView(mapSize);
			if (!tempRender.HasValue)
				return;
			renderSize = tempRender.Value;
			influenceArea = CalculateInfluenceArea();
			obstacles = GrabObstacles(batch, cam);
			texture = new Texture2D(graphicsDevice, renderSize.Width, renderSize.Height);
			var res = new FieldOfView().DoSomething(lights.ToArray(), ambient, obstacles, renderSize, influenceArea, 2);
			texture.SetData(res);
		}

		private Rectangle? CalculateRenderView(Index2 mapSize)
		{
			renderSize = camera.View;
			if (camera.View.X < 0 || camera.View.Y < 0 ||
				renderSize.X + renderSize.Width > mapSize.X ||
				renderSize.Y + renderSize.Height > mapSize.Y)
			{
				if (renderSize.X + renderSize.Width < 0 || renderSize.Y + renderSize.Height < 0)
					return null;
				else
				{
					renderSize.Width += renderSize.X;
					renderSize.Height += renderSize.Y;
					renderSize.X = 0;
					renderSize.Y = 0;
				}
				if (renderSize.X + renderSize.Width > mapSize.X)
				{
					renderSize.Width = mapSize.X - renderSize.X;
				}
				if (renderSize.Y + renderSize.Height > mapSize.Y)
				{
					renderSize.Height = mapSize.Y - renderSize.Y;
				}
				if (renderSize.Width <= 0 || renderSize.Height <= 0)
					return null;
			}
			return renderSize;
		}

		private Rectangle CalculateInfluenceArea()
		{
			CollisionBox renderBox = new CollisionBox(renderSize);
			var nearByLights = lights;// lights.Where(l => renderBox.Collide(new CollisionCircle(l.Area)));
			int minX = (int)Math.Min(nearByLights.Min(l => l.Position.X) * 0.95f, renderSize.X);
			int minY = (int)Math.Min(nearByLights.Min(l => l.Position.Y) * 0.95f, renderSize.Y);
			int maxX = (int)Math.Max(nearByLights.Max(l => l.Position.X) * 1.05f, renderSize.Right);
			int maxY = (int)Math.Max(nearByLights.Max(l => l.Position.Y) * 1.05f, renderSize.Bottom);

			return new Rectangle(minX, minY, maxX - minX, maxY - minY);
		}

		public void Draw(SpriteBatch batch)
		{
			if(texture != null)
				batch.Draw(texture, renderSize, Color.White);
		}

		public void AddLayer(MapLayer layer)
		{
			layers.Add(layer);
		}

		public void AddLight(Light light)
		{
			lights.Add(light);
		}
	}
}
