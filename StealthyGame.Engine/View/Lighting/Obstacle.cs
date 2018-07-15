using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public class Obstacle
	{
		public Index2 Position { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public float[,] Shape { get; private set; }

		private bool changed;
		public bool Changed
		{
			get
			{
				if (changed)
				{
					changed = false;
					return true;
				}
				return false;
			}
		}

		public Obstacle(GameObject gameObject)
		{
			Position = gameObject.Position.ToIndex2();
			Width = (int)Math.Round(gameObject.Size.X);
			Height = (int)Math.Round(gameObject.Size.Y);
			Shape = new float[Width, Height];
			changed = true;
		}

		public Obstacle(Index2 position, int width, int height)
		{
			Position = position;
			Width = width;
			Height = height;
			Shape = new float[width, height];
			changed = true;
		}

		public void Update(float[,] shape)
		{
			if (shape.GetLength(0) != Width ||
				shape.GetLength(1) != Height)
				Console.WriteLine("Warning: Array size doesn't fit obstacle size.");
			Shape = shape;
			changed = true;
		}

		public void Update(Index2 position)
		{
			Position = position;
			changed = true;
		}
	}
}
