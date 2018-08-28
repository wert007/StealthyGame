﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

		private int? maxHeight;
		private int? maxWidth;
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

		public delegate void OnClickHandler(MouseState mouseState);
		public event OnClickHandler Clicked;

		public abstract void Draw(SpriteBatch batch);
		protected abstract void _Update(GameTime time);

		public Control(Control parent)
		{
			RelativeX = 0;
			RelativeY = 0;
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
			Parent = parent;
		}

		public void Update(GameTime time)
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
			_Update(time);
		}

		private bool IsMouseOver(MouseState mouseState)
		{
			var pos = mouseState.Position.ToVector2();
			return pos.X >= AbsoluteX && pos.X <= AbsoluteX + Width
				&& pos.Y >= AbsoluteY && pos.Y <= AbsoluteY + Height;
		}

		protected virtual void OnClick(MouseState mouseState)
		{ }

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
					return MaxWidth;
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
					return MaxHeight;
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
						return Parent.GetAbsoluteX();
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
						return 0;
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
						return Parent.GetAbsoluteY();
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
						return 0;
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
				return RenderHeight;
			return Parent.MaxHeight;
		}

		private int GetMaxWidth()
		{
			if (maxWidth.HasValue)
				return maxWidth.Value;
			if (Parent == null)
				return RenderWidth;
			return Parent.MaxWidth;
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