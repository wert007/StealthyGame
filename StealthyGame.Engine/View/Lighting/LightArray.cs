using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	class LightArray
	{
		Color ambient;
		int size;
		HSV[] colors;
		int[] counts;

		public LightArray(int size, Color ambient)
		{
			this.size = size;
			this.ambient = ambient;
			colors = new HSV[size];
			counts = new int[size];
		}

		public void Set(int index, HSV color)
		{
			if (counts[index] == 0)
				colors[index] = color;
			else
			{
				float lerpFactor = counts[index] / (counts[index] + 1f);
				colors[index].Hue = (int)MathHelper.Lerp(colors[index].Hue, color.Hue, lerpFactor);
				colors[index].Saturation = Math.Min(colors[index].Saturation, color.Saturation);
				colors[index].Value = Math.Max(colors[index].Value, color.Value);
			}
			counts[index]++;
		}

		public Color[] ToColorArray()
		{
			Color[] result = new Color[size];
			for (int i = 0; i < size; i++)
			{
				if (counts[i] == 0)
					result[i] = ambient;
				else
					result[i] = colors[i].ToColor();
			}
			return result;
		}
	}

	public struct HSV
	{
		public int Hue;
		public float Saturation;
		public float Value;

		public HSV(Color color)
		{
			byte max = Math.Max(color.R, Math.Max(color.G, color.B));
			byte min = Math.Min(color.R, Math.Min(color.G, color.B));

			if (max == min) Hue = 0;
			else if (max == color.R) Hue = ((60 * ((color.G - color.B) / (max - min))) + 360) % 360;
			else if (max == color.G) Hue = ((60 * (2 + ((color.B - color.R) / (max - min)))) + 360) % 360;
			else if (max == color.B) Hue = ((60 * (4 + ((color.R - color.G) / (max - min)))) + 360) % 360;
			else throw new ArgumentException("No Value for Hue could be computed.");

			if (max == 0) Saturation = 0;
			else Saturation = (float)(max - min) / max;

			Value = max / 255f;
		}

		public Color ToColor()
		{
			float chroma = Value * Saturation;
			float section = Hue / 60.0f;
			float x = chroma * (1 - Math.Abs(section % 2 - 1));
			byte m = (byte)(255 * (Value - chroma));
			byte r = 0;
			byte g = 0;
			byte b = 0;

			if (section >= 0 && section <= 1)
			{
				r = (byte)(chroma * 255);
				g = (byte)(x * 255);
				b = 0;
			}
			else if (section >= 1 && section <= 2)
			{
				r = (byte)(x * 255);
				g = (byte)(chroma * 255);
				b = 0;
			}
			else if (section >= 2 && section <= 3)
			{
				r = 0;
				g = (byte)(chroma * 255);
				b = (byte)(x * 255);
			}
			else if (section >= 3 && section <= 4)
			{
				r = 0;
				g = (byte)(x * 255);
				b = (byte)(chroma * 255);
			}
			else if (section >= 4 && section <= 5)
			{
				r = (byte)(x * 255);
				g = 0;
				b = (byte)(chroma * 255);
			}
			else if (section >= 5 && section <= 6)
			{
				r = (byte)(chroma * 255);
				g = 0;
				b = (byte)(x * 255);
			}
			return new Color(r + m, g + m, b + m, 255);
		}
	}
}
