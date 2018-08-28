using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.UI.DataTypes
{
	public class Font
	{
		public string Name { get; }
		public int Size { get; }
		public SpriteFont SpriteFont { get; }
		public static Font Arial16 { get { return new Font("Arial", 16); } }
		public static Font Arial11 { get { return new Font("Arial", 11); } }

		public Font(string name, int size)
		{
			Name = name;
			Size = size;
			SpriteFont = FontManager.Load(this);
		}

		public override string ToString()
		{
			return Name + Size;
		}

		public override bool Equals(object obj)
		{
			if(obj is Font font)
			{
				return Name.Equals(font.Name) &&
					Size.Equals(font.Size);
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var hashCode = -966199182;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + Size.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Font font1, Font font2)
		{
			return EqualityComparer<Font>.Default.Equals(font1, font2);
		}

		public static bool operator !=(Font font1, Font font2)
		{
			return !(font1 == font2);
		}
	}
}
