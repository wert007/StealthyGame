using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class GraphicsDevice
	{
		public void Clear(Color color)
		{
			GL.ClearColor(color.ToColor4());
		}
	}
}
