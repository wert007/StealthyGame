using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class RandomHelper
	{
		public static byte NextByte(this Random random) => random.NextByte(0, 255);
		public static byte NextByte(this Random random, byte max) => random.NextByte(0, max);
		public static byte NextByte(this Random random, byte min, byte max) => (byte)(random.Next(min, max + 1));

		public static Color NextGrayColor(this Random random, bool keepAlpha = true)
		{
			if (keepAlpha)
				return random.NextGrayColor(0, 255);
			else
				return random.NextGrayColor(0, 255, 0, 255);
		}
		public static Color NextGrayColor(this Random random, byte minV, byte maxV) => random.NextGrayColor(minV, maxV, 255, 255);
		public static Color NextGrayColor(this Random random, byte minV, byte maxV, byte minAlpha, byte maxAlpha)
		{
			byte val = random.NextByte(minV, maxV);
			Color ret = new Color(val, val, val, random.NextByte(minAlpha, maxAlpha));
			return ret;
		}

		public static Color NextColor(this Random random, float minV, float maxV, bool keepAlpha = true)
			=> random.NextColor((byte)Math.Round(minV * 255), (byte)Math.Round(maxV * 255), keepAlpha);
		public static Color NextColor(this Random random, float minR, float maxR, float minG, float maxG, float minB, float maxB)
			=> random.NextColor((byte)Math.Round(minR * 255), (byte)Math.Round(maxR * 255), (byte)Math.Round(minG * 255), (byte)Math.Round(maxG * 255),
					(byte)Math.Round(minB * 255), (byte)Math.Round(maxB * 255));
		public static Color NextColor(this Random random, float minR, float maxR, float minG, float maxG, float minB, float maxB, float minA, float maxA)
			=> random.NextColor((byte)Math.Round(minR * 255), (byte)Math.Round(maxR * 255), (byte)Math.Round(minG * 255), (byte)Math.Round(maxG * 255),
					(byte)Math.Round(minB * 255), (byte)Math.Round(maxB * 255), (byte)Math.Round(minA * 255), (byte)Math.Round(maxA * 255));

		public static Color NextColor(this Random random, int minV, int maxV, bool keepAlpha = true)
			=> random.NextColor((byte)minV, (byte)maxV, keepAlpha);
		public static Color NextColor(this Random random, int minR, int maxR, int minG, int maxG, int minB, int maxB)
			=> random.NextColor((byte)minR, (byte)maxR, (byte)minG, (byte)maxG, (byte)minB, (byte)maxB);
		public static Color NextColor(this Random random, int minR, int maxR, int minG, int maxG, int minB, int maxB, int minA, int maxA)
			=> random.NextColor((byte)minR, (byte)maxR, (byte)minG, (byte)maxG, (byte)minB, (byte)maxB, (byte)minA, (byte)maxA);

		public static Color NextColor(this Random random, bool keepAlpha = true) => random.NextColor(0, 255, keepAlpha);
		public static Color NextColor(this Random random, byte minV, byte maxV, bool keepAlpha = true)
		{
			if (keepAlpha)
				return random.NextColor(minV, maxV, minV, maxV, minV, maxV);
			else
				return random.NextColor(minV, maxV, minV, maxV, minV, maxV, minV, maxV);
		}
		public static Color NextColor(this Random random, byte minR, byte maxR, byte minG, byte maxG, byte minB, byte maxB)
			=> random.NextColor(minR, maxR, minG, maxG, minB, maxB, (byte)255, (byte)255);
		public static Color NextColor(this Random random, byte minR, byte maxR, byte minG, byte maxG, byte minB, byte maxB, byte minA, byte maxA)
		{
			Color ret = new Color(random.NextByte(minR, maxR), random.NextByte(minG, maxG), random.NextByte(minB, maxB), random.NextByte(minA, maxA));
			return ret;
		}

		public static Index2 NextIndex2(this Random random) 
			=> new Index2(random.Next(), random.Next());
		public static Index2 NextIndex2(this Random random, int max)
			=> NextIndex2(random, 0, max);
		public static Index2 NextIndex2(this Random random, int min, int max)
			=> new Index2(random.Next(min, max), random.Next(min, max));
		public static Index2 NextIndex2(this Random random, Index2 max)
			=> NextIndex2(random, new Index2(), max);
		public static Index2 NextIndex2(this Random random, Index2 min, Index2 max)
			=> new Index2(random.Next(min.X, max.X), random.Next(min.Y, max.Y));

	}
}