using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures.TimeManagement
{
	public static class TimeWatcher
	{
		public static TimeWatchCollection Current { get; private set; } = new TimeWatchCollection();
		//static TimeWatchCollection avg = new TimeWatchCollection();

		public static void ClearCurrent()
		{
			Current.Clear();
		}

		public static void Start(string name)
		{
			Current.Add(new TimeWatchEntry(name));
		}

		public static void End()
		{
			Current.LastRunning().Stop();
		}
	}

}
