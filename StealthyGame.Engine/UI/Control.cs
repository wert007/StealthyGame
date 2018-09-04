using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StealthyGame.Engine.Input;
using StealthyGame.Engine.UI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.UI
{
	public abstract class Control
	{
		public static int RenderWidth { get; private set; }
		public static int RenderHeight { get; private set; }
		public static List<Control> allControls;

		//TODO: Differentiate more between Margin and RelativeX/RelativeY
		//			When is used what? And why? Make it more obvious to the user.
		private int? maxHeight;
		private int? maxWidth;
		public Thickness Margin { get; set; }
		public int MaxWidth { get { return GetMaxWidth(); } }
		public int MinWidth { get; set; }
		public int MaxHeight { get { return GetMaxHeight(); } }
		public int MinHeight { get; set; }
		public int RelativeX { get; set; }
		public int RelativeY { get; set; }
		public int AbsoluteX { get { return GetAbsoluteX(); } }
		public int AbsoluteY { get { return GetAbsoluteY(); } }
		public int Width { get { return GetWidth(); } }
		public int Height { get { return GetHeight(); } }
		public Control Parent { get; private set; }
		public HorizontalAlignment HorizontalAlignment { get; set; }
		public VerticalAlignment VerticalAlignment { get; set; }
		public bool Hovered { get; private set; }
		public bool IsClickable { get; set; }
		public bool Focused { get; private set; }
		public bool Visible { get; set; }

		public delegate void OnClickHandler(MouseState mouseState);
		public event OnClickHandler Clicked;

		protected abstract void _Draw(SpriteBatch batch);
		protected abstract void _Update(GameTime time, KeyboardManager keyboardManager);

		public Control(Control parent)
		{
			RelativeX = 0;
			RelativeY = 0;
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
			Parent = parent;
			Visible = true;
			Margin = new Thickness();


			if (allControls == null)
				allControls = new List<Control>();
			allControls.Add(this);
		}

		public void Update(GameTime time, KeyboardManager keyboardManager)
		{
			var mouseState = Mouse.GetState();
			Hovered = IsMouseOver(mouseState);
			if (Hovered && IsClickable)  
			{
				//TODO: Currently not Working, but already fixed in future versions of MonoGame
				//see https://github.com/MonoGame/MonoGame/issues/6162
				Mouse.SetCursor(MouseCursor.Hand);


				if (mouseState.LeftButton == ButtonState.Pressed)
				{
					OnClick(mouseState);
					Clicked?.Invoke(mouseState);
				}
			}
			if(keyboardManager.IsKeyPressed(Keys.Enter))
				_TextInput(keyboardManager, new TextInputEventArgs((char)13, Keys.Enter));

			_Update(time, keyboardManager);
		}

		public void Draw(SpriteBatch batch)
		{
			if (!Visible)
				return;
			_Draw(batch);
		}

		private bool IsMouseOver(MouseState mouseState)
		{
			var pos = mouseState.Position.ToVector2();
			return pos.X >= AbsoluteX && pos.X <= AbsoluteX + Width
				&& pos.Y >= AbsoluteY && pos.Y <= AbsoluteY + Height;
		}

		protected virtual void OnClick(MouseState mouseState)
		{ }

		public static void TextInput(object sender, TextInputEventArgs e)
		{
			foreach (var control in allControls.Where(c=> c.Focused))
			{
				control._TextInput(sender, e);
			}
		}

		protected virtual void _TextInput(object sender, TextInputEventArgs e)
		{

		}

		public virtual void Focus()
		{
			Focused = true;
		}

		public virtual void Unfocus()
		{
			Focused = false;
		}

		public static void Initialize(int width, int height)
		{
			RenderWidth = width;
			RenderHeight = height;
		}

		#region GetterFunctions
		private int GetWidth()
		{
			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
				case HorizontalAlignment.Center:
				case HorizontalAlignment.Right:
					return MinWidth;
				case HorizontalAlignment.Stretch:
					return MaxWidth - Margin.HorizontalSum(); ;
				default:
					throw new NotImplementedException();
			}
		}

		private int GetHeight()
		{
			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
				case VerticalAlignment.Center:
				case VerticalAlignment.Bottom:
					return MinHeight;
				case VerticalAlignment.Stretch:
					return MaxHeight - Margin.VertivalSum();
				default:
					throw new NotImplementedException();
			}
		}

		private int GetAbsoluteX()
		{
			if (Parent != null)
			{
				switch (HorizontalAlignment)
				{
					case HorizontalAlignment.Left:
						return RelativeX + Parent.GetAbsoluteX();
					case HorizontalAlignment.Center:
						return (int)(Parent.GetAbsoluteX() + 0.5f * Parent.Width - 0.5f * Width);
					case HorizontalAlignment.Right:
						return Parent.GetAbsoluteX() + Parent.Width - Width - RelativeX;
					case HorizontalAlignment.Stretch:
						return Parent.GetAbsoluteX() + Margin.Left;
					default:
						throw new NotImplementedException();
				}
			}
			else
			{
				switch (HorizontalAlignment)
				{
					case HorizontalAlignment.Left:
						return RelativeX;
					case HorizontalAlignment.Center:
						return (int)(0.5f * RenderWidth - 0.5f * Width);
					case HorizontalAlignment.Right:
						return RenderWidth - Width - RelativeX;
					case HorizontalAlignment.Stretch:
						return Margin.Left;
					default:
						throw new NotImplementedException();
				}
			}
		}

		private int GetAbsoluteY()
		{
			if (Parent != null)
			{
				switch (VerticalAlignment)
				{
					case VerticalAlignment.Top:
						return RelativeY + Parent.GetAbsoluteY();
					case VerticalAlignment.Center:
						return (int)(Parent.GetAbsoluteY() + 0.5f * Parent.Height - 0.5f * Height);
					case VerticalAlignment.Bottom:
						return Parent.GetAbsoluteY() + Parent.Height - Height - RelativeY;
					case VerticalAlignment.Stretch:
						return Parent.GetAbsoluteY() + Margin.Top;
					default:
						throw new NotImplementedException();
				}
			}
			else
			{
				switch (VerticalAlignment)
				{
					case VerticalAlignment.Top:
						return RelativeY;
					case VerticalAlignment.Center:
						return (int)(0.5f * RenderHeight - 0.5f * Height);
					case VerticalAlignment.Bottom:
						return RenderHeight - Height - RelativeY;
					case VerticalAlignment.Stretch:
						return Margin.Top;
					default:
						throw new NotImplementedException();
				}
			}
		}

		private int GetMaxHeight()
		{
			if (maxHeight.HasValue)
				return maxHeight.Value;
			if (Parent == null)
				return RenderHeight - Margin.VertivalSum();
			return Parent.MaxHeight - Margin.VertivalSum();
		}

		private int GetMaxWidth()
		{
			if (maxWidth.HasValue)
				return maxWidth.Value;
			if (Parent == null)
				return RenderWidth - Margin.HorizontalSum();
			return Parent.MaxWidth- Margin.HorizontalSum();
		}

		protected void SetMaxHeight(params Control[] controls)
		{
			maxHeight = controls.Min(c => c.MaxHeight);
		}

		protected void SetMaxWidth(params Control[] controls)
		{
			maxWidth = controls.Min(c => c.MaxWidth);
		}
		#endregion
	}
}