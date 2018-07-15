using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.MapBasics.Tiled
{
	public class TiledProperties
	{
		Dictionary<string, string> properties;
		Dictionary<string, string> propertyTypes;

		public TiledProperties()
		{
			properties = new Dictionary<string, string>();
			propertyTypes = new Dictionary<string, string>();
		}

		public static TiledProperties Load(XmlReader xr)
		{
			xr.Read();
			TiledProperties result = new TiledProperties();
			if (xr.Name == "properties") //Has Properties
			{
				while (xr.Read() && !(xr.Name == "properties" && xr.NodeType == XmlNodeType.EndElement))
				{
					if (xr.Name == "property" && xr.NodeType == XmlNodeType.Element)
					{
						result.properties.Add(xr["name"], xr["value"]);
						result.propertyTypes.Add(xr["name"], xr["type"]);
					}
				}
			}
			return result;
		}

		public string GetTypeOfProperty(string name)
		{
			return propertyTypes[name];
		}

		public string GetProperty(string name)
		{
			return properties[name];
		}

		public int GetPropertyAsInt(string name)
		{
			return int.Parse(GetProperty(name));
		}

		public float GetPropertyAsFloat(string name)
		{
			return float.Parse(GetProperty(name).Replace('.',','));
		}

		public bool GetPropertyAsBool(string name)
		{
			return bool.Parse(GetProperty(name));
		}

		public string GetPropertyAsFile(string name)
		{
			return ".\\" + GetProperty(name);
		}

		public Color GetPropertyAsColor(string name)
		{
			Color result = new Color();
			string value = GetProperty(name);
			string @as = value.Substring(1, 2);
			string rs = value.Substring(3, 2);
			string gs = value.Substring(5, 2);
			string bs = value.Substring(7, 2);
			int a = int.Parse(@as, System.Globalization.NumberStyles.HexNumber);
			int r = int.Parse(rs, System.Globalization.NumberStyles.HexNumber);
			int g = int.Parse(gs, System.Globalization.NumberStyles.HexNumber);
			int b = int.Parse(bs, System.Globalization.NumberStyles.HexNumber);
			result = new Color(r, g, b, a);
			return result;
		}

		public static TiledProperties operator +(TiledProperties a, TiledProperties b)
		{
			TiledProperties result = new TiledProperties();
			if (a != null)
				foreach (var p in a.properties)
				{
					if (!result.properties.ContainsKey(p.Key))
						result.properties.Add(p.Key, p.Value);
				}
			if (b != null)
				foreach (var p in b.properties)
				{
					if (!result.properties.ContainsKey(p.Key))
						result.properties.Add(p.Key, p.Value);
				}
			if (a != null)
				foreach (var p in a.propertyTypes)
				{
					if (!result.propertyTypes.ContainsKey(p.Key))
						result.propertyTypes.Add(p.Key, p.Value);
				}
			if (b != null)
				foreach (var p in b.propertyTypes)
				{
					if (!result.propertyTypes.ContainsKey(p.Key))
						result.propertyTypes.Add(p.Key, p.Value);
				}
			return result;
		}
	}
}