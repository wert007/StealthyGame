using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.View.Lighting
{
	public class LightRaycast : Raycast
	{
		public Light Light { get; set; }

		public LightRaycast(Light light, Index2 start, Index2 end) : base(start, end)
		{
			Light = light;
		}


		public int Shrink(byte[,] obstacles)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].Y < 0 || this[i].Y >= obstacles.GetLength(1) ||
					this[i].X < 0 || this[i].X >= obstacles.GetLength(0) ||
					obstacles[this[i].X, this[i].Y] == 255)
				{
					int dif = Count - i;
					CutEnd(i);
					return dif;
				}
			}
			return 0;
		}

	}
}
