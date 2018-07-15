using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public class AngleCollection
	{
		List<AnglePair> pairs;
		public AnglePair[] All => pairs.ToArray();

		public AngleCollection()
		{
			pairs = new List<AnglePair>();
		}

		public void Add(AnglePair add)
		{
			for(int i = 0; i < pairs.Count; i++)
			{
				if(pairs[i].IsInRange(add.Minor))
				{
					if (!pairs[i].IsInRange(add.Major))
					{
						pairs[i] = new AnglePair(pairs[i].Minor, add.Major);
					}
					return;
				}
				else if (pairs[i].IsInRange(add.Major))
				{
					if(!pairs[i].IsInRange(add.Minor)) //Should be always true
					{
						pairs[i] = new AnglePair(add.Minor, pairs[i].Major);
					}
					return;
				}
			}
			pairs.Add(add);
		}

		public void Shorten()
		{
			for (int i = pairs.Count - 1; i >= 0; i--)
				for (int j = pairs.Count - 1; j >= 0 && i >= 0; j--)
				{
					if (pairs[i].IsInRange(pairs[j].Minor))
					{
						if (!pairs[i].IsInRange(pairs[j].Major))
						{
							pairs[i] = new AnglePair(pairs[i].Minor, pairs[j].Major);
							pairs.RemoveAt(j);
							i--;
							j--;
						}
					}
					else if (pairs[i].IsInRange(pairs[j].Major))
						if (!pairs[i].IsInRange(pairs[j].Minor)) //Should be always true
						{
							pairs[i] = new AnglePair(pairs[j].Minor, pairs[i].Major);
							pairs.RemoveAt(j);
							i--;
							j--;
						}
				}
		}

		public bool ContainsAny(Angle angle)
			=> pairs.Any(p => p.IsInRange(angle));

		public bool ContainsAny(Angle angle, out AnglePair pair)
		{
			pair = pairs.Where(p => p.IsInRange(angle)).FirstOrDefault();
			return pairs.Any(p => p.IsInRange(angle));
		}
	}
}
