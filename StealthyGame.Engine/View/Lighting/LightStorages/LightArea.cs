using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public class LightArea : ILightStorage
	{
		public Index2 Position { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		int[,] hues;
		float[,] saturations;
		float[,] values;
		int[,] sources;


		public LightArea(int x, int y, int width, int height)
		{
			Position = new Index2(x, y);
			Width = width;
			Height = height;
			hues = new int[width, height];
			sources = new int[width, height];
			saturations = new float[width, height];
			values = new float[width, height];
		}

		public void Reset()
		{
			hues = new int[Width, Height];
			sources = new int[Width, Height];
			saturations = new float[Width, Height];
			values = new float[Width, Height];
		}

		public HSVColor GetAt(int x, int y)
		{
			return new HSVColor(hues[x, y], saturations[x, y], values[x, y]);
		}

		//public Task LoadArea(Texture2D texture) => new Task(() => _LoadArea(texture));

		//void _LoadArea(Texture2D texture)
		//{
		//	Console.WriteLine("generate");
		//	Color[] data = new Color[Width * Height];
		//	for (int x = 0; x < Width; x++)
		//	{
		//		for (int y = 0; y < Height; y++)
		//		{
		//			data[y * Width + x] = GetAt(x, y).RGB;
		//		}
		//	}
		//	texture.SetData(data);
		//	Console.WriteLine("generated");
		//}

		public void SetAt(int x, int y, HSVColor color)
		{
			if (color.RGB.A == 0) return;
			sources[x, y]++;
			if (hues[x, y] == 0) hues[x, y] = color.Hue;
			//hues[x, y] = (hues[x, y] + color.Hue) / sources[x,y];
			saturations[x, y] = Math.Max(saturations[x, y], color.Saturation);
			values[x, y] = Math.Max(values[x, y], color.Value);
		}
	}
}
