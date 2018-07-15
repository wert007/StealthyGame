using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Wordpress.Fields
{
	class SpiralField : Field
	{
		AngleCollection obstructedArea;
		int i;
		Index2[] toCheck;

		public SpiralField(string name) : base(name)
		{
			obstructedArea = new AngleCollection();
			i = 0;
			toCheck = new Index2[0];
		}

		protected override void InnerReset()
		{
			obstructedArea = new AngleCollection();
			i = 0;
			toCheck = new Index2[0];
		}

		protected override void PreCompute()
		{
			obstructedArea = new AngleCollection();
			Current = lightPosition;
			toCheck = circle.SpiralArea(false);
			i = 0;
		}
		
		protected override void ComputeSingleStep()
		{
			if (i >= toCheck.Length)
			{
				Done = true;
				return;
			}
			Index2 tile = toCheck[i];
			i++;
			Angle angle = ToAngle(lightPosition - tile);
			if (obstacles[tile.X, tile.Y] > 0)
			{
				float increment = MathHelper.TwoPi / (8 * (tile - lightPosition).Length());
				obstructedArea.Add(new AnglePair(angle - 0.5f * increment, angle + 0.5f * increment));
				ComputeSingleStep();
				return;
			}
			if (obstructedArea.ContainsAny(angle))
			{
				ComputeSingleStep();
				return;
			}
			Current = tile;
		}

		private Angle ToAngle(Index2 tile)
		{
			return new Angle((float)Math.Atan2(tile.Y, tile.X));
		}
	}
}