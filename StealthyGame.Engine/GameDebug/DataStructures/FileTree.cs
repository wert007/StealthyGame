using Microsoft.Xna.Framework.Graphics;
using StealthyGame.Engine.GameDebug.GameConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.DataStructures
{
	public class FileTree
	{
		public FileTreeItem Root { get; set; }
		public FileTreeItem Current { get; set; }

		public FileTree()
		{
			Root = new FileTreeItem(".\\");
			Current = Root;
		}

		public FileTree(string path)
		{
			Root = new FileTreeItem(path);
			Current = Root;
		}
	}

	public class FileTreeItem
	{
		public string FilePath { get; private set; }
		public bool IsFile => Path.HasExtension(FilePath);
		public string FileExtension => Path.GetExtension(FilePath).ToLower();
		public string FileName => Path.GetFileNameWithoutExtension(FilePath);
		public FileTreeItem[] Children { get; private set; }
		public string ShortName
		{
			get
			{
				if(IsFile)
				{
					return FileName + "." + FileExtension;
				}
				else
				{
					return FilePath;
				}
			}
		}

		public FileTreeItem(string path)
		{
			this.FilePath = path;
		}

		public void GenerateChildren()
		{
			if (IsFile)
				return;
			List<FileTreeItem> result = new List<FileTreeItem>();
			foreach (var fe in Directory.GetFileSystemEntries(FilePath, "*", SearchOption.TopDirectoryOnly))
			{
				result.Add(new FileTreeItem(fe));
			}
			Children = result.OrderBy(f => f.IsFile).ToArray();
		}

		public FileTreeItem[] GetChildren()
		{
			if (Children == null)
				GenerateChildren();
			return Children;
		}

		public object Load()
		{
			object result = null;
			if (!IsFile)
				return result;
			switch (FileExtension)
			{
				case ".png":
				case ".jpg":
					using (FileStream fs = new FileStream(FilePath, FileMode.Open))
						result = Texture2D.FromStream(null, fs);
					break;
				default:
					GameConsole.GameConsole.Error("Couldn't match file-extension " + FileExtension);
					break;
			}

			return result;
		}
	}
}
