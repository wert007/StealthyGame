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
			player = new Sprite(@"justapicture.png");
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
			batch.Draw(player, new Rectangle(50, 50, 90, 90), Color.CornflowerBlue);
			//batch.DrawTest();
			Console.WriteLine("Drawing..");
		}
	}
}
