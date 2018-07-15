namespace StealthyGame.Engine.OpenTKVersion
{
	public class Rectangle
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public Rectangle() : this(0, 0, 0, 0) { }
		public Rectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

	}
}