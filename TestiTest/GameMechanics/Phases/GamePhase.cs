using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.GameDebug;
using StealthyGame.Engine.GameDebug.DataStructures.TimeManagement;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Renderer;
using StealthyGame.Engine.View;
using System.Reflection;
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


		public override void Draw(Renderer2D renderer, GameTime time)
		{
			renderer.Begin(cam.Transform);
			TimeWatcher.Start("Map.Draw");
			w.map.Draw(renderer, cam.Transform);
			TimeWatcher.End();
			renderer.End();

			if (debug)
			{
				renderer.Begin(cam.Transform);
				DebugRenderer.DrawWorld(renderer, time);
				renderer.End();

				renderer.Begin();
				DebugRenderer.DrawScreen(renderer, time);
				renderer.End();
			}

		}

		public override void Load(ContentManager content)
		{
		}

		public override void Update(UpdateContainer container)
		{
			DebugRenderer.Update(container);
			if (container.Keyboard.IsKeyPressed(Keys.U))
			{
				followedNPC = (followedNPC + 1) % w.npcs.Count;
				cam.Follow = w.npcs[followedNPC];
			}

			if (container.Keyboard.IsKeyPressed(Keys.N))
				debug = !debug;

			TimeWatcher.Start("World.Update");
			w.Update(container.Time);
			TimeWatcher.End();
			TimeWatcher.Start("Camera.Update");
			cam.Update(container.Time);
			TimeWatcher.End();
		}
	}
}
