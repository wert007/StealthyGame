using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestiTest.GameMechanics.Phases.Containers;

namespace TestiTest.GameMechanics.Phases
{
	class GamePhase : Phase
	{
		Camera cam;
		int followedNPC;
		MyWorld w;
		bool debug;

		public GamePhase(GameContainer container) : base(PhaseType.InGame, null)
		{
			cam = new Camera(container.GraphicsDevice.Viewport);
			w = new MyWorld(cam);
			w.Load(@".\Content\Level\Maps\smallRoom.xml", container.GraphicsDevice);
			cam.Follow = w.npcs[0];
			followedNPC = 0;
			debug = false;
		}


		public override void Draw(SpriteBatch batch, GameTime time)
		{
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
			batch.End();


			if (debug)
			{
				batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, cam.Transform);
				DebugSpriteBatch.Draw(batch);
				batch.End();
			}

		}

		public override void Load(ContentManager content)
		{
		}

		public override void Update(GameTime time)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				followedNPC = (followedNPC + 1) % w.npcs.Count;
				cam.Follow = w.npcs[followedNPC];
			}

			if (Keyboard.GetState().IsKeyDown(Keys.N))
				debug = !debug;

			w.Update(time);
			cam.Update(time);
		}
	}
}
