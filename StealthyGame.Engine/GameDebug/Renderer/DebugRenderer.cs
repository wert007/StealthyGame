using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.MapBasics;

namespace StealthyGame.Engine.GameDebug
{
	public static class DebugRenderer
	{
		private static Map map;
		private static Dictionary<Type, DebugCollection> drawables;
		private static Dictionary<Type, DebugCollection> drawablesSingleCall;

		public static void SetMap(Map map)
		{
			DebugRenderer.map = map;
		}

		public static void AddDebugObject(IDebugObject debugNPC)
		{
			if (drawables == null)
				drawables = new Dictionary<Type, DebugCollection>();
			if (!drawables.ContainsKey(debugNPC.GetType()))
				drawables.Add(debugNPC.GetType(), new DebugCollection());
			drawables[debugNPC.GetType()].Add(debugNPC);
		}

		public static void AddDebugObjectSingleCall(IDebugObject debugObject)
		{
			if (drawablesSingleCall == null)
				drawablesSingleCall = new Dictionary<Type, DebugCollection>();
			if (!drawablesSingleCall.ContainsKey(debugObject.GetType()))
				drawablesSingleCall.Add(debugObject.GetType(), new DebugCollection());
			drawablesSingleCall[debugObject.GetType()].Add(debugObject);
		}

		public static void VisibilityOfType(Type type, bool visible)
		{
			drawables[type].Visible = visible;
			drawablesSingleCall[type].Visible = visible;
		}


		public static void Draw(SpriteBatch batch)
		{
			if (drawables != null)
				foreach (var drawable in drawables)
				{
					drawable.Value.Draw(batch);
				}
			if (drawablesSingleCall != null)
			{
				foreach (var drawable in drawablesSingleCall)
				{
					drawable.Value.Draw(batch);
				}
				drawablesSingleCall.Clear();
			}
		}
	}
}