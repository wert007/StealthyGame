using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoolinAround
{
	struct ColoredVertex
	{
		public const int Size = (3 + 4) * 4; // size of struct in bytes

		private readonly Vector3 position;
		private readonly Color4 color;

		public ColoredVertex(Vector3 position, Color4 color)
		{
			this.position = position;
			this.color = color;
		}
	}
}
