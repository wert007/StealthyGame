using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.Dialogs
{
	class DecideDialog : BaseDialog
	{
		int nextYes;
		int nextNo;
		bool decision;
		string yes;
		string no;

		public DecideDialog(int id, string name, string content, int nextYes, int nextNo, string yes, string no) : base(id, name, content)
		{
			this.nextYes = nextYes;
			this.nextNo = nextNo;
			this.yes = yes;
			this.no = no;
		}

		public void Decide(bool decision)
		{
			this.decision = decision;
		}

		public override BaseDialog GetNext()
		{
			if (decision)
				return GetManager().ById(nextYes);
			else
				return GetManager().ById(nextNo);
		}
	}
}
