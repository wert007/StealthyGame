using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.GameDebug.Console;
using StealthyGame.Engine.GameDebug.Console.Parser;
using StealthyGame.Engine.GameDebug.UI;
using StealthyGame.Engine.Dialogs;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI;
using StealthyGame.Engine.UI.DataTypes;
using StealthyGame.Engine.View;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using TestiTest.Console;
using TestiTest.GameMechanics.Phases;
using TestiTest.GameMechanics.Phases.Containers;
using StealthyGame.Engine.GameDebug.Renderer;
using StealthyGame.Engine.Renderer;
using System.IO;

namespace TestiTest
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch batch;
		PhaseManager phaseManager;
		RenderTarget2D gameContent;
		Queue<RenderTarget2D> lastFrames;
		Rectangle render;
		KeyboardManager debugKeyboardManager;
		ConsoleControl consoleControl;
		UpdateContainer updateContainer;
		Renderer2D renderer;

		int width;
		int height;
		private readonly int MaxSavedFrames = 80;
		int currentLastFrame = 0;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			Content.RootDirectory = "Content";
			Window.TextInput += Window_TextInput;
		}

		private void Window_TextInput(object sender, TextInputEventArgs e)
		{
			updateContainer.TextInput(sender, e);
			debugKeyboardManager.TextInput(sender, e);
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
			
			updateContainer = new UpdateContainer();
			debugKeyboardManager = new KeyboardManager();
			

			GameConsole.Init();

			consoleControl = new ConsoleControl(null);

			StdConsoleCommands.Init();
			Parser.Parse(@".\Content\stdCommands.cmds", typeof(StdConsoleCommands));
			Parser.Parse(@".\Content\commands.cmds", typeof(ConsoleCommands));
			StdConsoleCommands.ExitCalled += () => Exit();
			StdConsoleCommands.SaveFrames += (dir) =>
			{
				for (int i = 0; i < lastFrames.Count; i++)
				{
					RenderTarget2D cur = lastFrames.ElementAt(i);
					using (FileStream fs = new FileStream(Path.Combine(dir, (i + 1).ToString() + ".png"), FileMode.CreateNew))
						cur.SaveAsPng(fs, cur.Width, cur.Height);
				}
			};

			ConsoleCommands.ClassTree = new ClassTree();
			ConsoleCommands.ClassTree.SetRoot(this, "game1");

			base.Initialize();
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);
			renderer = new Renderer2D(batch);

			phaseManager.Load(Content);
		}

		protected override void Update(GameTime time)
		{
			debugKeyboardManager.Update(time);
			if (!consoleControl.Focused)
			{
				updateContainer.Update(time);
			}
			if (updateContainer.Keyboard.IsKeyPressed(Keys.F))
			{
				ConsoleCommands.FreezeGame = !ConsoleCommands.FreezeGame;
			}
			if (updateContainer.Keyboard.IsKeyPressed(Keys.L))
			{
				ConsoleCommands.PlayLoop = !ConsoleCommands.PlayLoop;
			}
			if (debugKeyboardManager.IsCtrlKeyPressed(Keys.C) || 
				(!consoleControl.Focused && debugKeyboardManager.IsCharPressed('/')))
			{
				if (consoleControl.Focused)
					consoleControl.Unfocus();
				else
					consoleControl.Focus();
			}
			IsMouseVisible = false;
			debugKeyboardManager.EndUpdate();
			updateContainer.EndUpdate();
			consoleControl.Update(time, debugKeyboardManager);
			
			if (ConsoleCommands.FreezeGame)
			{
				IsMouseVisible = true;
				return;
			}


			phaseManager.Update(updateContainer);

			base.Update(time);
		}

		protected override void Draw(GameTime time)
		{

			GraphicsDevice.Clear(new Color(51, 51, 51));
			render = new Rectangle(200, 0, GraphicsDevice.Viewport.Width - 200, GraphicsDevice.Viewport.Height - 80);
			if (!Console.ConsoleCommands.FreezeGame)
			{

				GraphicsDevice.SetRenderTarget(gameContent);
				GraphicsDevice.Clear(Color.Black);
				phaseManager.Draw(renderer, time);
				GraphicsDevice.SetRenderTarget(null);
				render = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			}
			renderer.Begin();
			renderer.Draw(gameContent, render, Color.White);
			if(!Console.ConsoleCommands.FreezeGame)
				lastFrames.Enqueue(CopyRenderTarget(gameContent));
			if (lastFrames.Count > MaxSavedFrames)
			{
				lastFrames.Dequeue().Dispose();
			}
			if (Console.ConsoleCommands.PlayLoop && time.TotalGameTime.Ticks % 3 == 0)
			{
				currentLastFrame = (currentLastFrame + 1) % lastFrames.Count;
			}
			if (Console.ConsoleCommands.FreezeGame)
			{
				if(Console.ConsoleCommands.PlayLoop)
				{
					batch.Draw(lastFrames.ElementAt(currentLastFrame), Vector2.Zero, Color.White);
				}
			}
			consoleControl.Draw(renderer);
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