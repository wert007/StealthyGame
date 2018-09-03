using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Debug.Console;
using StealthyGame.Engine.Debug.Console.Parser;
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
using TestiTest.Console;
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
		RenderTarget2D gameContent;
		Queue<RenderTarget2D> lastFrames;
		Rectangle render;
		KeyboardManager gameKeyboardManager;
		KeyboardManager debugKeyboardManager;
		ConsoleControl consoleControl;
		int width;
		int height;
		private readonly int MaxSavedFrames = 80;
		int currentLastFrame = 0;
		ConsoleMessage[] msg;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			Content.RootDirectory = "Content";
			Window.TextInput += Window_TextInput;
		}

		private void Window_TextInput(object sender, TextInputEventArgs e)
		{
			gameKeyboardManager.TextInput(sender, e);
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
			
			gameKeyboardManager = new KeyboardManager();
			debugKeyboardManager = new KeyboardManager();

			InGameConsole.Init();

			consoleControl = new ConsoleControl(null);

			Parser.Parse(@".\Content\commands.cmds", typeof(ConsoleCommands));
			//InGameConsole.AddCommand(new ConsoleCommand("exit", ConsoleExit));
			//InGameConsole.AddCommand(new ConsoleCommand("loop", ConsoleCommands.ConsoleLoop));
			//InGameConsole.AddCommand(new ConsoleCommand("freeze", ConsoleCommands.ConsoleFreeze));
			//InGameConsole.AddCommand(new ConsoleCommand("inspect", ConsoleCommands.ConsoleInspect, 
			//	new Parameter("object", "o", true, true, ParameterType.File),
			//	new Parameter("reset", "r", false, true, ParameterType.Boolean),
			//	new Parameter("current", "c", false, true, ParameterType.Boolean)));
			msg = new ConsoleMessage[5];
			msg[0] = new ConsoleMessage(string.Empty, Color.White, Color.Blue);
			msg[1] = new ConsoleMessage(string.Empty, Color.White, Color.Blue);
			msg[2] = new ConsoleMessage("\t\t\t\t\t\tHeyyy", Color.White, Color.Blue);
			msg[3] = new ConsoleMessage(string.Empty, Color.White, Color.Blue);
			msg[4] = new ConsoleMessage(string.Empty, Color.White, Color.Blue);
			InGameConsole.Log(msg);

			ConsoleCommands.ClassTree = new ClassTree();
			ConsoleCommands.ClassTree.SetRoot(this, "game1");

			base.Initialize();
		}

		private void ConsoleExit(ParameterValue[] args)
		{
			Exit();
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
			debugKeyboardManager.Update(time);
			if (!consoleControl.Focused)
			{
				gameKeyboardManager.Update(time);
			}
			if (gameKeyboardManager.IsKeyPressed(Keys.F))
			{
				ConsoleCommands.FreezeGame = !ConsoleCommands.FreezeGame;
			}
			if (gameKeyboardManager.IsKeyPressed(Keys.L))
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
			gameKeyboardManager.EndUpdate();
			consoleControl.Update(time, debugKeyboardManager);

			if (time.TotalGameTime.Ticks % 5 == 0)
			{
				for (int i = 0; i < msg.Length; i++)
				{ 
					if (msg[i].BackgroundColor == Color.Blue)
					{
						msg[i].BackgroundColor = Color.Yellow;
						msg[i].Color = Color.Black;
					}
					else
					{
						msg[i].BackgroundColor = Color.Blue;
						msg[i].Color = Color.White;
					}
				}
			}

			if (ConsoleCommands.FreezeGame)
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
			if (!ConsoleCommands.FreezeGame)
			{

				GraphicsDevice.SetRenderTarget(gameContent);
				GraphicsDevice.Clear(Color.Black);
				phaseManager.Draw(batch, time);
				GraphicsDevice.SetRenderTarget(null);
				render = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			}
			batch.Begin();
			batch.Draw(gameContent, render, Color.White);
			if(!ConsoleCommands.FreezeGame)
				lastFrames.Enqueue(CopyRenderTarget(gameContent));
			if (lastFrames.Count > MaxSavedFrames)
			{
				lastFrames.Dequeue().Dispose();
			}
			if (ConsoleCommands.PlayLoop && time.TotalGameTime.Ticks % 3 == 0)
			{
				currentLastFrame = (currentLastFrame + 1) % lastFrames.Count;
			}
			if (ConsoleCommands.FreezeGame)
			{
				if(ConsoleCommands.PlayLoop)
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