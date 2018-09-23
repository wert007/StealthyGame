using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures.VariableSystem
{
	class CmdValue
	{
		public string Name { get; set; }
		public object Value { get; set; }

		public CmdValue(string name, object value)
		{
			this.Name = name;
			this.Value = value;
		}
	}
}
