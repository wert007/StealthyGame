using StealthyGame.Engine.OpenTKVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.OpenTK
{
	class Program
	{
		static void Main(string[] args)
		{
			
			using (Game game = new MyGame())
			{
				game.Run(30, 30);
			}
		}
	}
}
