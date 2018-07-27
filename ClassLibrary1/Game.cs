using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.OpenTKVersion
{
	public abstract class Game : GameWindow, IDisposable
	{
		public bool IsMouseVisible { get => CursorVisible; set => CursorVisible = value; }

		public new string Title { get => base.Title; set => base.Title = value; }
		public new int Width { get => base.Width; set => base.Width = value; }
		public new int Height { get => base.Height; set => base.Height = value; }
		public new int X { get => base.X; set => base.X = value; }
		public new int Y { get => base.Y; set => base.Y = value; }
		public new WindowIcon Icon => throw new NotImplementedException();
		public new bool VSync { get => base.VSync == VSyncMode.On; set
			{
				if (value)
					base.VSync = VSyncMode.On;
				else
					base.VSync = VSyncMode.Off;
			}
		} //TODO
		public new object WindowBorder => throw new NotImplementedException();
		public new object WindowState => throw new NotImplementedException();

		public GraphicsDevice GraphicsDevice { get; private set; }

		GameTime time;
		SpriteBatch batch;

		//int pgmID, vsID, fsID, attribute_vcol, attribute_vpos, uniform_mview, vbo_position, vbo_color,  vbo_mview;


		void initProgram()
		{	

		}

		public Game() : base(1280, 720, GraphicsMode.Default, "OpenTK Intro",
			 GameWindowFlags.FixedWindow, DisplayDevice.Default,
			 // ask for an OpenGL 3.0 forward compatible context
			 3, 0, GraphicsContextFlags.ForwardCompatible)
		{
			IsMouseVisible = true;
			time = new GameTime();
			batch = new SpriteBatch();
			batch.Init();
			GraphicsDevice = new GraphicsDevice();
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);
		}
		protected override void OnLoad(EventArgs e)
		{
			
			Load();
			GL.PointSize(5f);
		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			time.AddSeconds(e.Time);
			Update(time);


		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			time.AddSeconds(e.Time);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Draw(time, batch);
			

			GL.Flush();

			SwapBuffers();
		}

		public new void Close() => base.Close();


		public new abstract void Load();
		public abstract void Update(GameTime time);
		public abstract void Draw(GameTime time, SpriteBatch batch);
	
	}
}