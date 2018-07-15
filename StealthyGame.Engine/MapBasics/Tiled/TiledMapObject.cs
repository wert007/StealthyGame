using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.MapBasics.Tiled
{
	public class TiledMapObject
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Type { get; private set; }
		public bool Visible { get; private set; }
		public Vector2 Position { get; private set; }
		public Vector2 Size { get; private set; }
		public float Rotation { get; private set; }
		public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }
		public TiledProperties Properties { get; private set; }

		public TiledMapObject(int id, Vector2 pos, string name = "", string type = "", bool visible = true, Vector2 size = new Vector2(), float rotation = 0)
		{
			this.Id = id;
			this.Position = pos;
			this.Name = name;
			this.Type = type;
			this.Visible = visible;
			this.Size = size;
			this.Rotation = rotation;
		}

		public static TiledMapObject FromXML(XmlReader xr)
		{
			xr.Read();
			int id = int.Parse(xr["id"]);
			float x = float.Parse(xr["x"]);
			float y = float.Parse(xr["y"]);
			string type = xr["type"];
			string name = xr["name"];
			bool visible = true;
			float w = 0;
			float h = 0;
			float rotation = 0;
			if (!string.IsNullOrWhiteSpace(xr["width"]))
			{
				w = float.Parse(xr["width"]);
			}
			if (!string.IsNullOrWhiteSpace(xr["height"]))
			{
				h = float.Parse(xr["height"]);
			}
			if (!string.IsNullOrWhiteSpace(xr["visible"]))
			{
				visible = bool.Parse(xr["visible"]);
			}
			if (!string.IsNullOrWhiteSpace(xr["rotation"]))
			{
				rotation = float.Parse(xr["rotation"]);
			}
			//Normal result without userdefined properties
			TiledMapObject result = new TiledMapObject(id, new Vector2(x, y), name, type, visible, new Vector2(w, h), rotation);
			while(xr.Read()) //Read Xml to Properties or the End
			{
				if (xr.Name == "properties" && xr.NodeType == XmlNodeType.Element)//Properties
				{
					using(StringReader sr = new StringReader(xr.ReadOuterXml()))
					using(XmlReader r = XmlReader.Create(sr))
						result.Properties = TiledProperties.Load(r);
				}
				if (xr.Name == "object" && xr.NodeType == XmlNodeType.EndElement) //The End
					break;
			}
			return result;
		}

	}
}
