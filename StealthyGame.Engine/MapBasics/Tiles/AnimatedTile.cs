using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.MapBasics.Tiles
{
	public class AnimatedTile : BasicTile
	{
		public AnimatedTile(string name, Index3 index, float drawOrder) : base(name, index, drawOrder)
		{
		}
	}
}
