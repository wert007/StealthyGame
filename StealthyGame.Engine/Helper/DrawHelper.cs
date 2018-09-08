using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	static class DrawHelper
	{

		static Color GetPixel(this Color[] data, int x, int y, int width) => data[y * width + x];

		static Texture2D GaussianBlur(this Texture2D origin, int sigma)
		{
			int maskSize = 3 * sigma ;
			Texture2D result = new Texture2D(origin.GraphicsDevice, origin.Width, origin.Height);
			Color[] data = new Color[origin.Width * origin.Height];
			origin.GetData(data);
			Color[] resultData = new Color[origin.Width * origin.Height];
			float[,] mask = Mask(sigma);
			float maskSum = MaskSum(mask) * 255; // 1
			for (int x = maskSize; x < origin.Width - maskSize; x++)
			{
				for (int y = maskSize; y < origin.Height - maskSize; y++)
				{
					Color cur = Color.TransparentBlack;
					float r = 0;
					float g = 0;
					float b = 0;
					float a = 0;
					for (int xOff = -maskSize; xOff <= maskSize; xOff++)
					{
						for (int yOff = -maskSize; yOff <= maskSize; yOff++)
						{
							cur = data.GetPixel(x + xOff, y + yOff, origin.Width);
							r += cur.R * mask[xOff + maskSize, yOff + maskSize];
							g += cur.G * mask[xOff + maskSize, yOff + maskSize];
							b += cur.B * mask[xOff + maskSize, yOff + maskSize];
							a += cur.A * mask[xOff + maskSize, yOff + maskSize];
						}
					}
					cur = new Color(r / maskSum, g / maskSum, b / maskSum, a / maskSum);
					resultData[y * origin.Width + x] = cur;
				}
			}
			result.SetData(resultData);
			return result;
		}

		static float MaskSum(float[,] mask)
		{
			float sum = 0;
			for (int x = 0; x < mask.GetLength(0); x++)
			{
				for (int y = 0; y < mask.GetLength(1); y++)
				{
					sum += mask[x, y];
				}
			}
			return sum;
		}

		static float[,] Mask(int sigma)
		{
			float[,] tmpResult = new float[6 * sigma+ 1, 6 * sigma + 1];
			for (int x = -3 * sigma; x <= 3 * sigma; x++)
			{
				for (int y = -3 * sigma; y <= 3 * sigma; y++)
				{
					tmpResult[x + 3 * sigma, y + 3 * sigma] = Foo(x, y, sigma);
				}
			}
			return tmpResult;
		}

		static float Foo(int x, int y, float sigma)
		{
			return (1f / (MathHelper.TwoPi * sigma * sigma)) * (float)Math.Pow(Math.E, -((x * x + y * y) / 2 * sigma * sigma));
		}
		

		//public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2? position = null, Rectangle? destinationRectangle = null,
		//	Color? color = null, float rotation = 0, Vector2? origin = null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
		//{
		//	if (animationCollection.Length <= 0)
		//		return;
		//	batch.Draw(animationCollection.GetCurrent(), position, destinationRectangle, color, rotation, origin, scale, effects, layerDepth);
		//}


	}
}
