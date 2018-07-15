using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.View.Lighting;
using StealthyGame.Engine.View.Lighting.LightStorages.Lightmaps;
using StealthyGame.Wordpress.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace StealthyGame.Wordpress
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static int bigFieldSize = 2;
		public static int smallFieldSize = 1;

		public static SpriteFont font;

		public readonly int width = bigFieldSize * Field.Size + 2 * smallFieldSize * Field.Size;
		public readonly int height = bigFieldSize * Field.Size;

		Random random;
		Field main;
		Field raycast;
		Field storeRaycast;
		Field shadowRaycast;
		Field jaField;
		Field debug;
		Field bField;
		List<Field> fields;

		int speed = 1;
		bool pause;
		bool singleStepAnimation = true;
		bool drawLess = true;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			graphics.ApplyChanges();



			random = new Random();
			Field.Init(GraphicsDevice, random);
			fields = new List<Field>();
			raycast = new RaycastField("Raycasts");
			bField = new Field("Basis");
			debug = new DebugField("Debug");
			storeRaycast = new RaycastStoringField("Raycast Storing");
			shadowRaycast = new ShadowCastingField("Shadow Casting");
			jaField = new SpiralField("Spiral");
			main = bField;
			//fields.Add(raycast);
			fields.Add(bField);
			//fields.Add(debug);
			fields.Add(storeRaycast);
			//fields.Add(shadowRaycast);
			fields.Add(jaField);


			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < 1000; i++)
			{
			//	storeRaycast.Compute();
			}
			sw.Stop();
			Console.WriteLine("Storing Raycasts took: " + sw.ElapsedMilliseconds + "ms");
			sw.Reset();
			sw.Start();
			for (int i = 0; i < 1000; i++)
			{
				//jaField.Compute();
			}
			sw.Stop();
			Console.WriteLine("Spiral took: " + sw.ElapsedMilliseconds + "ms");

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData(new Color[] { Color.White });
			DrawHelper.Pixel = pixel;

			font = Content.Load<SpriteFont>("font");
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.F5))
			{
				Field.Init(GraphicsDevice, random);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				speed--;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.E))
			{
				speed++;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.P))
			{
				pause = !pause;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.N))
			{
				singleStepAnimation = !singleStepAnimation;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.L))
			{
				drawLess = !drawLess;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.R))
			{
				fields.ForEach(f => f.Reset());
			}
			if(Keyboard.GetState().IsKeyDown(Keys.S))
			{
				main = fields[(fields.IndexOf(main) + 1) % fields.Count];
			}
			for (int i = 0; i < Math.Max(speed, 1) && !pause; i++)
			{
				if (speed < 0 && gameTime.TotalGameTime.Ticks % -speed != 0)
					break;
				if (singleStepAnimation)
					fields.ForEach(f => f.SimulateStep());
				else
					fields.ForEach(f => f.Compute());
			}
			base.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();
			Field.Draw(spriteBatch, main, bigFieldSize, new Index2(0, 0), true);
			if (!drawLess)
			{
				Field.Draw(spriteBatch, debug, smallFieldSize, new Index2(bigFieldSize * Field.Size, 0), false);
				Field.Draw(spriteBatch, raycast, smallFieldSize, new Index2(bigFieldSize * Field.Size, smallFieldSize * Field.Size), false);
				Field.Draw(spriteBatch, storeRaycast, smallFieldSize, new Index2(bigFieldSize * Field.Size, 2 * smallFieldSize * Field.Size), false);
				Field.Draw(spriteBatch, shadowRaycast, smallFieldSize, new Index2(bigFieldSize * Field.Size + smallFieldSize * Field.Size, 0), false);
				Field.Draw(spriteBatch, jaField, smallFieldSize, new Index2(bigFieldSize * Field.Size + smallFieldSize * Field.Size, smallFieldSize * Field.Size), false);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
