using StealthyGame.Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Geometrics
{ 
	public class Bresenham
	{
		static int sgn(int x)
		{
			return (x > 0) ? 1 : (x < 0) ? -1 : 0;
		}

		public static Index2[] Circle(Index2 origin, int radius)
		{
			List<Index2> res = new List<Index2>();
			int x = radius - 1;
			int y = 0;
			int dx = 1;
			int dy = 1;
			int err = dx - (radius << 1);

			while( x>= y)
			{
				res.Add(origin + new Index2(+x, +y));
				res.Add(origin + new Index2(+y, +x));
				res.Add(origin + new Index2(-y, +x));
				res.Add(origin + new Index2(-x, +y));
				res.Add(origin + new Index2(-x, -y));
				res.Add(origin + new Index2(-y, -x));
				res.Add(origin + new Index2(+y, -x));
				res.Add(origin + new Index2(+x, -y));

				if(err <= 0)
				{
					y++;
					err += dy;
					dy += 2;
				}
				else if( err > 0)
				{
					x--;
					dx += 2;
					err += dx - (radius << 1);
				}
			}
			return res.ToArray();
		}

		public static Index2[] LineFromAngle(Index2 start, float angle, int length)
		{
			Index2 end = start + new Index2((int)Math.Round(Math.Cos(angle) * length), (int)Math.Round(Math.Sin(angle) * length));
			return LineBetweenTwoPoints(start, end);
		}

		public static Index2[] LineBetweenTwoPoints(Index2 start, Index2 end)
		{
			List<Index2> result = new List<Index2>();
			Index2 pos;
			int t;
			Index2 delta;
			Index2 inc;
			Index2 parallel;
			Index2 diagonal;
			int deltaslowdirection;
			int deltafastdirection;
			int err;
			delta = end - start;
			inc = new Index2(sgn(delta.X), sgn(delta.Y));
			if (delta.X < 0) delta.X = -delta.X;
			if (delta.Y < 0) delta.Y = -delta.Y;
			if (delta.X > delta.Y)
			{
				parallel = new Index2(inc.X, 0);
				diagonal = new Index2(inc.X, inc.Y);
				deltaslowdirection = delta.Y;
				deltafastdirection = delta.X;
			}
			else
			{
				parallel = new Index2(0, inc.Y);
				diagonal = new Index2(inc.X, inc.Y);
				deltaslowdirection = delta.X;
				deltafastdirection = delta.Y;
			}

			pos = new Index2(start.X, start.Y);
			err = deltafastdirection / 2;
			result.Add(pos);
			for(t = 0; t < deltafastdirection; t++)
			{
				err -= deltaslowdirection;
				if(err < 0)
				{
					err += deltafastdirection;
					pos += diagonal;
				}
				else
				{
					pos += parallel;
				}
				result.Add(pos);
			}

			return result.ToArray();
		}

		public static Index3[] LineFromTwoAngles(Index3 start, float angle1, float angle2, int length)
		{
			Index3 end = start + new Index3((int)Math.Round(Math.Cos(angle1) * length), (int)Math.Round(Math.Sin(angle1) * length), (int)Math.Round(-Math.Sin(angle2) * length));
			return gbham3(start, end);
		}

		static Index3[] gbham3(Index3 start, Index3 end)
		{
			List<Index3> result = new List<Index3>();
			Index3 pos;
			Index3 delta;
			Index3 step;
			bool swapXY;
			bool swapXZ;
			int driftXY;
			int driftXZ;
			Index3 c;
			swapXY = Math.Abs(end.Y - start.Y) > Math.Abs(end.X - start.X);
			if(swapXY)
			{
				int buf = start.X;
				start.X = start.Y;
				start.Y = buf;
				buf = end.X;
				end.X = end.Y;
				end.Y = buf;
			}

			swapXZ = Math.Abs(end.Z - start.Z) > Math.Abs(end.X - start.X);
			if(swapXZ)
			{
				int buf = start.X;
				start.X = start.Z;
				start.Z = buf;
				buf = end.X;
				end.X = end.Z;
				end.Z = buf;
			}
			delta = new Index3(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y), Math.Abs(end.Z - start.Z));
			driftXY = delta.X / 2;
			driftXZ = delta.X / 2;
			step = new Index3(start.X > end.X ? -1 : 1,
				start.Y > end.Y ? -1 : 1,
				start.Z > end.Z ? -1 : 1);
			pos = new Index3(start);
			 for(pos.X = start.X; pos.X < end.X; pos.X += step.X)
			{
				c = new Index3(pos);
				if(swapXZ)
				{
					int buf = c.X;
					c.X = c.Z;
					c.Z = buf;
				}
				if(swapXY)
				{
					int buf = c.X;
					c.X = c.Y;
					c.Y = buf;
				}
				result.Add(c);
				driftXY -= delta.Y;
				driftXZ -= delta.Z;

				if(driftXY < 0)
				{
					pos.Y += step.Y;
					driftXY += delta.X;
				}
				if(driftXZ < 0)
				{
					pos.Z += step.Z;
					driftXZ += delta.X;
				}
			}
			return result.ToArray();
		}
			
	}
}
