using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameMechanics.Phases;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameDebug
{
	//May inheritate from IDebugObject itself??
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
		
		internal void Update(UpdateContainer container)
		{
			foreach (var obj in objects)
			{
				obj.Update(container);
			}
		}

		internal void Draw(Renderer2D renderer, GameTime time)
		{
			if (!Visible)
				return;
			foreach (var obj in objects)
			{
				obj.Draw(renderer, time);
			}
		}
	}
}