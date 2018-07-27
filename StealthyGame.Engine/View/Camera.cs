using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View
{
	public class Camera
	{
		public Matrix Transform { get; private set; }
		public static Camera Instance { get; private set; }
		Viewport view;
		Vector2 position;
		Vector2 velocity;
		Vector2 acceleration;
		Vector2 target;
		public float Zoom { get; set; } = 1f;
		public GameObject Follow { get; set; }

		public Camera(Viewport view)
		{
			this.view = view;
			position = new Vector2();
			velocity = new Vector2();
			acceleration = new Vector2();
			Instance = this;
		}

		public void Update(GameTime time)
		{
			if (Follow == null) return;
			target = Follow.Center;
			acceleration = target - position;
			acceleration.Normalize();
			acceleration *= 1;
			velocity += acceleration;
			acceleration = new Vector2();
			position += velocity;
			Transform = Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
				Matrix.CreateTranslation(new Vector3(-target.X * Zoom + view.Width / 2, -target.Y * Zoom + view.Height / 2, 0));
		}
	}
}
