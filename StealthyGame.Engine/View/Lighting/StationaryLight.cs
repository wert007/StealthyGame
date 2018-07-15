using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.View.Lighting
{
	class StationaryLight : Light
	{
		public StationaryLight(Index2 position, int strength, float brightness, HSVColor color) : base(position, strength, brightness, color)
		{
		}
	}
}
