using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class Quad
	{
		public Vector3[] Vertices => new Vector3[]
		{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(0, 1, 0),
			new Vector3(1, 1, 0),
		};

		public int[] Indices => new int[]
		{
			0, 1, 2, 2, 1, 3,
		};

		public int IndiceCount => Indices.Length;
		public Vector2 Position { get; set; }
		public Vector2 Scale { get; set; }

	}
}