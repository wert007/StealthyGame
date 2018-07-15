using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class SpriteBatch
	{
		int indiceat;
		Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
		string activeShader = "default";
		int ibo_elements;




		int[] indicedata;
		Vector3[] vertdata;
		Vector3[] coldata;
		Matrix4[] mviewdata;

		public void Init()
		{
			GL.GenBuffers(1, out ibo_elements);

			shaders.Add("default", new ShaderProgram("Shaders/VertexShaders/vs.glsl", "Shaders/FragmentShaders/fs.glsl", true));
		}

		public void Load()
		{
			vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f,  -0.8f),
					 new Vector3(0.8f, -0.8f,  -0.8f),
					 new Vector3(0.8f, 0.8f,  -0.8f),
					 new Vector3(-0.8f, 0.8f,  -0.8f),
					 new Vector3(-0.8f, -0.8f,  0.8f),
					 new Vector3(0.8f, -0.8f,  0.8f),
					 new Vector3(0.8f, 0.8f,  0.8f),
					 new Vector3(-0.8f, 0.8f,  0.8f),
				};

			coldata = new Vector3[] { new Vector3(1f, 0f, 0f),
					 new Vector3( 0f, 0f, 1f),
					 new Vector3( 0f,  1f, 0f),new Vector3(1f, 0f, 0f),
					 new Vector3( 0f, 0f, 1f),
					 new Vector3( 0f,  1f, 0f),new Vector3(1f, 0f, 0f),
					 new Vector3( 0f, 0f, 1f)};

			indicedata = new int[]{
                //front
                0, 7, 3,
					 0, 4, 7,
                //back
                1, 2, 6,
					 6, 5, 1,
                //left
                0, 2, 1,
					 0, 3, 2,
                //right
                4, 5, 6,
					 6, 7, 4,
                //top
                2, 3, 6,
					 6, 3, 7,
                //bottom
                0, 1, 5,
					 0, 5, 4
				};

			mviewdata = new Matrix4[]{
					 Matrix4.Identity
				};
		}

		public void Update()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

			if (shaders[activeShader].GetAttribute("vColor") != -1)
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
				GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
				GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
			}

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

			GL.UseProgram(shaders[activeShader].ProgramID);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Draw(Sprite sprite, Rectangle rectangle, Color color)
		{
			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

			GL.MatrixMode(MatrixMode.Modelview);

			GL.LoadMatrix(ref modelview);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, sprite.ID);
			//GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref sprite.ModelViewProjectionMatrix);

			if (shaders[activeShader].GetUniform("maintexture") != -1)
			{
				GL.Uniform1(shaders[activeShader].GetUniform("maintexture"), 0);
			}

			GL.DrawElements(BeginMode.Triangles, sprite.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
			indiceat += sprite.IndiceCount;
		}

		public void DrawTest()
		{
			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

			GL.MatrixMode(MatrixMode.Modelview);

			GL.LoadMatrix(ref modelview);

			GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref modelview);
			shaders[activeShader].EnableVertexAttribArrays();
			GL.DrawElements(BeginMode.Triangles, indicedata.Length, DrawElementsType.UnsignedInt, 0);
			shaders[activeShader].DisableVertexAttribArrays();

		}
	}
}
