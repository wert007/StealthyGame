using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.UI.DataTypes
{
	public struct Thickness
	{
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }

		public Thickness(int unit)
		{
			Left = unit;
			Top = unit;
			Right = unit;
			Bottom = unit;
		}

		public Thickness(int horizontal, int vertical)
		{
			Left = horizontal;
			Right = horizontal;
			Top = vertical;
			Bottom = vertical;
		}

		public Thickness(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public int HorizontalSum()
		{
			return Left + Right;
		}

		public int VerticalSum()
		{
			return Top + Bottom;
		}
	}
}
