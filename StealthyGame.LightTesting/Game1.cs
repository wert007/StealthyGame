using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.View.Lighting;
using StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps;
using System;
using System.IO;

namespace StealthyGame.LightTesting
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		StaticLightMap smap;
		Texture2D pixel;
		Texture2D background;
		Texture2D heightmap;
		Obstacle obstacle;
		int width = 300;
		int height = 300;
		float i = 0;
		Color[] data;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			graphics.PreferredBackBufferHeight = width;
			graphics.PreferredBackBufferWidth = height;
			graphics.ApplyChanges();
			smap = new StaticLightMap(width, height, 1f);
			DebugSpriteBatch.Initialize();
			data = new Color[width * height];
			PerlinNoise.GenerateNoise();
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData(new Color[] { Color.White });
			DrawHelper.Pixel = pixel;
			DebugSpriteBatch.Load(pixel);

			background = Texture2D.FromStream(GraphicsDevice, new FileStream(@".\Content\justapicture.png", FileMode.Open));// Content.Load<Texture2D>("test");

			heightmap = new Texture2D(GraphicsDevice, width, height);
			
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Color current = Color.White;
					float v = PerlinNoise.Turbulence2D(x, y, 8);

					current = new Color(v, v, v);
					data[width * y + x] = current;
				}
			}
			heightmap.SetData(data);

			obstacle = new Obstacle(new Index2(5, 6), 3, 3);
			float[,] shape = new float[3, 3];
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					shape[x, y] = 1;
				}
			}
			obstacle.Update(shape);

			Obstacle obst = new Obstacle(new Index2(90, 50), 3, 3);
			obst.Update(shape);

			DebugSpriteBatch.AddObstacle(new DebugObstacle(obstacle, Color.DarkGray));
			DebugSpriteBatch.AddObstacle(new DebugObstacle(obst, Color.Blue));
			//smap.AddObstacle(obstacle);
			//smap.AddObstacle(obst);
			smap.AddObstacles(heightmap);
			smap.AddLight(new Light(new Index2(width / 2, height / 2), height / 2, 0.5f, new HSVColor(Color.White)));
			smap.Generate(null, new HSVColor(Color.Black), GraphicsDevice);
			Random random = new Random();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			i++;
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var m = Mouse.GetState();
			if(m.Position.X > 0 && m.Position.X < width - obstacle.Width &&
				m.Position.Y > 0 && m.Position.Y < height - obstacle.Height)
			{
				obstacle.Update((Index2)m.Position.ToVector2());
			}


			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Color current = Color.White;
					float v = PerlinNoise.Turbulence3D(x / (float)width, y / (float)height, i * 0.0005f, 50);
					if (v > 0.8f)
						current = Color.Black;
					data[width * y + x] = new Color(v,v,v);
				}
			}
			heightmap.SetData(data);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			spriteBatch.Draw(background, new Rectangle(0, 0, width, height), Color.White);
			spriteBatch.Draw(heightmap, new Rectangle(0, 0, width, height), Color.White);
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, new BlendState()
			{
				ColorSourceBlend = Blend.DestinationColor,
				ColorDestinationBlend = Blend.Zero,
				ColorBlendFunction = BlendFunction.Add
			});
			//smap.Draw(spriteBatch);
			spriteBatch.End();

			spriteBatch.Begin();
			DebugSpriteBatch.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
