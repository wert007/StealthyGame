using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Helper
{
	public static class DrawHelper
	{
		public static Texture2D Pixel { get; set; }

		

		public static void Draw(this Vector2 vector, Vector2 position, SpriteBatch batch, HSVColor color, int thickness = 3)
			=> vector.Draw(position, batch, color.RGB, thickness);
		public static void Draw(this Vector2 vector, Vector2 position, SpriteBatch batch, Color color, int thickness = 3)
			=> batch.DrawVector(vector, position, color, thickness);
		public static void DrawVector(this SpriteBatch batch, Vector2 vector, Vector2 position, HSVColor color, int thickness = 3)
			=> batch.DrawVector(vector, position, color.RGB, thickness);
		public static void DrawVector(this SpriteBatch batch, Vector2 vector, Vector2 position, Color color, int thickness = 3)
		{
			Rectangle r = new Rectangle((int)position.X, (int)position.Y, (int)vector.Length(), thickness);
			float rotation = (float)Math.Atan2(vector.Y, vector.X);
			batch.Draw(Pixel, r, null, color, rotation, new Vector2(0, 1), SpriteEffects.None, 1);
		}

		public static void DrawPixel(this SpriteBatch batch, int x, int y, Color c)
			=> batch.Draw(Pixel, new Vector2(x, y), c);
		public static void DrawPixel(this SpriteBatch batch, Index2 index, Color c)
		{
			batch.Draw(Pixel, index, c);
		}

		public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, HSVColor color, int thickness = 2)
			=> batch.DrawRectangle(rect, color.RGB, thickness);
		public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, Color color, int thickness = 2)
		{
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width + thickness, thickness), color);
			batch.Draw(Pixel, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height + thickness), color);
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width + thickness, thickness), color);
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height + thickness), color);
		}

		public static void DrawFilledRectangle(this SpriteBatch batch, Rectangle rect, HSVColor color)
			=> batch.DrawFilledRectangle(rect, color.RGB);
		public static void DrawFilledRectangle(this SpriteBatch batch, Rectangle rect, Color color)
			=> batch.Draw(Pixel, rect, color);
		public static void DrawFilledRectangle(this SpriteBatch batch, Rectangle rect, HSVColor inner, HSVColor outer, int thickness = 2)
			=> batch.DrawFilledRectangle(rect, inner.RGB, outer.RGB, thickness);
		public static void DrawFilledRectangle(this SpriteBatch batch, Rectangle rect, Color inner,  Color outer, int thickness = 2)
		{
			batch.Draw(Pixel, rect, inner);
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width + thickness, thickness), outer);
			batch.Draw(Pixel, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height + thickness), outer);
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width + thickness, thickness), outer);
			batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height + thickness), outer);
		}

		public static void DrawCircle(this SpriteBatch batch, Circle circle, Color color, int thickness = 2)
		{
			foreach (var point in circle.All)
			{
				batch.Draw(Pixel, new Rectangle(point.X - thickness / 2, point.Y - thickness / 2, thickness, thickness), color);
			}
		}

		public static void DrawFilledPolygon(this SpriteBatch batch, Polynom polygon, Color color)
		{
			foreach (var point in polygon.FloodFill())
			{
				batch.Draw(Pixel, point, color);
			}
		}

		public static void Draw(this SpriteBatch batch, Animation2D animation, Rectangle destinationRectangle, HSVColor color)
			=> batch.Draw(animation, destinationRectangle, color.RGB);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Rectangle destinationRectangle, Color color) 
			=> batch.Draw(animation.Texture, destinationRectangle, animation.GetFrame(), color);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, HSVColor color)
			=> batch.Draw(animation, position, color.RGB);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, Color color) 
			=> batch.Draw(animation.Texture, position, animation.GetFrame(), color);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Rectangle destinationRectangle, HSVColor color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation, destinationRectangle, color.RGB, rotation, origin, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Rectangle destinationRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation.Texture, destinationRectangle, animation.GetFrame(), color, rotation, origin, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, HSVColor color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation, position, color.RGB, rotation, origin, scale, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation.Texture, position, animation.GetFrame(), color, rotation, origin, scale, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, HSVColor color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation, position, color.RGB, rotation, origin, scale, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animation.Texture, position, animation.GetFrame(), color, rotation, origin, scale, effects, layerDepth);

		//public static void Draw(this SpriteBatch batch, Animation2D animation, Vector2? position = null, Rectangle? destinationRectangle = null,
		//	Color? color = null, float rotation = 0, Vector2? origin = null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
		//{
		//	batch.Draw(animation.Texture, position, destinationRectangle, animation.GetFrame(), origin, rotation, scale, color, effects, layerDepth);
		//}

		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Rectangle destinationRectangle, HSVColor color)
			=> batch.Draw(animationCollection, destinationRectangle, color.RGB);
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Rectangle destinationRectangle, Color color)
		{
			if (animationCollection.Length <= 0)
				return;
			batch.Draw(animationCollection.GetCurrent(), destinationRectangle, color);
		}
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, HSVColor color)
			=> batch.Draw(animationCollection, position, color.RGB);
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, Color color)
		{
			if (animationCollection.Length <= 0)
				return;
			batch.Draw(animationCollection.GetCurrent(), position, color);
		}
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Rectangle destinationRectangle, HSVColor color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animationCollection, destinationRectangle, color.RGB, rotation, origin, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Rectangle destinationRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			batch.Draw(animationCollection.GetCurrent(), destinationRectangle, color, rotation, origin, effects, layerDepth);
		}
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, HSVColor color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animationCollection, position, color.RGB, rotation, origin, scale, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			batch.Draw(animationCollection.GetCurrent(), position, color, rotation, origin, scale, effects, layerDepth);
		}
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, HSVColor color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
			=> batch.Draw(animationCollection, position, color.RGB, rotation, origin, scale, effects, layerDepth);
		public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			batch.Draw(animationCollection.GetCurrent(), position, color, rotation, origin, scale, effects, layerDepth);
		}
		//public static void Draw(this SpriteBatch batch, AnimationCollection animationCollection, Vector2? position = null, Rectangle? destinationRectangle = null,
		//	Color? color = null, float rotation = 0, Vector2? origin = null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
		//{
		//	if (animationCollection.Length <= 0)
		//		return;
		//	batch.Draw(animationCollection.GetCurrent(), position, destinationRectangle, color, rotation, origin, scale, effects, layerDepth);
		//}


	}
}