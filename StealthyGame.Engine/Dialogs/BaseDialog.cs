using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Dialogs
{
	public abstract class BaseDialog
	{
		public string Name { get; private set; }
		public string Content { get; private set; }
		public int Id { get; private set; }
		//Picture. Emotions etc pp

		public BaseDialog(int id, string name, string content)
		{
			this.Id = id;
			this.Name = name;
			this.Content = content;
		}

		protected DialogManager GetManager()
		{
			return DialogManager.GetByDialog(this);
		}

		public abstract BaseDialog GetNext();
	}
}
