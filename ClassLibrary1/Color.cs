using System;
using OpenTK.Graphics;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class Color
	{
		Color4 color;
		public Color(Color4 color)
		{
			this.color = color;
		}

		internal Color4 ToColor4()
		{
			return color;
		}






		public static Color CornflowerBlue => new Color(Color4.CornflowerBlue);
	}
}