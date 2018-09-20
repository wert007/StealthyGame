using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.MapBasics;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	public static class DebugRenderer
	{
		private static Map map;
		private static Dictionary<Type, DebugCollection> drawablesWorld;
		private static Dictionary<Type, DebugCollection> drawablesScreen;
		private static Dictionary<Type, DebugCollection> drawablesWorldSingleCall;

		public static void SetMap(Map map)
		{
			DebugRenderer.map = map;
		}

		public static void AddDebugObjectWorld(IDebugObject debugNPC)
		{
			if (drawablesWorld == null)
				drawablesWorld = new Dictionary<Type, DebugCollection>();
			if (!drawablesWorld.ContainsKey(debugNPC.GetType()))
				drawablesWorld.Add(debugNPC.GetType(), new DebugCollection());
			drawablesWorld[debugNPC.GetType()].Add(debugNPC);
		}

		public static void AddDebugObjectScreen(IDebugObject debugObject)
		{
			if (drawablesScreen == null)
				drawablesScreen = new Dictionary<Type, DebugCollection>();
			if (!drawablesScreen.ContainsKey(debugObject.GetType()))
				drawablesScreen.Add(debugObject.GetType(), new DebugCollection());
			drawablesScreen[debugObject.GetType()].Add(debugObject);
		}

		public static void AddDebugObjectWorldSingleCall(IDebugObject debugObject)
		{
			if (drawablesWorldSingleCall == null)
				drawablesWorldSingleCall = new Dictionary<Type, DebugCollection>();
			if (!drawablesWorldSingleCall.ContainsKey(debugObject.GetType()))
				drawablesWorldSingleCall.Add(debugObject.GetType(), new DebugCollection());
			drawablesWorldSingleCall[debugObject.GetType()].Add(debugObject);
		}

		public static void VisibilityOfType(Type type, bool visible)
		{
			drawablesWorld[type].Visible = visible;
			drawablesWorldSingleCall[type].Visible = visible;
		}

		public static void Update(UpdateContainer container)
		{
			if (drawablesWorld != null)
				foreach (var drawable in drawablesWorld)
				{
					drawable.Value.Update(container);
				}
			if (drawablesWorldSingleCall != null)
				foreach (var drawable in drawablesWorldSingleCall)
				{
					drawable.Value.Update(container);
				}
			if (drawablesScreen != null)
				foreach (var drawable in drawablesScreen)
				{
					drawable.Value.Update(container);
				}
		}

		public static void DrawWorld(Renderer2D renderer, GameTime time)
		{
			if (drawablesWorld != null)
				foreach (var drawable in drawablesWorld)
				{
					drawable.Value.Draw(renderer, time);
				}
			if (drawablesWorldSingleCall != null)
			{
				foreach (var drawable in drawablesWorldSingleCall)
				{
					drawable.Value.Draw(renderer, time);
				}
				drawablesWorldSingleCall.Clear();
			}

		}

		public static void DrawScreen(Renderer2D renderer, GameTime time)
		{
			if (drawablesScreen != null)
			{
				foreach (var drawable in drawablesScreen)
				{
					drawable.Value.Draw(renderer, time);
				}
			}
		}
	}
}