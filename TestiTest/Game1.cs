using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Debug.Console;
using StealthyGame.Engine.Debug.UI;
using StealthyGame.Engine.Dialogs;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI;
using StealthyGame.Engine.UI.DataTypes;
using StealthyGame.Engine.UI.Engine;
using StealthyGame.Engine.View;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using TestiTest.GameMechanics.Phases;
using TestiTest.GameMechanics.Phases.Containers;

namespace TestiTest
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch batch;
		Texture2D pix;
		PhaseManager phaseManager;
		bool freeze;
		RenderTarget2D gameContent;
		Queue<RenderTarget2D> lastFrames;
		Rectangle render;
		KeyboardManager keyboardManager;
		ConsoleControl consoleControl;
		int width;
		int height;
		private readonly int MaxSavedFrames = 80;
		int currentLastFrame = 0;
		bool playLoop = false;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			Content.RootDirectory = "Content";
			Window.TextInput += Window_TextInput;
		}

		private void Window_TextInput(object sender, TextInputEventArgs e)
		{
			keyboardManager.TextInput(sender, e);
			Control.TextInput(sender, e);
		}

		protected override void Initialize()
		{
			width = GraphicsDevice.Viewport.Width;
			height = GraphicsDevice.Viewport.Height;


			GameContainer gc = new GameContainer(GraphicsDevice, null);

			phaseManager = new PhaseManager(new GamePhase(gc));

			gameContent = new RenderTarget2D(GraphicsDevice, width, height);
			lastFrames = new Queue<RenderTarget2D>();
			render = new Rectangle(0, 0, width, height);

			FontManager.Initialize(Content);
			Control.Initialize(width, height);
			
			keyboardManager = new KeyboardManager();

			consoleControl = new ConsoleControl(null);
			InGameConsole.TextReceived += (txt) =>
			{
				Console.WriteLine(txt);
			};
			InGameConsole.AddCommand(new ConsoleCommand("loop", ConsoleLoop));

			base.Initialize();
		}

		private void ConsoleLoop(object[] args)
		{
			playLoop = !playLoop;
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);

			pix = Content.Load<Texture2D>("Pixel");
			DrawHelper.Pixel = pix;

			phaseManager.Load(Content);
		}

		protected override void Update(GameTime time)
		{
			keyboardManager.Update(time);
			if (!consoleControl.Focused)
			{
				if (keyboardManager.IsKeyPressed(Keys.F))
				{
					InGameConsole.Log("Freeze");
					freeze = !freeze;
				}
				if (keyboardManager.IsKeyPressed(Keys.L))
				{
					InGameConsole.Log("Loop");
					playLoop = !playLoop;

				}
			}
			if(keyboardManager.IsCtrlKeyPressed(Keys.C))
			{
				if (consoleControl.Focused)
					consoleControl.Unfocus();
				else
					consoleControl.Focus();
			}
			IsMouseVisible = false;

			if (freeze)
			{
				IsMouseVisible = true;
				return;
			}


			phaseManager.Update(time);

			base.Update(time);
		}

		protected override void Draw(GameTime time)
		{

			GraphicsDevice.Clear(new Color(51, 51, 51));
			render = new Rectangle(200, 0, GraphicsDevice.Viewport.Width - 200, GraphicsDevice.Viewport.Height - 80);
			if (!freeze)
			{

				GraphicsDevice.SetRenderTarget(gameContent);
				GraphicsDevice.Clear(Color.Black);
				phaseManager.Draw(batch, time);
				GraphicsDevice.SetRenderTarget(null);
				render = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			}
			batch.Begin();
			batch.Draw(gameContent, render, freeze ? Color.Gray : Color.White);
			if(!freeze)
				lastFrames.Enqueue(CopyRenderTarget(gameContent));
			if (lastFrames.Count > MaxSavedFrames)
			{
				lastFrames.Dequeue().Dispose();
			}
			if (playLoop && time.TotalGameTime.Ticks % 3 == 0)
			{
				currentLastFrame = (currentLastFrame + 1) % lastFrames.Count;
			}
			if (freeze)
			{
				if(playLoop)
				{
					batch.Draw(lastFrames.ElementAt(currentLastFrame), Vector2.Zero, Color.White);
				}
			}
			consoleControl.Draw(batch);
			batch.End();

			base.Draw(time);
		}



		private RenderTarget2D CopyRenderTarget(RenderTarget2D gameContent)
		{
			RenderTarget2D result = new RenderTarget2D(gameContent.GraphicsDevice, gameContent.Width, gameContent.Height);
			Color[] data = new Color[gameContent.Width * gameContent.Height];
			gameContent.GetData(data);
			result.SetData(data);
			return result;
		}
	}
}