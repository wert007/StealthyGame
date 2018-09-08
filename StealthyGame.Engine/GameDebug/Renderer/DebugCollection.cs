using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	internal class DebugCollection
	{
		List<IDebugObject> objects;
		public bool Visible { get; set; }

		public DebugCollection()
		{
			objects = new List<IDebugObject>();
			Visible = true;
		}

		internal void Add(IDebugObject debugNPC)
		{
			objects.Add(debugNPC);
		}
		

		internal void Draw(Renderer2D renderer)
		{
			if (!Visible)
				return;
			foreach (var obj in objects)
			{
				obj.Draw(renderer);
			}
		}
	}
}