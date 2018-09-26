using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.GameDebug.GameConsole;
using StealthyGame.Engine.GameDebug.GameConsole.Parser;
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
using StealthyGame.Engine.GameDebug.DataStructures.TimeManagement;
using StealthyGame.Engine.GameDebug.DataStructures;

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
		Rectangle render;
		KeyboardManager debugKeyboardManager;
		ConsoleControl consoleControl;
		UpdateContainer updateContainer;
		Renderer2D renderer;
		FPSCounter counter;
		Recorder recorder;

		int width;
		int height;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			Content.RootDirectory = "Content";
			Window.TextInput += Window_TextInput;
		//	this.graphics.SynchronizeWithVerticalRetrace = true;
			this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33); // 33ms = 30fps
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
			render = new Rectangle(0, 0, width, height);
			recorder = new Recorder();

			FontManager.Initialize(Content);
			Control.Initialize(width, height);
			
			updateContainer = new UpdateContainer();
			debugKeyboardManager = new KeyboardManager();

			counter = new FPSCounter(this);
			DebugRenderer.AddDebugObjectScreen(new DebugFPS(counter));

			GameConsole.Init();

			consoleControl = new ConsoleControl(null);

			StdConsoleCommands.Init();
			Parser p = new Parser();

			var res5 = p.Parse(@".\Content\stdCommands.cmds", typeof(StdConsoleCommands));
			var res6 = p.Parse(@".\Content\commands.cmds", typeof(ConsoleCommands));
			GameConsole.Add(res5);
			GameConsole.Add(res6);
			StdConsoleCommands.ExitCalled += () => Exit();
		
			StdConsoleCommands.ClassTree.Root = new ClassTreeItem(this, "game1");

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
			if (!ConsoleCommands.FreezeGame)
			{

				GraphicsDevice.SetRenderTarget(gameContent);
				GraphicsDevice.Clear(Color.Black);
				phaseManager.Draw(renderer, time);
				GraphicsDevice.SetRenderTarget(null);
				render = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
				recorder.AddRecordEntry(new RecordEntry(gameContent, time));
			}
			renderer.Begin();
			renderer.Draw(gameContent, render, Color.White);
			if (ConsoleCommands.FreezeGame && ConsoleCommands.PlayLoop)
				recorder.Draw(renderer, time);
			consoleControl.Draw(renderer);
			batch.End();

			TimeWatcher.ClearCurrent();

			base.Draw(time);
		}
	}
}