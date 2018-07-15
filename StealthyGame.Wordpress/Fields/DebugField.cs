using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Wordpress.Fields
{
	class DebugField : Field
	{
		public DebugField(string name) : base(name)
		{
		}

		protected override void ComputeSingleStep()
		{
			if (Current.X == -1) Current = new Index2();
			else if (Current.X < Size) Current = new Index2(Current.X + 1, Current.Y);
			else if (Current.Y < Size) Current = new Index2(0, Current.Y + 1);
			else
			{ Current = new Index2(-1, -1); Done = true; }
		}

		protected override void PreCompute()
		{
		}
	}
}
