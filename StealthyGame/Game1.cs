using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Dialogs;
using StealthyGame.Engine.GameObjects;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.View;
using System;

namespace StealthyGame
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch batch;
		MyWorld w;
		Camera cam;
		Texture2D pix;
		bool debug;
		int i;
		Random random;

		Voronoi v;
		Area[] a;

		public Game1()
		{
			i = 0;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			random = new Random();


		Index2[] ind  = new Index2[]{
				new Index2(50, 50),
				new Index2(50, 90),
				new Index2(90, 90),
				new Index2(350, 190),
				new Index2(90, 50) };
			a = Voronoi.Generate(new Rectangle(0,0, 640, 480).ToArea(), ind);
		}

		protected override void Initialize()
		{
			base.Initialize();
			cam = new Camera(GraphicsDevice.Viewport);
			DebugSpriteBatch.Initialize();
			w = new MyWorld(cam);
			w.Load(@".\Content\Level\Maps\map.xml", GraphicsDevice);
			cam.Follow = w.npcs[0];
			DialogManager.Load(@".\Content\Level\Dialogs\dialog.xml");
			debug = false;
			//foronoi = new Voronoi(new Index2(50, 70), new Index2(700, 60), new Index2(560, 790), new Index2(60, 700));
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);
			pix = Content.Load<Texture2D>("Pixel");
			DebugSpriteBatch.Load(pix);
			DrawHelper.Pixel = pix;
		}

		protected override void Update(GameTime time)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.I))
				DebugSpriteBatch.DrawPathfinding = !DebugSpriteBatch.DrawPathfinding;
			if (Keyboard.GetState().IsKeyDown(Keys.O))
				DebugSpriteBatch.DrawPathfindingNeighbours = !DebugSpriteBatch.DrawPathfindingNeighbours;
			if (Keyboard.GetState().IsKeyDown(Keys.N))
				debug = !debug;

			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				i = (i + 1) % w.npcs.Count;
				cam.Follow = w.npcs[i];
			}
			if (Keyboard.GetState().IsKeyDown(Keys.L))
				w.map.Lightmap.Generate(w.map, new HSVColor(new Color(0.1f, 0.1f, 0.1f)), graphics.GraphicsDevice);

			w.Update(time);

			cam.Update(time);

			base.Update(time);
		}

		protected override void Draw(GameTime time)
		{
			GraphicsDevice.Clear(new Color(0.1f, 0.15f, 0.15f)); // new Color(51, 51, 51));

			batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, cam.Tranform);
			w.map.Draw(batch, cam.Tranform);
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
				batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, cam.Tranform);
				DebugSpriteBatch.Draw(batch);
			}

			batch.End();
			//batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, null);
			//foreach (var area in a)
			//{
			//	Color c = random.NextColor();
			//	foreach (var p in area.All)
			//	{
			//		batch.DrawIndex2(p, c);
			//	}
			//}
			//batch.End();

			base.Draw(time);
		}
	}
}