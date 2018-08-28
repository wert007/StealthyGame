using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using StealthyGame.Engine.MapBasics.Tiled;

namespace StealthyGame.Engine.MapBasics.Tiles
{
	public class TileSetManager
	{
		List<TileSet> tileSets;
		List<int> firstGid;

		public IEnumerable<TileSet> TileSets => tileSets;

		public TileSetManager()
		{
			tileSets = new List<TileSet>();
			firstGid = new List<int>();
		}

		public void LoadTileSet(string file, XmlReader xr, GraphicsDevice graphicsDevice)
		{
			xr.Read();
			string tileSetPath = Path.Combine(Path.GetDirectoryName(file), xr["source"]);
			tileSets.Add(TileSet.Load(tileSetPath, graphicsDevice));
			firstGid.Add(int.Parse(xr["firstgid"]));
		}

		public void AddTileSet(TileSet tileSet)
		{
			firstGid.Add(tileSets.Sum(t => t.TileCount) + 1);
			tileSets.Add(tileSet);
		}

		public Rectangle GetSourceRectangle(int index)
		{
			Rectangle result = new Rectangle();
			int i = IndexToTileSetIndex(index);
			if (i >= 0)
			{
				result = tileSets[i].GetSourceRectangle(index - firstGid[i]);
			}
			return result;
		}

		//0, r, data, 0, data.Length
		public Color[] GetData(int index)
		{
			Color[] result = null;
			int i = IndexToTileSetIndex(index);
			if(i >= 0)
			{
				result = new Color[GetSourceRectangle(index).Width * GetSourceRectangle(index).Height];
				tileSets[i].Texture.GetData(0, GetSourceRectangle(index), result, 0, result.Length);
			}
			return result;
		}

		public TileSet IndexToTileSet(int index)
		{
			int tileSet = IndexToTileSetIndex(index);
			if (tileSet < 0) return null;
			return tileSets[tileSet];
		}

		public int IndexToTileSetIndex(int index)
		{
			if (index == 0 || index > firstGid.Last() + tileSets.Last().TileCount)
				return -1;
			for (int i = 0; i < firstGid.Count - 1; i++)
			{
				if (index >= firstGid[i] && index < firstGid[i + 1])
					return i;
			}
			return firstGid.Count - 1;
		}

		public int ShortenIndex(int index)
		{
			if (index == 0 || index > firstGid.Last() + tileSets.Last().TileCount)
				return -1;
			for (int i = 0; i < firstGid.Count - 1; i++)
			{
				if (index >= firstGid[i] && index < firstGid[i + 1])
					return index - firstGid[i];
			}
			return index - firstGid.Last();
		}

		public bool IsInteractive(int index)
		{
			bool result = false;
			int i = IndexToTileSetIndex(index);
			if (i >= 0)
			{
				result = tileSets[i].IsInteractive(index - firstGid[i]);
			}
			return result;
		}

		public bool IsAnimation(int index)
		{
			bool result = false;
			int i = IndexToTileSetIndex(index);
			if (i >= 0)
			{
				result = tileSets[i].IsAnimation(index - firstGid[i]);
			}
			return result;
		}

		public TiledProperties GetProperties(int index)
		{
			TiledProperties result = null;
			int i = IndexToTileSetIndex(index);
			if(i >= 0)
			{
				result = tileSets[i].GetProperties(index - firstGid[i]);
			}
			return result;
		}
	}
}
