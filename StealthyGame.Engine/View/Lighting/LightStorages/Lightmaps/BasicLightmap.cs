using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.MapBasics.Tiles;

namespace StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps
{
	public abstract class BasicLightmap : ILightStorage
	{
		public Index2 Position => new Index2();

		public int Width { get; }
		public int Height { get; }

		public float DrawOrder { get; }
		public HSVColor AmbientColor { get; private set; }

		protected List<Light> Lights { get; set; }

		public BasicLightmap(int width, int height, float drawOrder)
		{
			Width = width;
			Height = height;
			DrawOrder = drawOrder;
			Lights = new List<Light>();
		}

		public void AddLight(Light light) => Lights.Add(light);


		public abstract void Draw(SpriteBatch batch);
		//public void Generate(HSVColor ambient, GraphicsDevice graphicsDevice)
		//{
		//	AmbientColor = ambient;
		//	_Generate(new AnimatedTileLighting[0], ambient, graphicsDevice);
		//}
		public void Generate(Map map, HSVColor ambient, GraphicsDevice graphicsDevice)
		{
			AmbientColor = ambient;
			//List<AnimatedTileLighting> animated = new List<AnimatedTileLighting>();
			
			//for (int x = 0; x < Width/BasicTile.Size; x++)
			//{
			//	for (int y = 0; y < Height/BasicTile.Size; y++)
			//	{
			//		var tile = map.GetTile(x, y);
			//		if(tile.Animations != null)
			//		{
			//			animated.Add(new AnimatedTileLighting(tile));
			//		}
			//	}
			//}
			_Generate(ambient, graphicsDevice);
		}
		protected abstract void _Generate(HSVColor ambient, GraphicsDevice graphicsDevice);
		public abstract HSVColor GetAt(int x, int y);
		public abstract void SetAt(int x, int y, HSVColor color);
	}
}
