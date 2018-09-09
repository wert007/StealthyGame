using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Renderer;

namespace StealthyGame.Engine.GameMechanics.Phases
{
	public abstract class Phase
	{
		public PhaseType PhaseType { get; private set; }
		public Type Next { get; private set; }
		public bool RequestNextPhase { get; protected set; }

		public Phase(PhaseType type, Type next)
		{
			PhaseType = type;
			Next = next;
		}

		public abstract void Load(ContentManager content);

		public abstract void Update(UpdateContainer container);

		public abstract void Draw(Renderer2D renderer, GameTime time);

		public Phase CreateNextPhase(PhaseContainer container)
		{
			Phase result = (Phase)Next.GetConstructor(new Type[] { typeof(PhaseContainer) }).Invoke(new object[] { container });
			return result;
		}
	}

	public enum PhaseType
	{
		InGame,
		PreGame,
		PostGame,
	}
}
