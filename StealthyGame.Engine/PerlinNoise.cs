using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine
{
	public static class PerlinNoise
	{
		static int xSize = 512;
		static int ySize = 512;
		static int zSize = 512;

		static float[,,] noise = new float[512, 512, 512];
		static Random rand = new Random();
		
		public static void GenerateNoise()
		{
			for (int y = 0; y < ySize; y++)
				for (int x = 0; x < xSize; x++)
					for(int z = 0; z < zSize; z++)
						noise[z, y, x] = (float)rand.NextDouble();
		}
		
		static float SmoothNoise2D(float x, float y)
		{
			//get fractional part of x and y
			float fractX = x - (int)(x);
			float fractY = y - (int)(y);

			//wrap around
			int x1 = ((int)(x) + xSize) % xSize;
			int y1 = ((int)(y) + ySize) % ySize;

			//neighbor values
			int x2 = (x1 + xSize - 1) % xSize;
			int y2 = (y1 + ySize - 1) % ySize;

			//smooth the noise with bilinear interpolation
			float value = 0.0f;
			value += fractX * fractY * noise[0, y1,x1];
			value += (1 - fractX) * fractY * noise[0, y1,x2];
			value += fractX * (1 - fractY) * noise[0, y2,x1];
			value += (1 - fractX) * (1 - fractY) * noise[0, y2,x2];

			return value;
		}

		static float SmoothNoise3D(float x, float y, float z)
		{
			//get fractional part of x and y
			float fractX = x - (int)(x);
			float fractY = y - (int)(y);
			float fractZ = z - (int)(z);

			//wrap around
			int x1 = ((int)(x) + xSize) % xSize;
			int y1 = ((int)(y) + ySize) % ySize;
			int z1 = ((int)(z) + zSize) % zSize;

			//neighbor values
			int x2 = (x1 + xSize - 1) % xSize;
			int y2 = (y1 + ySize - 1) % ySize;
			int z2 = (z1 + zSize - 1) % zSize;

			//smooth the noise with bilinear interpolation
			float value = 0.0f;
			value += fractX * fractY * fractZ * noise[z1, y1, x1];
			value += fractX * (1 - fractY) * fractZ * noise[z1, y2, x1];
			value += (1 - fractX) * fractY * fractZ * noise[z1, y1, x2];
			value += (1 - fractX) * (1 - fractY) * fractZ * noise[z1, y2, x2];

			value += fractX * fractY * (1 - fractZ) * noise[z2, y1, x1];
			value += fractX * (1 - fractY) * (1 - fractZ) * noise[z2, y2, x1];
			value += (1 - fractX) * fractY * (1 - fractZ) * noise[z2, y1, x2];
			value += (1 - fractX) * (1 - fractY) * (1 - fractZ) * noise[z2, y2, x2];

			return value;
		}

		public static float Turbulence2D(float x, float y, float size)
		{
			x *= 1024;
			y *= 1024;
			
			float value = 0.0f;
			float initialSize = size;

			while (size >= 1)
			{
				value += SmoothNoise2D(x / size, y / size) * size;
				size /= 2.0f;
			}

			return (value / initialSize);
		}

		public static float Turbulence3D(float x, float y, float z, float size)
		{
			x *= 1024;
			y *= 1024;
			z *= 1024;

			float value = 0.0f;
			float initialSize = size;

			while (size >= 1)
			{
				value += SmoothNoise3D(x / size, y / size, z / size) * size;
				size /= 2.0f;
			}

			return (value / initialSize);
		}
	}
}