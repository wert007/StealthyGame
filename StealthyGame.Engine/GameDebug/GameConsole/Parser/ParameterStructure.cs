using System;
using System.Collections.Generic;
using System.Linq;

namespace StealthyGame.Engine.GameDebug.GameConsole.Parser
{
	public class ParameterStructure
	{
		public CommandStructure Parent { get; private set; }
		public string[] Names { get; private set; }
		public bool HasShort { get; private set; }
		public bool IsExclusive { get; private set; }
		public CommandFileLine Flags { get; private set; }
		public string[] Others { get; private set; }

		public ParameterStructure(CommandStructure parent, string[] names)
		{
			Parent = parent;
			Names = names;
			HasShort = false;
			Others = null;
			IsExclusive = false;
		}

		public void FlagsPosition(CommandFileLine position)
		{
			Flags = position;
		}

		public void SetShort()
		{
			if (Flags == null)
				throw new Exception();
			HasShort = true;
		}

		public void SetExclusive()
		{
			if (Flags == null)
				throw new Exception();
			IsExclusive = true;
			this.Others = null;
		}

		public void SetOther(string[] others)
		{
			if (IsExclusive)
				throw new Exception();
			this.Others = others;
		}



		public bool Allows(ParameterStructure parameter)
		{
			if (IsExclusive) return false;
			foreach (var name in parameter.Names)
			{
				if (Others.Contains(name))
					return true;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			var structure = obj as ParameterStructure;
			return structure != null &&
					 EqualityComparer<CommandStructure>.Default.Equals(Parent, structure.Parent) &&
					 EqualityComparer<string[]>.Default.Equals(Names, structure.Names) &&
					 HasShort == structure.HasShort &&
					 IsExclusive == structure.IsExclusive &&
					 EqualityComparer<string[]>.Default.Equals(Others, structure.Others);
		}

		public override int GetHashCode()
		{
			var hashCode = 1745518476;
			hashCode = hashCode * -1521134295 + EqualityComparer<CommandStructure>.Default.GetHashCode(Parent);
			hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Names);
			hashCode = hashCode * -1521134295 + HasShort.GetHashCode();
			hashCode = hashCode * -1521134295 + IsExclusive.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Others);
			return hashCode;
		}

		public static bool operator ==(ParameterStructure a, ParameterStructure b)
		{
			bool result = true;
			result &= a.IsExclusive == b.IsExclusive;
			result &= a.HasShort == b.HasShort;
			foreach (var name in a.Names)
			{
				result &= b.Names.Contains(name);
			}
			if (!(a.IsExclusive || b.IsExclusive
				|| a.Others == null || b.Others == null)) //So.. must be an exclusive flag be set if there aren't any other commands?
				foreach (var other in a.Others)
				{
					result &= b.Others.Contains(other);
				}
			return result;
		}

		public static bool operator != (ParameterStructure a, ParameterStructure b)
		{
			return !(a == b);
		}
	}
}
