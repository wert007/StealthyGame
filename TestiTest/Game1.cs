﻿using Microsoft.Xna.Framework;
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
		DebugKeyPressed gameKeyboardDisplay;

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

			gameKeyboardDisplay = new DebugKeyPressed(gameKeyboardManager);
			DebugRenderer.AddDebugObjectScreen(gameKeyboardDisplay);

			InGameConsole.Init();

			consoleControl = new ConsoleControl(null);

			Parser.Parse(@".\Content\stdCommands.cmds", typeof(StealthyGame.Engine.GameDebug.Console.ConsoleCommands));
			Parser.Parse(@".\Content\commands.cmds", typeof(Console.ConsoleCommands));

			Console.ConsoleCommands.ClassTree = new ClassTree();
			Console.ConsoleCommands.ClassTree.SetRoot(this, "game1");

			base.Initialize();
		}

		private void ConsoleExit(ParameterValue[] args)
		{
			Exit();
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);
			renderer = new Renderer2D(batch);

			//pix = Content.Load<Texture2D>("Pixel");
			//DrawHelper.Pixel = pix;

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
				Console.ConsoleCommands.FreezeGame = !Console.ConsoleCommands.FreezeGame;
			}
			if (gameKeyboardManager.IsKeyPressed(Keys.L))
			{
				Console.ConsoleCommands.PlayLoop = !Console.ConsoleCommands.PlayLoop;

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
			
			if (Console.ConsoleCommands.FreezeGame)
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