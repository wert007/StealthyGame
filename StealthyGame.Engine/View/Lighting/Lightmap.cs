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
		List<AnimatedTileLighting> animatedTileLightings;

		public Lightmap(Map map)
		{
			this.map = map;
			animatedTileLightings = new List<AnimatedTileLighting>();
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
			for (int x = 0; x < map.TileSize.X; x++)
			{
				for (int y = 0; y < map.TileSize.Y; y++)
				{
					foreach (var layer in map.Layers)
					{
						if (layer.IsInteractive(x, y))
							animatedTileLightings.Add(new AnimatedTileLighting(layer.GetInteractiveAt(new DataTypes.Index2(x, y))));
					}
				}
			}
		}

		public void Update(GameTime time)
		{
			foreach (var tile in animatedTileLightings)
			{
				tile.Update(time);
			}
		}

		public void Draw(SpriteBatch batch, Matrix cam, Rectangle areaOfInfluence)
		{
			renderTarget?.Dispose();
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
					if(tileSetsMap.IsAnimation(data[i]))
					{
						var tile = animatedTileLightings.FirstOrDefault(t => t.Index == new DataTypes.Index2((i % map.TileSize.X), (i / map.TileSize.X)));
						batch.Draw(tile.Current(), new Rectangle(x - areaOfInfluence.X, y - areaOfInfluence.Y, 16, 16), Color.White);
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
			using (FileStream fs = new FileStream("obstacles.png", FileMode.Create))
				renderTarget.SaveAsPng(fs, renderTarget.Width, renderTarget.Height);
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