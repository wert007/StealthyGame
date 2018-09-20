using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures.TimeManagement
{
	public struct TimeWatchEntry
	{
		Stopwatch stopwatch;
		public string Name { get; private set; }
		public bool IsRunning => stopwatch.IsRunning;
		public TimeSpan Duration => stopwatch.Elapsed;

		public TimeWatchEntry(string name)
		{
			this.Name = name;
			stopwatch = Stopwatch.StartNew();
		}

		public TimeWatchEntry(Stopwatch stopwatch, string name)
		{
			this.stopwatch = stopwatch;
			this.Name = name;
		}

		public void Stop()
		{
			stopwatch.Stop();
		}

		public override string ToString()
		{
			return Name + (IsRunning ? "(r)" : string.Empty) + ": " + Duration;
		}
	}
}
