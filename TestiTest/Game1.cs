using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Dialogs;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.View;
using StealthyGame.Engine.View.Lighting;

namespace TestiTest
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch batch;
		Camera cam;
		bool debug;
		Texture2D pix;
		int followedNPC;
		MyWorld w;
		LightRenderer lr;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;
			//temp

			Content.RootDirectory = "Content";
		}
		
		protected override void Initialize()
		{
			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;
			//GraphicsDevice.RasterizerState = rasterizerState;

			cam = new Camera(GraphicsDevice.Viewport);
			w = new MyWorld(cam);
			w.Load(@".\Content\Level\Maps\map.xml", GraphicsDevice);
			cam.Follow = w.npcs[0];
			DialogManager.Load(@".\Content\Level\Dialogs\dialog.xml");
			debug = false;
			followedNPC = 0;
			lr = new LightRenderer(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			Light light = new Light(new Index2(192, 160), 64, 1, new HSVColor(Color.AliceBlue));
			lr.AddLight(light);
			lr.AddObstacle(new Rectangle(150, 140, 20, 40));
			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);

			pix = Content.Load<Texture2D>("Pixel");
			DrawHelper.Pixel = pix;

			lr.Load(Content);
		}

		protected override void Update(GameTime time)
		{
			//if (Keyboard.GetState().IsKeyDown(Keys.I))
			//	DebugSpriteBatch.DrawPathfinding = !DebugSpriteBatch.DrawPathfinding;
			//if (Keyboard.GetState().IsKeyDown(Keys.O))
			//	DebugSpriteBatch.DrawPathfindingNeighbours = !DebugSpriteBatch.DrawPathfindingNeighbours;
			if (Keyboard.GetState().IsKeyDown(Keys.N))
				debug = !debug;

			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				followedNPC = (followedNPC + 1) % w.npcs.Count;
				cam.Follow = w.npcs[followedNPC];
			}

			w.Update(time);
			cam.Update(time);
			
			base.Update(time);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.MonoGameOrange);


			batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, cam.Transform);
			w.map.Draw(batch, cam.Transform);
			//batch.End();
			//batch.Begin(SpriteSortMode.Deferred, new BlendState()
			//{
			//	ColorSourceBlend = Blend.DestinationColor,
			//	ColorDestinationBlend = Blend.Zero,
			//	ColorBlendFunction = BlendFunction.Add
			//}, null, null, null, null, cam.Tranform);
			//w.map.Lightmap.Draw(batch);

			if (debug)
			{
				batch.End();
				batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, cam.Transform);
				DebugSpriteBatch.Draw(batch);
			}

			batch.End();
			lr.Draw(GraphicsDevice, cam);

			base.Draw(gameTime);
		}
	}
}
