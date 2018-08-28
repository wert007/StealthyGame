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
		Viewport viewport;
		Vector2 target;
		public float Zoom { get; set; } = 1f;
		public GameObject Follow { get; set; }
		Rectangle view;
		public Rectangle View => view;

		public Camera(Viewport viewport)
		{
			this.viewport = viewport;
			Instance = this;
			view = new Rectangle(0, 0, viewport.Width, viewport.Height);
		}

		public void Update(GameTime time)
		{
			if (Follow == null) return;
			target = Follow.Center;
			Transform = Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
				Matrix.CreateTranslation(new Vector3(-target.X * Zoom + viewport.Width / 2, -target.Y * Zoom + viewport.Height / 2, 0));
			view.X = (int)target.X - view.Width / 2;
			view.Y = (int)target.Y - view.Height / 2;
		}
	}
}
