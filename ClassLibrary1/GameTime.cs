using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class GameTime
	{
		double totalTime;
		double elapsedTime;

		public GameTime()
		{
			totalTime = 0;
			elapsedTime = 0;
		}

		internal void AddSeconds(double time)
		{
			totalTime += time;
			elapsedTime = time;
		}
	}
}
