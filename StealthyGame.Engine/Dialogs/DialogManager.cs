using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StealthyGame.Engine.Dialogs
{
	public class DialogManager
	{
		List<BaseDialog> dialogs;
		static List<DialogManager> dialogManagers;

		public DialogManager()
		{
			dialogs = new List<BaseDialog>();
			if (dialogManagers == null)
				dialogManagers = new List<DialogManager>();
			dialogManagers.Add(this);

		}

		public BaseDialog ById(int id)
		{
			foreach (var dialog in dialogs)
			{
				if (dialog.Id == id)
					return dialog;
			}
			return null;
		}

		public bool ContainsDialog(BaseDialog dialog)
		{
			return dialogs.Contains(dialog);
		}

		public static DialogManager GetByDialog(BaseDialog dialog)
		{
			foreach (var dm in dialogManagers)
			{
				if (dm.ContainsDialog(dialog))
				{
					return dm;
				}
			}
			return null;
		}

		public static DialogManager Load(string file)
		{
			DialogManager result = new DialogManager();
			using (XmlReader xr = XmlReader.Create(file))
			{
				while (xr.Read())
				{
					if (xr.NodeType == XmlNodeType.EndElement)
						continue;
					switch (xr.Name)
					{
						case "dialog":
							int id = int.Parse(xr["id"]);
							string name = xr["name"];
							string content = "";
							int next = -1;
							if (!string.IsNullOrWhiteSpace(xr["next"]))
								next = int.Parse(xr["next"]);
							switch (xr["type"])
							{
								case "text":
									xr.Read();
									content = xr.ReadContentAsString();
									result.dialogs.Add(new TextDialog(id, name, content, next));
									break;
								case "decision":
									int nextyes = int.Parse(xr["nextyes"]);
									int nextno = int.Parse(xr["nextno"]);
									string yes = xr["yes"];
									string no = xr["no"];
									xr.Read();
									content = xr.ReadContentAsString();
									result.dialogs.Add(new DecisionDialog(id, name, content, nextyes, nextno, yes, no));
									break;
								default:
									throw new NotSupportedException("Unknown Type " + xr["type"]);
							}
							break;
						default:
							break;
					}
				}
			}

			return result;
		}
	}
}