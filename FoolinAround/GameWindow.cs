﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace FoolinAround
{
	public class GameWindow : OpenTK.GameWindow, IDisposable
	{
		private VertexBuffer<ColoredVertex> vertexBuffer;
		private ShaderProgram shaderProgram;
		private VertexArray<ColoredVertex> vertexArray;
		private Matrix4Uniform projectionMatrix;

		public GameWindow()
			 // set window resolution, title, and default behaviour
			 : base(1280, 720, GraphicsMode.Default, "OpenTK Intro",
			 GameWindowFlags.FixedWindow, DisplayDevice.Default,
			 // ask for an OpenGL 3.0 forward compatible context
			 3, 0, GraphicsContextFlags.ForwardCompatible)
		{
			Console.WriteLine("gl version: " + GL.GetString(StringName.Version));
				
			this.Run();
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);
			Console.WriteLine("Window Resized.");
		}

		protected override void OnLoad(EventArgs e)
		{
			// this is called when the window starts running
			Console.WriteLine("Loading started");

			this.vertexBuffer = new VertexBuffer<ColoredVertex>(ColoredVertex.Size);

			this.vertexBuffer.AddVertex(new ColoredVertex(new Vector3(-1, -1, -1.5f), Color4.Lime));
			this.vertexBuffer.AddVertex(new ColoredVertex(new Vector3(1, 1, -1.5f), Color4.Red));
			this.vertexBuffer.AddVertex(new ColoredVertex(new Vector3(1, -1, -1.5f), Color4.Blue));

			// load shaders
			#region Shaders

			var vertexShader = new Shader(ShaderType.VertexShader,
@"#version 130
// a projection transformation to apply to the vertex' position
uniform mat4 projectionMatrix;
// attributes of our vertex
in vec3 vPosition;
in vec4 vColor;
out vec4 fColor; // must match name in fragment shader
void main()
{
    // gl_Position is a special variable of OpenGL that must be set
	gl_Position = projectionMatrix * vec4(vPosition, 1.0);
	fColor = vColor;
}"
				 );
			var fragmentShader = new Shader(ShaderType.FragmentShader,
@"#version 130
in vec4 fColor; // must match name in vertex shader
out vec4 fragColor; // first out variable is automatically written to the screen
void main()
{
    fragColor = fColor;
}"
				 );

			#endregion

			// link shaders into shader program
			this.shaderProgram = new ShaderProgram(vertexShader, fragmentShader);

			// create vertex array to specify vertex layout
			this.vertexArray = new VertexArray<ColoredVertex>(
				 this.vertexBuffer, this.shaderProgram,
				 new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, ColoredVertex.Size, 0),
				 new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, ColoredVertex.Size, 12)
				 );

			// create projection matrix uniform
			this.projectionMatrix = new Matrix4Uniform("projectionMatrix");
		
			this.projectionMatrix.Matrix = Matrix4.CreatePerspectiveFieldOfView(
				 MathHelper.PiOver2, 16f / 9, 0.1f, 100f);

		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			// this is called every frame, put game logic here
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			// clea the screen
			GL.ClearColor(231f / 255f, 60f / 255f, 0f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// activate shader program and set uniforms
			this.shaderProgram.Use();
			this.projectionMatrix.Set(this.shaderProgram);

			// bind vertex buffer and array objects
			this.vertexBuffer.Bind();
			this.vertexArray.Bind();

			// upload vertices to GPU and draw them
			this.vertexBuffer.BufferData();
			this.vertexBuffer.Draw();

			// reset state for potential further draw calls (optional, but good practice)
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.UseProgram(0);

			// swap backbuffer
			this.SwapBuffers();
		}
	}
}
