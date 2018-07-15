using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.DataTypes
{
	public class Animation2D
	{
		public Texture2D Texture { get; private set; }
		public int Length { get; private set; }
		public bool IsLooped { get; private set; }
		public string Name { get; private set; }
		Rectangle[] sources;
		public float Duration { get; private set; }
		int index;
		float screenTime;
		public delegate void FrameChangedHandler(int index);
		public event FrameChangedHandler FrameChanged;

		private Animation2D(Texture2D texture, int length)
		{
			Texture = texture;
			this.Length = length;
			sources = new Rectangle[length];
			IsLooped = false;
			Name = "";
			Duration = 0;
			index = 0;
			screenTime = 0;
		}

		public Animation2D(Texture2D[] textures, bool looped, string name, float duration)
		{
			this.IsLooped = looped;
			this.Name = name;
			this.Duration = duration;
			Length = textures.Length;
			int x = 0;
			int y = 0;
			Color[] data;
			sources = new Rectangle[textures.Length];
			Texture = new Texture2D(textures[0].GraphicsDevice, textures.Sum(t => t.Width), textures.Max(t => t.Height));
			for (int i = 0; i < textures.Length; i++)
			{
				data = new Color[textures[i].Width * textures[i].Height];
				textures[i].GetData(data);
				sources[i] = new Rectangle(x,y, textures[i].Width, textures[i].Height);
				Texture.SetData(0, sources[i], data, 0, textures[i].Width * textures[i].Height);
				x += textures[i].Width;
			}
			index = 0;
			screenTime = 0;
		}

		public Animation2D(Texture2D[] textures, Animation2D parent) :
			this(textures, parent.IsLooped, parent.Name, parent.Duration)
		{
			parent.FrameChanged += (i) =>
			{
				index = i;
			};
		}

		public void Update(GameTime time)
		{
			screenTime += (float)time.ElapsedGameTime.TotalSeconds;
			if(screenTime * 1000 > Duration)
			{
				index++;
				screenTime = 0;
				if(index >= Length)
				{
					if(IsLooped)
					{
						index %= Length;
					}
					else
					{
						index = Length - 1;
					}
				}
				FrameChanged?.Invoke(index);
			}
		}

		public void Reset()
		{
			screenTime = 0;
			index = 0;
			FrameChanged?.Invoke(index);
		}

		public Rectangle GetFrame(int i = -1)
		{
			if (i == -1)
				i = index;
			return sources[i];
		}

		public Texture2D GetCurrentTexture(int i = -1)
		{
			Rectangle frame = GetFrame(i);
			Color[] data = new Color[frame.Width * frame.Height];
			Texture.GetData(0, frame, data, 0, frame.Width * frame.Height);
			Texture2D result = new Texture2D(Texture.GraphicsDevice, frame.Width, frame.Height);
			result.SetData(data);
			return result;
		}

		public static Animation2D Load(XmlReader xr, Texture2D texture, int tilesize, int spacing)
		{
			xr.Read();
			int length = int.Parse(xr["length"]);
			Animation2D result = new Animation2D(texture, length);
			result.Name = xr["name"];
			result.Duration = float.Parse(xr["duration"]);
			result.IsLooped = bool.Parse(xr["looped"]);
			int j = int.Parse(xr["index"]);
			for (int i = 0; i < length; i++)
			{
				result.sources[i] = new Rectangle(i * (tilesize + spacing), j * (tilesize + spacing), tilesize, tilesize);
			}

			return result;
		}
	}
}
