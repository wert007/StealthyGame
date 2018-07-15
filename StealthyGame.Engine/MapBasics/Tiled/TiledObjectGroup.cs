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
	public class TiledObjectGroup
	{
		public string Name { get; private set; }
		public TiledMapObject[] Objects { get; private set; }
		public TiledProperties Properties { get; private set; }

		public TiledObjectGroup(string name)
		{
			this.Name = name;
			Objects = new TiledMapObject[0];
		}

		internal void Load(XmlReader xr)
		{
			List<TiledMapObject> objects = new List<TiledMapObject>();
			while(!(xr.NodeType == XmlNodeType.EndElement && xr.Name == "objectgroup") && xr.Read())
			{
				if(xr.Name == "properties")
					using (XmlReader r = XmlReader.Create(new StringReader(xr.ReadOuterXml())))
						Properties = TiledProperties.Load(r);
				if(xr.Name == "object" && xr.NodeType != XmlNodeType.EndElement)
				{
					using (XmlReader r = XmlReader.Create(new StringReader(xr.ReadOuterXml())))
						objects.Add(TiledMapObject.FromXML(r));
				}
			}
			Objects = objects.ToArray();
		}
	}
}
