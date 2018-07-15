using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.DataTypes
{
	public static class AnimationFile
	{
		public static Animation2D[] Load(string file, GraphicsDevice graphicsDevice)
		{
			List<Animation2D> result = new List<Animation2D>();
			int tilesize = -1;
			int spacing = 0;
			Texture2D source = null;
			using (XmlReader xr = XmlReader.Create(file))
			{
				while(xr.Read())
				{
					if (xr.NodeType != XmlNodeType.Element)
						continue;
					switch (xr.Name)
					{
						case "animations":
							tilesize = int.Parse(xr["tilewidth"]); //Should be same as tileheight
							if(!string.IsNullOrWhiteSpace(xr["spacing"]))
							{
								spacing = int.Parse(xr["spacing"]);
							}
							string imagePath = Path.Combine(Path.GetDirectoryName(file), xr["source"]);
							using(FileStream fs = new FileStream(imagePath, FileMode.Open))
								source = Texture2D.FromStream(graphicsDevice, fs);
							break;
						case "animation":
							using (StringReader sr = new StringReader(xr.ReadOuterXml()))
							using (XmlReader r = XmlReader.Create(sr))
								result.Add(Animation2D.Load(r, source, tilesize, spacing));
							break;
						default:
							throw new NotSupportedException("Unknown Type " + xr.Name);

					}
				}
			}
				return result.ToArray();
		}
	}
}
