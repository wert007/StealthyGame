using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.DataTypes;
using StealthyGame.Engine.Geometrics;
using StealthyGame.Engine.Helper;
using StealthyGame.Engine.View.Lighting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Wordpress.Fields
{
	public class Field
	{
		protected static readonly int radius = 256;
		public static readonly int Size = 2 * radius + 1;
		protected static readonly Index2 lightPosition = new Index2(radius, radius);
		protected static readonly Circle circle = new Circle(lightPosition, radius);
		protected static readonly byte[,] obstacles = new byte[Size, Size];
		
		private bool[,] computedBool;
		private int[,] curComputedCount;
		private int[,] computedCount;
		private float runs;
		private Stopwatch watch;
		private long timePreCompute;
		private long timeSimulation;

		public string Name { get; private set; }

		public bool Done { get; protected set; }
		public Index2 Current { get; protected set; }
		public Index2 AntiCurrent { get; protected set; }

		protected bool UseAnti { get; set; }

		static Texture2D map;
		static Color[] lightData;
		Texture2D light;

		public Field(string name)
		{
			Name = name;
			watch = new Stopwatch();
			Reset();

		}

		public void Reset()
		{
			curComputedCount = new int[Size, Size];
			computedCount = new int[Size, Size];
			computedBool = new bool[Size, Size];
			if(UseAnti)
			{
				for (int x = 0; x < Size; x++)
				{
					for (int y = 0; y < Size; y++)
					{
						computedBool[x, y] = true;
					}
				}
			}
			runs = 0;
			Current = new Index2(-1, -1);
			AntiCurrent = new Index2(-1, -1);
			Done = true;
			InnerReset();
		}

		protected virtual void InnerReset()
		{

		}

		public static void Init(GraphicsDevice graphicsDevice, Random random)
		{
			map = new Texture2D(graphicsDevice, Size, Size);
			Color[] data = new Color[Size * Size];
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					obstacles[x, y] = 0;
					if (random.NextDouble() > 0.9f)
						obstacles[x, y] = 255;


					if (circle.IsOutside(new Index2(x, y)))
						obstacles[x, y] = 127;
					data[x * Size + y] = new Color((Vector3.One - new Vector3(obstacles[x, y] / 256.0f)));
					if (x == lightPosition.X && y == lightPosition.Y)
						data[x * Size + y] = Color.Red;
				}
			}
			map.SetData(data);
		}

		public void Compute()
		{
			Reset();
			watch.Restart();
			PrePreCompute();
			watch.Stop();
			timePreCompute = watch.ElapsedMilliseconds;
			watch.Restart();
			while (!Done)
			{
				ComputeSingleStep();
				UpdateStats();
			}
			watch.Stop();
			timeSimulation = watch.ElapsedMilliseconds;
		}
		private void PrePreCompute()
		{
			curComputedCount = new int[Size, Size];
			Done = false;
			runs++;
			PreCompute();
		}
		protected virtual void PreCompute()
		{
			Done = true;
		}

		protected virtual void ComputeSingleStep()
		{
		}

		public void SimulateStep()
		{
			if (Done)
			{
				watch.Restart();
				PrePreCompute();
				watch.Stop();
				timePreCompute = watch.ElapsedMilliseconds;
			}
			watch.Restart();
			ComputeSingleStep();
			watch.Stop();
			timeSimulation = watch.ElapsedMilliseconds;
			UpdateStats();
		}

		private void UpdateStats()
		{
			if (Current.X >= 0 && Current.Y >= 0 && Current.X < Size && Current.Y < Size)
			{
				curComputedCount[Current.X, Current.Y]++;
				computedCount[Current.X, Current.Y]++;
				computedBool[Current.X, Current.Y] = true;
			}
			else if(AntiCurrent.X >= 0 && AntiCurrent.Y >= 0 && AntiCurrent.X < Size && AntiCurrent.Y < Size)
			{
				curComputedCount[AntiCurrent.X, AntiCurrent.Y]--;
				computedCount[AntiCurrent.X, AntiCurrent.Y]--;
				computedBool[AntiCurrent.X, AntiCurrent.Y] = false;
			}
		}

		private float MostChecks()
		{
			int max = -1;
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					max = Math.Max(computedCount[x, y], max);
				}
			}
			return max / runs;
		}

		private float LeastChecks()
		{
			int min = int.MaxValue;
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					min = Math.Min(computedCount[x, y], min);
				}
			}
			return min / runs;
		}

		private float AverageChecks()
		{
			int sum = 0;
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					sum += computedCount[x, y];
				}
			}
			return sum / (Size * Size) / runs;
		}

		private bool[,] LightedFields()
		{
			bool[,] result = new bool[Size, Size];
			Light l = new Light(lightPosition, radius, 0.5f, new HSVColor(Color.PaleTurquoise));
			foreach (var endpoint in circle.All)
			{
				foreach (var point in new LightRaycast(l, lightPosition, endpoint))
				{
					if (obstacles[point.X, point.Y] > 0)
						break;
					result[point.X, point.Y] = true;
				}
			}
			return result;
		}

		private float Accuracy()
		{
			float wrongTiles = 0;
			var perfect = LightedFields();
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					if (perfect[x, y] != computedBool[x, y])
						wrongTiles++;
				}
			}
			return 1 - wrongTiles / (Size * Size);
		}

		public static void Draw(SpriteBatch batch, Field field, int size, Index2 position, bool displayAllInformation)
		{
			if (field.light == null)
				field.light = new Texture2D(batch.GraphicsDevice, Size, Size);
			lightData = new Color[Size * Size];
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{

					//if (field.computedCount[x, y] / field.runs > 0)
						lightData[x * Size + y] = new Color(0, 255, 0) * (0.2f) * (field.computedCount[x, y] / field.runs);
				}
			}
			field.light.SetData(lightData);

			batch.Draw(map, new Rectangle(position.X, position.Y, size * Size, size * Size), Color.White);
			batch.Draw(field.light, new Rectangle(position.X, position.Y, size * Size, size * Size), Color.White);
			batch.DrawFilledRectangle(new Rectangle(position.X + field.Current.X * size, position.Y + field.Current.Y * size, size, size), Color.LawnGreen);
			batch.DrawFilledRectangle(new Rectangle(position.X, position.Y, 200, 18), new Color(Color.White, 0.5f));
			batch.DrawString(Game1.font, field.Name, position, Color.Black);
			if (displayAllInformation)
			{
				batch.DrawFilledRectangle(new Rectangle(position.X, position.Y + 18, 200, 6 * 18), new Color(Color.White, 0.5f));
				batch.DrawString(Game1.font, "Max: " + field.MostChecks().ToString("0.00"), position + new Index2(0, 18), Color.Black);
				batch.DrawString(Game1.font, "Avg: " + field.AverageChecks().ToString("0.00"), position + new Index2(0, 36), Color.Black);
				batch.DrawString(Game1.font, "Min: " + field.LeastChecks().ToString("0.00"), position + new Index2(0, 54), Color.Black);
				batch.DrawString(Game1.font, "Acc: " + field.Accuracy().ToString("0.00%"), position + new Index2(0, 72), Color.Black);
				batch.DrawString(Game1.font, "TPC: " + field.timePreCompute.ToString("0ms"), position + new Index2(0, 90), Color.Black);
				batch.DrawString(Game1.font, "TSS: " + field.timeSimulation.ToString("0ms"), position + new Index2(0, 108), Color.Black);
			}
			return;
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					Color fill = new Color((Vector3.One - new Vector3(obstacles[x, y] / 256.0f)));
					if (obstacles[x, y] == 0)
						if (!field.UseAnti)
						{
							if (field.computedCount[x, y] / field.runs > 0)
								fill = new Color(255 - fill.R, 255, 255 - fill.B) * (0.2f) * (field.computedCount[x, y] / field.runs);
						}
						else
						{
							if (-field.computedCount[x, y] / field.runs > 0)
								fill = new Color(255 - fill.R, 255 - fill.G, 255) * (0.2f) * (-field.computedCount[x, y] / field.runs);
						}

					if (x == lightPosition.X && y == lightPosition.Y)
						fill = Color.Red;
					if (x == field.Current.X && y == field.Current.Y)
						fill = Color.LawnGreen;
					if (x == field.AntiCurrent.X && y == field.AntiCurrent.Y)
						fill = Color.CornflowerBlue;

					batch.DrawFilledRectangle(new Rectangle(x * size + position.X, y * size + position.Y, size, size), fill);
					if(displayAllInformation)
					{
						//if (obstacles[x, y] == 0)
							//batch.DrawString(Game1.font, field.curComputedCount[x, y].ToString(), new Vector2(x * size + position.X, y * size + position.X), Color.Black);
					}
				}
			}
		}
	}
}
