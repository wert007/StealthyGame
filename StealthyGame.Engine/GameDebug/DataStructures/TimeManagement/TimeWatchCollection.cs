using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures.TimeManagement
{
	public class TimeWatchCollection : IEnumerable
	{
		List<TimeWatchEntry> entries;

		public TimeWatchCollection()
		{
			entries = new List<TimeWatchEntry>();
		}

		public IEnumerator GetEnumerator()
		{
			return entries.GetEnumerator();
		}

		internal void Clear()
		{
			entries.Clear();
		}

		internal void Add(TimeWatchEntry timeWatchEntry)
		{
			entries.Add(timeWatchEntry);
		}

		internal TimeWatchEntry LastRunning()
		{
			return entries.LastOrDefault(e => e.IsRunning);
		}
	}
}
