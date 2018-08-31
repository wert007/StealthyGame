using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.MapBasics
{
	public class MapLayer
	{
		int width;
		int height;
		string name;
		int[] data;
		TiledProperties properties;
		TileSetManager tileSets;
		List<BasicTile> animatedOrInteractiveTiles;
		public Texture2D Prerendered { get; private set; }
		public float DrawOrder { get { return properties.GetPropertyAsFloat("DrawOrder"); } }

		public static MapLayer Load(XmlReader xr, GraphicsDevice graphicsDevice, TileSetManager tileSets, string directory)
		{
			xr.Read();
			MapLayer result = new MapLayer();
			result.tileSets = tileSets;
			result.name = xr["name"];
			result.width = int.Parse(xr["width"]);
			result.height = int.Parse(xr["height"]);
			result.animatedOrInteractiveTiles = new List<BasicTile>();
			while(xr.Read())
			{
				if(xr.Name == "data" && xr.NodeType == XmlNodeType.Element)
				{
					string[] data = xr.ReadElementContentAsString().Split(',');
					result.data = new int[data.Length];
					for (int i = 0; i < data.Length; i++)
					{
						result.data[i] = int.Parse(data[i]);
					}
				}
				if (xr.Name == "properties" && xr.NodeType == XmlNodeType.Element)//Properties
				{
					using (StringReader sr = new StringReader(xr.ReadOuterXml()))
					using (XmlReader r = XmlReader.Create(sr))
						result.properties = TiledProperties.Load(r);
				}
				if (xr.Name == "layer" && xr.NodeType == XmlNodeType.EndElement) //The End
					break;
			}

			Texture2D t = new Texture2D(graphicsDevice, result.width * BasicTile.Size, result.height * BasicTile.Size);

			for (int i = 0; i < result.data.Length; i++)
			{
				int x = i % result.width;
				int y = i / result.width;
				bool interactive = result.tileSets.IsInteractive(result.data[i]);
				bool animated = result.tileSets.IsAnimation(result.data[i]);
				if(animated || interactive)
				{
					TiledProperties properties = result.tileSets.GetProperties(result.data[i]);
					result.animatedOrInteractiveTiles.Add(BasicTile.LoadTile(interactive, animated, properties, new Index2(x, y)));
					continue;
				}
				Color[] data = result.tileSets.GetData(result.data[i]);
				if (data == null)
				{
					data = new Color[BasicTile.Size * BasicTile.Size];
					for (int j = 0; j < BasicTile.Size * BasicTile.Size; j++)
					{
						data[j] = Color.Transparent;
					}
				}
				t.SetData(0, new Rectangle(x * BasicTile.Size, y * BasicTile.Size, BasicTile.Size, BasicTile.Size), data, 0, data.Length);
			}
			foreach (var tiles in result.animatedOrInteractiveTiles)
			{
				if(tiles is InteractiveTile interactive)
				{
					interactive.Load(Path.Combine(directory, @".\..\TileSets", interactive.Properties.GetPropertyAsFile("AnimationFile")), graphicsDevice);

				}
			}
			result.Prerendered = t;
			//result.Prerendered.SaveAsPng(new FileStream(".\\prerendered" + result.name + ".png", FileMode.Create), result.Prerendered.Width, result.Prerendered.Height);
			//Console.WriteLine("prerendered" + result.name + "Saved");
			return result;
		}

		internal InteractiveTile GetInteractiveAt(Index2 index)
		{
			return animatedOrInteractiveTiles.Where(t => t is InteractiveTile && t.Index == index).Single() as InteractiveTile;  //Brave
		}

		public int[] GetRawData() => data;

		public bool IsInteractive(int x, int y)
		{
			int i = y * width + x;
			return tileSets.IsInteractive(data[i]);
		}

		public bool IsAnimation(int x, int y)
		{
			int i = y * width + x;
			return tileSets.IsAnimation(data[i]);
		}

		public TiledProperties GetProperties(int x, int y)
		{
			int i = y * width + x;
			return tileSets.GetProperties(data[i]);
		}

		internal void Draw(SpriteBatch batch)
		{
			batch.Draw(Prerendered, Vector2.Zero, Color.White);
			foreach (var tile in animatedOrInteractiveTiles)
			{
				if (tile is InteractiveTile interactive)
					interactive.Draw(batch, Color.White);
			}
		}

		internal void Update(GameTime time)
		{
			foreach (var tile in animatedOrInteractiveTiles)
			{
				tile.Update(time);
			}
		}
	}
}
