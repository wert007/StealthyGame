using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameDebug.GameConsole;
using StealthyGame.Engine.GameDebug.DataStructures.TimeManagement;
using StealthyGame.Engine.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures
{
	public class Recorder
	{
		readonly int savedFramesCount = 80;
		Queue<RecordEntry> savedFrames;
		int currentFrame;

		public Recorder()
		{
			savedFrames = new Queue<RecordEntry>();


			StdConsoleCommands.SaveFrames += (dir) =>
			{
				for (int i = 0; i < savedFrames.Count; i++)
				{
					RenderTarget2D cur = savedFrames.ElementAt(i).RenderTarget;
					using (FileStream fs = new FileStream(Path.Combine(dir, (i + 1).ToString() + ".png"), FileMode.CreateNew))
						cur.SaveAsPng(fs, cur.Width, cur.Height);
				}
			};
		}

		public void AddRecordEntry(RecordEntry entry)
		{
			savedFrames.Enqueue(entry);
			if (savedFrames.Count > savedFramesCount)
			{
				savedFrames.Dequeue().Dispose();
			}
		}

		public void Draw(Renderer2D renderer, GameTime time)
		{
			currentFrame = (currentFrame + 1) % savedFrames.Count;
			var cur = savedFrames.ElementAt(currentFrame);
			renderer.Draw(cur.RenderTarget, Vector2.Zero, Color.White);
			if(time.ElapsedGameTime < cur.Time)
			{
				Thread.Sleep((int)(cur.Time.TotalMilliseconds - time.ElapsedGameTime.TotalMilliseconds));
			}
		}

		public static RenderTarget2D CopyRenderTarget(RenderTarget2D origin)
		{
			RenderTarget2D result = new RenderTarget2D(origin.GraphicsDevice, origin.Width, origin.Height);
			Color[] data = new Color[origin.Width * origin.Height];
			origin.GetData(data);
			result.SetData(data);
			return result;
		}
	}

	public struct RecordEntry
	{
		public RenderTarget2D RenderTarget { get; private set; }
		public TimeWatchCollection TimeWatch { get; private set; }
		public TimeSpan Time { get; private set; }

		public RecordEntry(RenderTarget2D renderTarget, GameTime time)
		{
			RenderTarget = Recorder.CopyRenderTarget(renderTarget);
			Time = time.ElapsedGameTime;
			TimeWatch = null;
		}

		public void Dispose()
		{
			RenderTarget.Dispose();
		}
	}
}
