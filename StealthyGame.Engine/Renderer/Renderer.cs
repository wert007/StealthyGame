using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.UI.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Renderer
{
	public class Renderer2D
	{
		SpriteBatch batch; //hopefully tmp
		public static Texture2D Pixel { get; set; }
		public GraphicsDevice GraphicsDevice => batch.GraphicsDevice;

		public Renderer2D(SpriteBatch batch)
		{
			this.batch = batch;
			Pixel = new Texture2D(batch.GraphicsDevice, 1, 1);
			Pixel.SetData(new Color[] { Color.White });
		}

		#region Using of SpriteBatch (tmp)
		public void Draw(Texture2D texture, Vector2 position, Color color) => batch.Draw(texture, position, color);
		public void Draw(Texture2D texture, Rectangle destination, Color color) => batch.Draw(texture, destination, color);
		public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Color color) => batch.Draw(texture, position, source, color);
		public void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Color color) => batch.Draw(texture, destination, source, color);

		public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float depth)
			=> batch.Draw(texture, position, source, color, rotation, origin, scale, spriteEffects, depth);

		public void Begin() => batch.Begin();
		public void Begin(Matrix matrix) => batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);
		public void Begin(BlendState blendState, Matrix matrix) => batch.Begin(SpriteSortMode.Deferred, blendState, null, null, null, null, matrix);
		public void End() => batch.End();

		public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float depth)
			=> batch.Draw(texture, position, source, color, rotation, origin, scale, spriteEffects, depth);
		public void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float depth)
			=> batch.Draw(texture, destination, source, color, rotation, origin, spriteEffects, depth);

		public void Draw(SpriteFont font, string text, Vector2 position, Color color) => batch.DrawString(font, text, position, color);
		#endregion

		public void DrawVector(Vector2 vector, Vector2 position, Color color, int thickness = 3)
		{
			Rectangle r = new Rectangle((int)position.X, (int)position.Y, (int)vector.Length(), thickness);
			float rotation = (float)Math.Atan2(vector.Y, vector.X);
			Draw(Pixel, r, null, color, rotation, new Vector2(0, 1), SpriteEffects.None, 1);
		}
		public void Draw(Font font, string str, Vector2 position, Color color)
		{
			batch.DrawString(font.SpriteFont, str, position, color);
		}

		public void DrawPixel(int x, int y, Color c)
			=> Draw(Pixel, new Vector2(x, y), c);
		public void DrawPixel(Index2 index, Color c)
		{
			Draw(Pixel, index, c);
		}
		
		public void DrawRectangle(Rectangle rect, Color color, int thickness = 2)
		{
			Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width + thickness, thickness), color);
			Draw(Pixel, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height + thickness), color);
			Draw(Pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width + thickness, thickness), color);
			Draw(Pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height + thickness), color);
		}
		
		public void DrawFilledRectangle(Rectangle rect, Color color)
			=> Draw(Pixel, rect, color);
		public void DrawFilledRectangle(Rectangle rect, Color inner, Color outer, int thickness = 2)
		{
			Draw(Pixel, rect, inner);
			Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width + thickness, thickness), outer);
			Draw(Pixel, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height + thickness), outer);
			Draw(Pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width + thickness, thickness), outer);
			Draw(Pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height + thickness), outer);
		}

		public void DrawCircle(Circle circle, Color color, int thickness = 2)
		{
			foreach (var point in circle.CalculateEdge())
			{
				Draw(Pixel, new Rectangle(point.X - thickness / 2, point.Y - thickness / 2, thickness, thickness), color);
			}
		}

		public void DrawFilledPolygon(Polynom polygon, Color color)
		{
			foreach (var point in polygon.FloodFill())
			{
				Draw(Pixel, point, color);
			}
		}
		
		public void Draw(Animation2D animation, Rectangle destinationRectangle, Color color)
			=> Draw(animation.Texture, destinationRectangle, animation.GetFrame(), color);
		public void Draw(Animation2D animation, Vector2 position, Color color)
			=> Draw(animation.Texture, position, animation.GetFrame(), color);
		public void Draw(Animation2D animation, Rectangle destinationRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
			=> Draw(animation.Texture, destinationRectangle, animation.GetFrame(), color, rotation, origin, effects, layerDepth);
		public void Draw(Animation2D animation, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
			=> Draw(animation.Texture, position, animation.GetFrame(), color, rotation, origin, scale, effects, layerDepth);


		public void Draw(Animation2D animation, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
			=> Draw(animation.Texture, position, animation.GetFrame(), color, rotation, origin, scale, effects, layerDepth);

		public void Draw(AnimationCollection animationCollection, Rectangle destinationRectangle, Color color)
		{
			if (animationCollection.Length <= 0)
				return;
			Draw(animationCollection.GetCurrent(), destinationRectangle, color);
		}
		public void Draw(AnimationCollection animationCollection, Vector2 position, Color color)
		{
			if (animationCollection.Length <= 0)
				return;
			Draw(animationCollection.GetCurrent(), position, color);
		}
		public void Draw(AnimationCollection animationCollection, Rectangle destinationRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			Draw(animationCollection.GetCurrent(), destinationRectangle, color, rotation, origin, effects, layerDepth);
		}
		public void Draw(AnimationCollection animationCollection, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			Draw(animationCollection.GetCurrent(), position, color, rotation, origin, scale, effects, layerDepth);
		}
		public void Draw(AnimationCollection animationCollection, Vector2 position, HSVColor color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
			=> Draw(animationCollection, position, color.RGB, rotation, origin, scale, effects, layerDepth);
		public void Draw(AnimationCollection animationCollection, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		{
			if (animationCollection.Length <= 0)
				return;
			Draw(animationCollection.GetCurrent(), position, color, rotation, origin, scale, effects, layerDepth);
		}
	}
}
