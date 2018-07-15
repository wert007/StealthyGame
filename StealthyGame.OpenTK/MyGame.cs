using StealthyGame.Engine.OpenTKVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace StealthyGame.OpenTK
{
	class MyGame : Game
	{
		Sprite player;

		public MyGame()
		{
			player = new Sprite(@"FILENAME");
		}

		public override void Load()
		{
			Console.WriteLine("Loading..");
			Title = "Hello World!";
			GraphicsDevice.Clear(Color.CornflowerBlue);
		}

		public override void Update(GameTime time)
		{
			Console.WriteLine("Update..");
		}

		public override void Draw(GameTime time, SpriteBatch batch)
		{
			batch.DrawTest();
			Console.WriteLine("Drawing..");
		}
	}
}
