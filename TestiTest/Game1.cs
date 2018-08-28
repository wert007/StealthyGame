using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
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
		Rectangle render;
		KeyboardManager keyboardManager;
		ClassTree classTree;
		ClassTreeControl classTreeControl;
		int width;
		int height;
		ScrollBar scrollBar;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			width = GraphicsDevice.Viewport.Width;
			height = GraphicsDevice.Viewport.Height;


			GameContainer gc = new GameContainer(GraphicsDevice, null);

			phaseManager = new PhaseManager(new GamePhase(gc));

			gameContent = new RenderTarget2D(GraphicsDevice, width, height);
			render = new Rectangle(0, 0, width, height);

			FontManager.Initialize(Content);
			Control.Initialize(width, height);

			keyboardManager = new KeyboardManager();

			classTree = new ClassTree();
			classTree.SetRoot(this);




			base.Initialize();
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);

			pix = Content.Load<Texture2D>("Pixel");
			DrawHelper.Pixel = pix;
			classTreeControl = new ClassTreeControl(null, classTree, Content.Load<Texture2D>("arrow"));
			scrollBar = new ScrollBar(null, Orientation.Vertical);
			scrollBar.HorizontalAlignment = HorizontalAlignment.Right;


			phaseManager.Load(Content);

		}

		protected override void Update(GameTime time)
		{



			classTreeControl.Update(time);

			keyboardManager.Update(time);
			if (keyboardManager.IsKeyPressed(Keys.F))
				freeze = !freeze;
			//if (Keyboard.GetState().IsKeyDown(Keys.I))
			//	DebugSpriteBatch.DrawPathfinding = !DebugSpriteBatch.DrawPathfinding;
			//if (Keyboard.GetState().IsKeyDown(Keys.O))
			//	DebugSpriteBatch.DrawPathfindingNeighbours = !DebugSpriteBatch.DrawPathfindingNeighbours;

			//IsMouseVisible = false;
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
				//renderTarget.Dispose();
				render = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			}
			batch.Begin();
			batch.Draw(gameContent, render, freeze ? Color.Gray : Color.White);
			if (freeze)
				classTreeControl.Draw(batch);
			scrollBar.Draw(batch);
			batch.End();

			base.Draw(time);
		}
	}
}