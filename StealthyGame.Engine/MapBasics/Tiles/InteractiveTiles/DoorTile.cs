using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;

namespace StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles
{
	public class DoorTile : InteractiveTile
	{
		protected float timeStayOpen;
		protected float currentDuration;

		public DoorTile(string name, Index2 index) : base(name, index)
		{
			durationOfInteraction = 0.3f;
			timeStayOpen = 1f;
			currentDuration = 0;
		}

		protected override void InnerInteract(object sender)
		{
			PlayAnimation("open");
		}

		protected override void InnerUpdate(GameTime time)
		{
			if(InteractionDone && CurrentAnimation.Name == "open")
			{
				currentDuration = timeStayOpen;
				PlayAnimation("idleopen");
			}
			if(currentDuration > 0)
			{
				currentDuration -= (float)time.ElapsedGameTime.TotalSeconds;
			}
			else if(CurrentAnimation.Name == "idleopen")
			{
				PlayAnimation("close");
			}
		}
	}
}
