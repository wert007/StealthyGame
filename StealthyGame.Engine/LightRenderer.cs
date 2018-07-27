using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.View;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine
{
	public class LightRenderer
	{
		Effect effect;
		List<VertexPositionColor> vertices;
		Matrix transform;
		List<int> indices;

		public LightRenderer(int width, int height)
		{
			vertices = new List<VertexPositionColor>();
			indices = new List<int>();
			//vertices[0].Position = new Vector3(50f, 50f, 0f);
			//vertices[0].Color = Color.Red;
			//vertices[1].Position = new Vector3(5, 5f, 0f);
			//vertices[1].Color = Color.Red;
			//vertices[2].Position = new Vector3(5f, 50f, 0f);
			//vertices[2].Color = Color.Green;


			Matrix toSmallCoords = Matrix.CreateScale(new Vector3(1f / (width * 0.5f), 1f / (height * 0.5f), 1));
			Matrix toCenter = Matrix.CreateTranslation(new Vector3(-1f, -1f, 0));
			Matrix tryAndFlipIt = Matrix.CreateScale(new Vector3(1, -1, 1));
			transform = toSmallCoords * toCenter * tryAndFlipIt;
		}

		public void AddLight(Light light)
		{
			Generate(light);
		}

		private void Generate(Light light)
		{
			int index = vertices.Count;
			vertices.Add(new VertexPositionColor(new Vector3(light.Position, 1), light.Color.RGB));
			//Top
			vertices.Add(new VertexPositionColor(new Vector3(light.Position.X, light.Position.Y - light.Radius, 0), light.Color.RGB));
			//Right
			vertices.Add(new VertexPositionColor(new Vector3(light.Position.X + light.Radius, light.Position.Y, 0), light.Color.RGB));
			//Bottom
			vertices.Add(new VertexPositionColor(new Vector3(light.Position.X, light.Position.Y + light.Radius, 0), light.Color.RGB));
			//Left
			vertices.Add(new VertexPositionColor(new Vector3(light.Position.X - light.Radius, light.Position.Y, 0), light.Color.RGB));
			indices.AddRange(new int[]
			{
				index, index + 1, index + 2,
				index, index + 2, index + 3,
				index, index + 3, index + 4,
				index, index + 4, index + 1,
			});
		}

		public void AddObstacle(Rectangle rect)
		{
			int index = vertices.Count;
			vertices.Add(new VertexPositionColor(new Vector3(rect.X, rect.Y, 0), Color.Black));
			vertices.Add(new VertexPositionColor(new Vector3(rect.X + rect.Width, rect.Y, 0), Color.Black));
			vertices.Add(new VertexPositionColor(new Vector3(rect.X + rect.Width, rect.Y + rect.Height, 0), Color.Black));
			vertices.Add(new VertexPositionColor(new Vector3(rect.X, rect.Y + rect.Height, 0), Color.Black));
			indices.AddRange(new int[]
			{
				index, index + 1, index + 2,
				index, index + 2, index + 3,
			});
		}

		public void Load(ContentManager content)
		{
			effect = content.Load<Effect>("HelloWorld");
		}

		public void Draw(GraphicsDevice graphicsDevice, Camera cam)
		{
			if (vertices.Count == 0) return;
			foreach (var pass in effect.Techniques[0].Passes)
			{
				effect.Parameters["matWorldViewProj"].SetValue(cam.Transform * transform);
				pass.Apply();
				//graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, 1);
				graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count, indices.ToArray(), 0, indices.Count / 3);
			}
		}
	}
}
