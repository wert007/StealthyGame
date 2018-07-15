using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Dialogs
{
	class TextDialog : BaseDialog
	{
		int next;

		public TextDialog(int id, string name, string content, int next) : base(id, name, content)
		{
			this.next = next;
		}

		public override BaseDialog GetNext()
		{
			return DialogManager.GetByDialog(this).ById(next);
		}
	}
}
