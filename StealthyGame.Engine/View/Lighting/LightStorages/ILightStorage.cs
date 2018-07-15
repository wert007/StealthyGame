using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public interface ILightStorage
	{
		Index2 Position { get; }
		int Width { get; }
		int Height { get; }
		HSVColor GetAt(int x, int y);
		void SetAt(int x, int y, HSVColor color);
	}
}
