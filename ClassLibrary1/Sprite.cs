
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StealthyGame.Engine.OpenTKVersion
{
	public class Sprite : Quad
	{
		int textureID;
		internal int ID => textureID;

		public Sprite(string filename)
		{
			textureID = LoadImage(filename);
		}

		int LoadImage(Bitmap image)
		{
			int texID = GL.GenTexture();

			GL.BindTexture(TextureTarget.Texture2D, texID);
			BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
				 ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
				 OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			image.UnlockBits(data);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			return texID;
		}

		int LoadImage(string filename)
		{
			try
			{
				Bitmap file = new Bitmap(filename);
				return LoadImage(file);
			}
			catch (Exception e)
			{
				return -1;
			}
		}
	}
}