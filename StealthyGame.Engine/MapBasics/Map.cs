using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Debug;
using StealthyGame.Engine.MapBasics.Tiled;
using StealthyGame.Engine.MapBasics.Tiles;
using StealthyGame.Engine.Pathfinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.MapBasics.Tiles.InteractiveTiles;
using StealthyGame.Engine.View.Lighting;
using Microsoft.Xna.Framework.Input;

namespace StealthyGame.Engine.MapBasics
{
	public class Map
	{
		int width;
		int height;
		public TiledObjectGroup[] ObjectGroups { get; private set; }
		public MapLayer[] Layers { get; private set; }
		protected BasicTile[,] tiles;
		public ANewHope hope;
		
		public Index2 PixelSize => new Index2(width * 32, height * 32);
		public Index2 TileSize => new Index2(width, height);

		//public Map(int width, int height, TiledObjectGroup[] objectGroups, MapLayer[] layers, GraphicsDevice graphicsDevice)
		//{
		//	this.width = width;
		//	this.height = height;
		//	this.ObjectGroups = objectGroups;
		//	this.Layers = layers;
		//	tiles = new BasicTile[width, height];
		//	hope = new ANewHope(graphicsDevice, this, tileSetManager);
		//	for (int x = 0; x < width; x++)
		//	{
		//		for (int y = 0; y < height; y++)
		//		{
		//			for (int z = 0; z < layers.Length; z++)
		//			{
		//				GetParameters(x, y,z, out Index3 index, out bool interactive, out bool animation, out TiledProperties properties);
		//				LoadTiles(x, y, z, properties, interactive, animation);
		//			}
		//		}
		//	}
		//	for (int x = 0; x < width; x++)
		//	{
		//		for (int y = 0; y < height; y++)
		//		{
		//			tiles[x, y].PathfindingNode.SetNeighbours(
		//				GetNeighbours(x, y));
		//		}
		//	}
		//}

		public Map()
		{
		}

		public BasicTile GetTile(Index2 position) => GetTile(position.X, position.Y);
		public BasicTile GetTile(int x, int y) => tiles[x, y];

		protected virtual void LoadTiles(int x, int y, int z, TiledProperties properties, bool interactive, bool animation)
		{
			if (interactive && animation)
			{
				switch (properties.GetProperty("Type"))
				{
					case "Door":
						tiles[x, y] = new DoorTile("door", new Index3(x, y, z), Layers[z].DrawOrder);
						break;
					default:
						tiles[x, y] = new InteractiveTile("door", new Index3(x, y, z), Layers[z].DrawOrder);
						break;
				}
			}
			else if (animation && !interactive)
			{
				tiles[x, y] = new AnimatedTile("flower", new Index3(x, y, z), Layers[z].DrawOrder);
			}
			else if (!interactive && !animation)
			{
				tiles[x, y] = new BasicTile("dirt", new Index3(x, y, z), Layers[z].DrawOrder);
			}
			else
				throw new NotSupportedException("No Tile should be interacitve but not animated");
			tiles[x, y].AddProperties(properties);
		}

		protected void GetParameters(int x, int y, int z, out Index3 index, out bool interactive, out bool animation, out TiledProperties properties)
		{
			index = new Index3(x, y, z);
			interactive = Layers[z].IsInteractive(x,y);
			animation = Layers[z].IsAnimation(x,y);
			properties = Layers[z].GetProperties(x, y);
		}

		public bool IsAnimated(Vector2 pixel, int z = -1)
		{
			bool result = false;
			Index2 index = (Index2) (pixel / 32);
			if (z == -1)
			{
				for (int _z = 0; _z < Layers.Length; _z++)
				{
					GetParameters(index.X, index.Y, _z, out Index3 _index, out bool i, out result, out TiledProperties p);
				}
				return result;
			}
			GetParameters(index.X, index.Y, z, out Index3 __index, out bool _i, out result, out TiledProperties _p);
			return result;
		}

