using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StealthyGame.Engine.UI.DataTypes
{
	public static class FontManager
	{
		static Dictionary<Font, SpriteFont> loadedFonts;
		static ContentManager contentManager;

		public static void Initialize(ContentManager content)
		{
			loadedFonts = new Dictionary<Font, SpriteFont>();
			contentManager = content;
		}

		public static SpriteFont Load(Font font)
		{
			SpriteFont result;
			if(!loadedFonts.TryGetValue(font, out result))
			{
				result = contentManager.Load<SpriteFont>(Path.Combine("fonts", font.ToString().ToLower()));
				loadedFonts.Add(font, result);
			}
			return result;
		}
	}
}