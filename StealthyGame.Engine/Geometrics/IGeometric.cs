using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{
	interface ILine
	{
		IEnumerable<Index2> All { get; }
		Index2 this[int index] { get; }
		Index2 Min { get; }
		Index2 Max { get; }

		int LengthSquared();
		float Length();
		bool Contains(Index2 point);
		int IndexOf(Index2 point);
		Index2 First();
		Index2 Last();
	}

}
