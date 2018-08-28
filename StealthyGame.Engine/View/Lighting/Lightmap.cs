using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
	class Lightmap
	{
		TileSetManager tileSetsShadow;
		TileSetManager tileSetsMap;
		Map map;
		public RenderTarget2D renderTarget;
		RenderTargetBinding[] oldRenderTargets;

		public Lightmap(Map map)
		{
			this.map = map;
		}

		public void LoadTileSets(TileSetManager tileSetManager, GraphicsDevice graphicsDevice)
		{
			tileSetsMap = tileSetManager;
			tileSetsShadow = new TileSetManager();
			foreach (var tileSet in tileSetManager.TileSets)
			{
				string path = Path.Combine(Path.GetDirectoryName(tileSet.SourcePath),
					Path.GetFileNameWithoutExtension(tileSet.SourcePath) + "_shadow" + Path.GetExtension(tileSet.SourcePath));
				if (!File.Exists(path))
					continue;
				TileSet shadowTileSet = TileSet.Load(path, graphicsDevice);
				tileSetsShadow.AddTileSet(shadowTileSet);
			}
		}

		public void Draw(SpriteBatch batch, Matrix cam, Rectangle areaOfInfluence)
		{
			renderTarget = new RenderTarget2D(batch.GraphicsDevice, areaOfInfluence.Width, areaOfInfluence.Height);
			oldRenderTargets = batch.GraphicsDevice.GetRenderTargets();
			batch.GraphicsDevice.SetRenderTarget(null);
			batch.GraphicsDevice.SetRenderTarget(renderTarget);
			batch.GraphicsDevice.Clear(Color.TransparentBlack);
			
			batch.End();
			batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
			int x;
			int y;
			int[] data;
			TileSet tileSet;
			int actualData;
			Rectangle sourceRectangle;
			foreach (var layer in map.Layers)
			{
				data = layer.GetRawData();

				for (int i = 0; i < data.Length; i++)
				{
					if (data[i] == 0)
						continue;
					x = 16 * (i % map.TileSize.X);
					y = 16 * (i / map.TileSize.X);
					if (x + 16 < areaOfInfluence.X || y + 16 < areaOfInfluence.Y ||
						x - 16 > areaOfInfluence.Right || y - 16 > areaOfInfluence.Bottom)
						continue;
					
					tileSet = tileSetsShadow.TileSets.FirstOrDefault(t => t.Name == tileSetsMap.IndexToTileSet(data[i]).Name + "_shadow");

					if (tileSet == null) continue;


					actualData = tileSetsMap.ShortenIndex(data[i]) + 1;
					if(tileSet.IsAnimation(actualData))
					{
						AnimatedTileLighting atl = new AnimatedTileLighting();
						
						continue;
					}
					sourceRectangle = tileSetsShadow.GetSourceRectangle(actualData);

					batch.Draw(tileSet.Texture, new Rectangle(x - areaOfInfluence.X, y - areaOfInfluence.Y, 16, 16), sourceRectangle, Color.White); //16  //
				}
			}
			batch.End();
			batch.GraphicsDevice.SetRenderTarget(null);
			batch.GraphicsDevice.SetRenderTargets(oldRenderTargets);
			batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam);
		}

		public Texture2D ToTexture2D(GraphicsDevice graphicsDevice)
		{
			Texture2D result = new Texture2D(graphicsDevice, renderTarget.Width, renderTarget.Height);
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			result.SetData(data);
			return result;
		}

		public bool[,] GetObstacles()
		{
			bool[,] result = new bool[renderTarget.Width, renderTarget.Height];
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			int i = 0;
			for (int x = 0; x < renderTarget.Width; x++)
				for (int y = 0; y < renderTarget.Height; y++)
				{
					i = y * renderTarget.Width + x;
					result[x, y] = data[i].A > 0 && data[i].R < 255 && data[i].G < 255 && data[i].B < 255;
				}
			return result;
		}
	}
}