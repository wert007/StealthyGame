using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameDebug.UI;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI.Basics;
using StealthyGame.Engine.UI.DataTypes;
using StealthyGame.Engine.UI.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.UI.Engine
{
	public class ClassTreeControl : StackPanel<ClassTreeItemControl>
	{
		public ClassTreeControl(Control parent, ClassTree classTree, Texture2D arrow) : base(parent, Orientation.Vertical)
		{
			AddChild(new ClassTreeItemControl(this, classTree.GetRoot(), arrow));
		}
    }

	public class ClassTreeItemControl : StackPanelItem
	{
		Icon icon;
		Label name;
		ClassTreeItem source;

		public ClassTreeItemControl(Control parent, ClassTreeItem classTreeItem, Texture2D arrow) : base(parent)
		{
			source = classTreeItem;

			string strName = classTreeItem.ClassName;
			if (classTreeItem.HasName)
				strName += " " + classTreeItem.Name;

			icon = new Icon(this, arrow);
			icon.VerticalAlignment = VerticalAlignment.Center;
			icon.IsClickable = true;
			icon.Clicked += (m) =>
			{
				source.GenerateChildren();
				foreach (var child in source.Children)
				{
					((ClassTreeControl)Parent).AddChild(new ClassTreeItemControl(Parent, child, arrow));

				}
			};

			name = new Label(this, strName);
			name.VerticalAlignment = VerticalAlignment.Center;
			name.RelativeX = icon.Width;

			SetMaxHeight(icon, name);
			SetMaxWidth(icon, name);
			MinHeight = Math.Max(icon.MinHeight, name.MinHeight);
			MinWidth = Math.Max(icon.MinWidth, name.MinWidth);
			VerticalAlignment = VerticalAlignment.Top;
		}

		protected override void _Draw(SpriteBatch batch)
		{
			name.Draw(batch);
			icon.Draw(batch);
		}

		protected override void _Update(GameTime time, KeyboardManager keyboardManager)
		{
			name.Update(time, keyboardManager);
			icon.Update(time, keyboardManager);
		}
	}
}
