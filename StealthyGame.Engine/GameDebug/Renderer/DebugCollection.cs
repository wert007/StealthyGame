using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.Engine.Debug
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
		

		internal void Draw(SpriteBatch batch)
		{
			if (!Visible)
				return;
			foreach (var obj in objects)
			{
				obj.Draw(batch);
			}
		}
	}
}