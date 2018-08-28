using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
using TestiTest.GameObjects.NPCs;
using StealthyGame.Engine.MapBasics;

namespace TestiTest.MapBasics.Tiles.InteractiveTiles
{
	class CellDoor : DoorTile
	{
		private enum State
		{
			OpenRequested,
			Opening,
			Open,
			Closing,
			Closed
		}

		State state;
		RightfulNPC requester;

		public CellDoor(string name, Index2 index) : base(name, index)
		{
			durationOfInteraction = 0.6f;
			timeStayOpen = 0.5f;
			state = State.Closed;
		}

		protected override void InnerInteract(object sender)
		{
			//if (sender is RightfulNPC)
			{
				//requester = sender as RightfulNPC;
				state = State.OpenRequested;
			}
			//else
			//	throw new NotSupportedException("CellDoors can only be opened by RightfulNPC! (Got " + sender.GetType().Name + ")");
		}

		protected override void InnerUpdate(GameTime time)
		{
			if(currentDuration > 0)
			{
				currentDuration -= (float)time.ElapsedGameTime.TotalSeconds;
			}
			else
			switch (state)
			{
				case State.OpenRequested:
						currentDuration = durationOfInteraction * 0.5f;
						state = State.Opening;
					break;
				case State.Opening:
						currentDuration = durationOfInteraction * 0.5f;
						PlayAnimation("open");
						state = State.Open;
						break;
				case State.Open:
						currentDuration = timeStayOpen;
						PlayAnimation("idleopen");
						state = State.Closing;
					break;
				case State.Closing:
						currentDuration = durationOfInteraction * 0.5f;
						PlayAnimation("close");
						state = State.Closed;
					break;
				case State.Closed:
						PlayAnimation("idle");
					break;
				default:
					throw new NotSupportedException("Unknown state " + state.ToString());
			}
		}
	}


}
