using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI.DataTypes;

namespace StealthyGame.Engine.UI.Panels
{
	public class StackPanel<T> : Control where T : StackPanelItem
	{
		List<T> children;
		Orientation orientation;
		int position;

		public StackPanel(Control parent, Orientation orientation) : base(parent)
		{
			this.orientation = orientation;
			children = new List<T>();
		}

		public void AddChild(T child)
		{
			if (orientation == Orientation.Horizontal)
			{
				child.RelativeX = position;
				position += child.Width;
			}
			else if (orientation == Orientation.Vertical)
			{
				child.RelativeY = position;
				position += child.Height;
			}
			else throw new NotImplementedException();
			children.Add(child);
		}

		protected override void _Draw(SpriteBatch batch)
		{
			foreach (var child in children)
			{
				child.Draw(batch);
			}
		}

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
			for (int i = 0; i < children.Count; i++)
			{
				children[i].Update(time, keyboardManager);
			}
		}
	}
}
