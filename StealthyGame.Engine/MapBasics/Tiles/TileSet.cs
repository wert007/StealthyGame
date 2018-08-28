using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.MapBasics.Tiled;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.MapBasics.Tiles
{
	public class TileSet
	{
		public string Name { get; private set; }
		public string ImagePath { get; private set; }
		public string SourcePath { get; private set; }
		public int TileCount { get; private set; }
		int columns;
		public int TileSize { get; private set; } = 32;
		TiledProperties[] properties;

		public Texture2D Texture { get; private set; }
		

		public static TileSet Load(string file, GraphicsDevice graphicsDevice)
		{
			TileSet result = new TileSet();
			result.properties = null;
			result.SourcePath = file;
			using (XmlReader xr = XmlReader.Create(file))
			{
				while(xr.Read())
				{
					if (xr.NodeType != XmlNodeType.Element)
						continue;
					if (xr.Name == "tileset")
					{
						result.Name = xr["name"];
						result.TileSize = int.Parse(xr["tilewidth"]); //should be same as tileheight
						result.TileCount = int.Parse(xr["tilecount"]);
						result.columns = int.Parse(xr["columns"]);
						result.properties = new TiledProperties[result.TileCount];
					}
					else if (xr.Name == "image")
					{
						result.ImagePath = Path.Combine(Path.GetDirectoryName(file), xr["source"]);
						using (FileStream fs = new FileStream(result.ImagePath, FileMode.Open))
							result.Texture = Texture2D.FromStream(graphicsDevice, fs);
					}
					else if (xr.Name == "tile")
					{
						int id = int.Parse(xr["id"]);
						while (xr.Name != "properties")
						{
							xr.Read();
						}
						using (StringReader sr = new StringReader(xr.ReadOuterXml()))
						using (XmlReader r = XmlReader.Create(sr))
						{
							//r.Read();
							result.properties[id] = TiledProperties.Load(r);
						}
					}
				}
			}
				return result;
		}

		public Rectangle GetSourceRectangle(int index)
		{
			int x = (index) % columns;
			int y = index / columns;
			return new Rectangle(TileSize * x, TileSize * y, TileSize, TileSize);
		}

		public bool IsInteractive(int index)
		{
			if (properties[index] == null)
				return false;
			return properties[index].GetPropertyAsBool("Interactive");
		}

		public bool IsAnimation(int index)
		{
			if (properties[index] == null)
				return false;
			return properties[index].GetPropertyAsBool("Animation");
		}

		public TiledProperties GetProperties(int index)
		{
			return properties[index];
		}
	}
}
