using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public struct HSVColor
	{
		public Color RGB { get; private set; }
		public int Hue { get; private set; }
		public float Saturation { get; private set; }
		public float Value { get; private set; }

		public HSVColor(int hue, float saturation, float value) 
			: this(hue, saturation, value, 255) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hue">0 to 360</param>
		/// <param name="saturation">0 to 1</param>
		/// <param name="value">0 to 1</param>
		/// <param name="alpha"></param>
		public HSVColor(int hue, float saturation, float value, byte alpha) 
		{
			RGB = FromHSV(hue, saturation, value, alpha);

			Hue = hue;
			Saturation = saturation;
			Value = value;
		}

		public HSVColor(Color origin)
		{
			RGB = origin;

			byte max = Math.Max(RGB.R, Math.Max(RGB.G, RGB.B));
			byte min = Math.Min(RGB.R, Math.Min(RGB.G, RGB.B));

			if      (max == min)   Hue = 0;
			else if (max == RGB.R) Hue = ((60 *      ((RGB.G - RGB.B) / (max - min)))  + 360) % 360;
			else if (max == RGB.G) Hue = ((60 * (2 + ((RGB.B - RGB.R) / (max - min)))) + 360) % 360;
			else if (max == RGB.B) Hue = ((60 * (4 + ((RGB.R - RGB.G) / (max - min)))) + 360) % 360;
			else throw new ArgumentException("No Value for Hue could be computed.");

			if (max == 0) Saturation = 0;
			else Saturation = (float)(max - min) / max;

			Value = max / 255f;
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hue">0 to 360</param>
		/// <param name="saturation">0 to 1</param>
		/// <param name="value">0 to 1</param>
		/// <returns></returns>
		public static Color FromHSV(int hue, float saturation, float value, byte alpha = 255)
		{
			float chroma = value * saturation;
			float section = hue / 60.0f;
			float x = chroma * (1 - Math.Abs(section % 2 - 1));
			byte m = (byte)(255 * (value - chroma));
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
			return new Color(r + m, g + m, b + m, alpha);
		}

		public static HSVColor operator* (HSVColor color, float f)
		{
			//DEBUG
			if (f == 1f)
				return color;
			return new HSVColor(color.RGB * f);
		}

		public override bool Equals(object obj)
		{
			if (obj is HSVColor hsv)
				return RGB.Equals(hsv.RGB);
			if (obj is Color rgb)
				return RGB.Equals(rgb);
			return base.Equals(obj);
		}

		public static HSVColor Transparent => new HSVColor(Color.Transparent);

		public static HSVColor White => new HSVColor(Color.White);
	}
}
