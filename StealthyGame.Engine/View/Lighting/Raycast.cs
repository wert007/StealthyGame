using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.MapBasics.Tiles;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.Geometrics;
using System.Collections;

namespace StealthyGame.Engine.View.Lighting
{
	public class Raycast : Line
	{
		public Raycast(Index2 start, Index2 end) 
			: base(start, end) {	}

		public bool Intersects(BasicTile tile)
		{
			Rectangle rect = new Rectangle(tile.Position.X, tile.Position.Y, BasicTile.Size, BasicTile.Size);
			foreach (var point in Points)
			{
				if (point.Intersects(rect))
					return true;
			}
			return false;
		}

		public void CutEnd(int end) => Points = Points.Take(end).ToArray();
		public void CutStart(int start) => Points = Points.Skip(start).ToArray();
	}
}
