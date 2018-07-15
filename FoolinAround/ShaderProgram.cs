using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace FoolinAround
{
	sealed class ShaderProgram
	{
		private readonly int handle;

		public ShaderProgram(params Shader[] shaders)
		{
			// create program object
			this.handle = GL.CreateProgram();

			// assign all shaders
			foreach (var shader in shaders)
				GL.AttachShader(this.handle, shader.Handle);

			// link program (effectively compiles it)
			GL.LinkProgram(this.handle);

			// detach shaders
			foreach (var shader in shaders)
				GL.DetachShader(this.handle, shader.Handle);
		}

		public void Use()
		{
			// activate this program to be used
			GL.UseProgram(this.handle);
		}

		public int GetAttributeLocation(string name)
		{
			// get the location of a vertex attribute
			return GL.GetAttribLocation(this.handle, name);
		}

		public int GetUniformLocation(string name)
		{
			// get the location of a uniform variable
			return GL.GetUniformLocation(this.handle, name);
		}
	}
}