		protected Node[] GetNeighbours(int x, int y)
		{
			Node[,] buf = new Node[3, 3];

			for (int xOff = -1; xOff <= 1; xOff++)
			{
				for (int yOff = -1; yOff <= 1; yOff++)
				{
					if (x + xOff < 0 ||
						y + yOff < 0 ||
						x + xOff >= width ||
						y + yOff >= height ||
						(xOff == 0 && yOff == 0))

						buf[1 + xOff, 1 + yOff] = null;
					else
						buf[1 + xOff, 1 + yOff] = tiles[x + xOff, y + yOff].PathfindingNode;
				}
			}

			TiledObjectGroup group = GetGroupByName("Pathfinding");

			int rx = x * BasicTile.Size;
			int ry = y * BasicTile.Size;

			foreach (var obj in group.Objects.Where(o => o.Type == "Room"))
			{
				if (obj.Rectangle.Right == rx || obj.Rectangle.X == rx)
				{
					if (obj.Rectangle.Y <= ry && obj.Rectangle.Bottom >= ry + BasicTile.Size) //Complete Left Side
					{
						for (int j = 0; j < 3; j++)
						{
							buf[0, j] = null;
						}
					}
					else if (obj.Rectangle.Y <= ry - BasicTile.Size && obj.Rectangle.Bottom >= ry) //Left Top
					{
						buf[0, 0] = null;
					}
					else if (obj.Rectangle.Y <= ry + BasicTile.Size && obj.Rectangle.Bottom >= ry + 2 * BasicTile.Size) //Left Bottom
					{
						buf[0, 2] = null;
					}
				}
				if (obj.Rectangle.Right == rx + BasicTile.Size || obj.Rectangle.X == rx + BasicTile.Size)
				{
					if (obj.Rectangle.Y <= ry && obj.Rectangle.Bottom >= ry + BasicTile.Size) //Complete Right Side
					{
						for (int j = 0; j < 3; j++)
						{
							buf[2, j] = null;
						}
					}
					else if (obj.Rectangle.Y <= ry - BasicTile.Size && obj.Rectangle.Bottom >= ry) //Right Top
					{
						buf[2, 0] = null;
					}
					else if (obj.Rectangle.Y <= ry + BasicTile.Size && obj.Rectangle.Bottom >= ry + 2 * BasicTile.Size) //Right Bottom
					{
						buf[2, 2] = null;
					}
				}
				if (obj.Rectangle.Bottom == ry + BasicTile.Size || obj.Rectangle.Y == ry + BasicTile.Size)
				{
					if (obj.Rectangle.X <= rx && obj.Rectangle.Right >= rx + BasicTile.Size) //Complete Bottom Side
					{
						for (int j = 0; j < 3; j++)
						{
							buf[j, 2] = null;
						}
					}
					else if (obj.Rectangle.X <= rx - BasicTile.Size && obj.Rectangle.Right >= rx) //Bottom Left
					{
						buf[0, 2] = null;
					}
					else if (obj.Rectangle.X <= rx + BasicTile.Size && obj.Rectangle.Right >= rx + 2 * BasicTile.Size) //Bottom Right
					{
						buf[2, 2] = null;
					}
				}
				if (obj.Rectangle.Bottom == ry || obj.Rectangle.Y == ry)
				{
					if (obj.Rectangle.X <= rx && obj.Rectangle.Right >= rx + BasicTile.Size) //Complete Top Side
					{
						for (int j = 0; j < 3; j++)
						{
							buf[j, 0] = null;
						}
					}
					else if (obj.Rectangle.X <= rx - BasicTile.Size && obj.Rectangle.Right >= rx) //Top Left
					{
						buf[0, 0] = null;
					}
					else if (obj.Rectangle.X <= rx + BasicTile.Size && obj.Rectangle.Right >= rx + 2 * BasicTile.Size) //Top Right
					{
						buf[2, 0] = null;
					}
				}
			}

			if (true)
				foreach (var door in GetGroupByName("Pathfinding").Objects.Where(o => o.Type == "Door"))
				{
					int xd1 = (int) (door.Position.X / BasicTile.Size);
					int yd1 = (int) (door.Position.Y / BasicTile.Size);
					int xd2 = -1;
					int yd2 = -1;
					//Should be 64 > 32
					if (door.Size.X > door.Size.Y)
					{
						xd2 = xd1 + 1;
						yd2 = yd1;
					}
					//Should be 64 > 32
					else if (door.Size.Y > door.Size.X)
					{
						xd2 = xd1;
						yd2 = yd1 + 1;
					}
					else
						throw new FormatException("Error on Map. Door objects in the Pathfinding Layer must contain two tiles!");
					if (x == xd1 && y == yd1)
					{
						buf[xd2 - x, yd2 - y] = GetTile(xd2, yd2).PathfindingNode;
					}
					else if (x == xd2 && y == yd2)
					{
						buf[1 + xd1 - x, 1 + yd1 - y] = GetTile(xd1, yd1).PathfindingNode;
					}
				}


			List<Node> result = new List<Node>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (buf[i, j] != null)
						result.Add(buf[i, j]);
				}
			}
			return result.ToArray();
		}

		public TiledObjectGroup GetGroupByName(string name)
		{
			foreach (var og in ObjectGroups)
			{
				if (og.Name == name)
					return og;
			}
			return null;
		}

		public void Load(string file, GraphicsDevice graphicsDevice)
		{
			int width = -1;
			int height = -1;
			int tilesize = -1;
			List<TiledObjectGroup> objectGroups = new List<TiledObjectGroup>();
			List<MapLayer> layers = new List<MapLayer>();
			TileSetManager ts = new TileSetManager();

			hope = new ANewHope(graphicsDevice, this);

			using (XmlReader xr = XmlReader.Create(file))
			{
				while (xr.Read())
				{
					if (xr.NodeType == XmlNodeType.EndElement)
						continue;
					switch (xr.Name)
					{
						case "tileset":
							using (StringReader sr = new StringReader(xr.ReadOuterXml()))
							using (XmlReader r = XmlReader.Create(sr))
								ts.LoadTileSet(file, r, graphicsDevice);
							break;
						case "layer":
							MapLayer layer = null;
							string name = xr["name"];
							using (StringReader sr = new StringReader(xr.ReadOuterXml()))
							using (XmlReader r = XmlReader.Create(sr))
								layer = MapLayer.Load(r, graphicsDevice, ts);
							if (!name.StartsWith("Light"))
							{
								layers.Add(layer);
							}
							else
							{
								hope.AddLayer(layer);
							}
							break;
						case "map":
							width = int.Parse(xr["width"]);
							height = int.Parse(xr["height"]);
							tilesize = int.Parse(xr["tilewidth"]); //should be same as tileheight
							break;
						case "objectgroup":
							TiledObjectGroup og = new TiledObjectGroup(xr["name"]);
							og.Load(xr);
							objectGroups.Add(og);
							break;
						default:
							break;
					}
				}
			}
			BasicTile.SetTileSize(tilesize);

			hope.LoadTileSets(ts);

			this.width = width;
			this.height = height;
			this.ObjectGroups = objectGroups.ToArray();
			this.Layers = layers.ToArray();
			tiles = new BasicTile[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					for (int z = 0; z < layers.Count; z++)
					{
						GetParameters(x, y, z, out Index3 index, out bool interactive, out bool animation, out TiledProperties properties);
						LoadTiles(x, y, z, properties, interactive, animation);
					}
				}
			}
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					tiles[x, y].PathfindingNode.SetNeighbours(
						GetNeighbours(x, y));
				}
			}

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (tiles[x, y] is InteractiveTile)
					{
						var i = tiles[x, y] as InteractiveTile;
						i.Load(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file), @".\..\TileSets", i.Properties.GetPropertyAsFile("AnimationFile")), graphicsDevice);
					}
				}
			}

			var a = GetGroupByName("Lights");
			foreach (var b in a.Objects)
			{
				hope.AddLight(new Light((Index2)b.Position, (short)b.Properties.GetPropertyAsInt("Strength"), 1f, b.Properties.GetPropertyAsColor("Color")));
			}
		}

		public void Update(GameTime time)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (tiles[x, y] is InteractiveTile)
					{
						var i = tiles[x, y] as InteractiveTile;
						i.Update(time);
					}
				}
			}
		}

		public void Draw(SpriteBatch batch, Matrix cam)
		{
			hope.CalculateLightmap(batch, cam, PixelSize, new Color(51, 51, 51));
			for (int i = 0; i < Layers.Length; i++)
			{
				batch.Draw(Layers[i].Prerendered, position: new Vector2(), color: Color.White,layerDepth: Layers[i].DrawOrder);
			}
			hope._Draw(batch);
			batch.End();
			batch.Begin(SpriteSortMode.Deferred, LightHelper.Multiply, null, null, null, null, cam);
			hope.Draw(batch);
			batch.End();
			batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam);
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (tiles[x, y] is InteractiveTile)
					{
						(tiles[x, y] as InteractiveTile).Draw(batch, Color.White);
					}
				}
			}

		}
	}
}