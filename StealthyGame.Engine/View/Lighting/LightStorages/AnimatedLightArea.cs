using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
//using StealthyGame.Engine.Debug;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.MapBasics.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.View.Lighting
{
	public class AnimatedLightArea : ILightStorage
	{
		Dictionary<string, LightArea[]> animations;
		string currentAnimation;
		int currentIndex;
		public int Length { get { return animations.Count; } }
		public Index2 Position { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public AnimatedLightArea(int x, int y, int width, int height)
		{
			animations = new Dictionary<string, LightArea[]>();
			Position = new Index2(x, y);
			Width = width;
			Height = height;
			currentAnimation = "idleshadow";
		}

		public void AddAnimation(string name, params LightArea[] areas)
		{
			if(!animations.ContainsKey(name))
			animations.Add(name, areas);
		}

		public HSVColor GetAt(int x, int y)
		{
			return animations[currentAnimation][currentIndex].GetAt(x, y);
		}

		public void SetAt(int x, int y, HSVColor color)
		{
			throw new NotSupportedException();
		}

		public static AnimatedLightArea CreateNew(BasicTile source, Light[] lights, byte[,] basicobstacles, float angle)
		{
			List<Raycast> raycasts = new List<Raycast>();
			//Collect Raycasts
			foreach (var l in lights)
			{
				Index2 start = (Index2)l.Position;
				//TODO compute only rays pointing to source

				//for (float a = 0; a < 2 * Math.PI; a += angle)
				//{
				//	//Raycast raycast = new Raycast(Bresenham.LineFromAngle(start, a, l.Radius));
				//	if (raycast.Intersects(source))
				//	{
				//		raycasts.Add(raycast);
				//		DebugSpriteBatch.AddRaycast(raycast, l);
				//		LightHelper.ShrinkRaycast(raycast, basicobstacles);
				//	}
				//}
			}
			if (raycasts.Count == 0) return null;
			int minX = raycasts.Min(r => r.Min.X);
			int minY = raycasts.Min(r => r.Min.Y);
			int maxX = raycasts.Max(r => r.Max.X);
			int maxY = raycasts.Max(r => r.Max.Y);
			AnimatedLightArea result = new AnimatedLightArea(minX, minY, maxX - minX, maxY - minY);

			//DebugSpriteBatch.AddRectangle(new DebugRectangle(new Rectangle(source.Position.X, source.Position.Y, 32, 32), Color.MonoGameOrange));
			//DebugSpriteBatch.AddRectangle(new DebugRectangle(new Rectangle(minX, minY, result.Width, result.Height), Color.Red));
			//Compute Raycasts

			for (int a = 0; a < source.Animations.Length; a++)
			{
				if (!source.Animations.GetCurrent(a).Name.EndsWith("shadow"))
					continue;

				//Texture2D[] shadowFrames = new Texture2D[source.Animations.GetCurrent(a).Length];
				LightArea[] areas = new LightArea[source.Animations.GetCurrent(a).Length];
				for (int f = 0; f < source.Animations.GetCurrent(a).Length; f++)
				{

					//TODO Go through each frame. And less copypaste. gracias.
					byte[,] obstacles = (byte[,])basicobstacles.Clone();

					Color[] data = new Color[BasicTile.Size * BasicTile.Size];
					Rectangle frame = source.Animations.GetCurrent(a).GetFrame(f);
					source.Animations.GetCurrent(a).Texture.GetData(0, frame, data, 0, frame.Width * frame.Height);
					for (int sx = 0; sx < BasicTile.Size; sx++)
						for (int sy = 0; sy < BasicTile.Size; sy++)
							if (data[sy * BasicTile.Size + sx].A == 255)
								obstacles[source.Position.X + sx, source.Position.Y + sy] = Math.Max(obstacles[source.Position.X + sx, source.Position.Y + sy],
									(byte)(255 - data[sy * BasicTile.Size + sx].B));
							else
								obstacles[source.Position.X + sx, source.Position.Y + sy] = 0;

					areas[f] = new LightArea(minX, minY, result.Width, result.Height);
					foreach (var l in lights)
						foreach (var raycast in raycasts)
						{
							LightHelper.ComputeRaycast(raycast, l, null, areas[f], obstacles);
						}

					result.AddAnimation(source.Animations.GetCurrent(a).Name, areas);

					//shadowFrames[f] = new Texture2D(graphicsDevice, result.Width, result.Height);
					//Color[] col = new Color[(result.Width) * (result.Height)];
					//for (int x = 0; x < result.Width; x++)
					//	for (int y = 0; y < result.Height; y++)
					//	{
					//		Color l = areas[f].GetAt(x, y);

					//		if (ambientLight.ToVector3().LengthSquared() > l.ToVector3().LengthSquared())
					//			col[y * (result.Width) + x] = ambientLight;
					//		else
					//			col[y * (result.Width) + x] = l;
					//	}
					//shadowFrames[f].SetData(col);

				}
				//var anima = new Animation2D(shadowFrames, source.Animations.GetCurrent(a));
				//anima.Texture.SaveAsPng(new System.IO.FileStream("animation-" + Guid.NewGuid().ToString() + ".png", System.IO.FileMode.Create), anima.Texture.Width, anima.Texture.Height);
				//result.shadow.AddAnimation(anima);
			}

			source.Animations.AnimationChanged += (n) =>
			{
				//result.shadow.Play(n + "shadow");
			};

			if (result.Length == 0)
				return null;
			return result;
		}

	}
}