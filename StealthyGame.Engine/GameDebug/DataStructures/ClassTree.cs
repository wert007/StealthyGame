using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.GameDebug.UI
{
	public class ClassTree
	{
		public ClassTreeItem Root { get; set; }
		public ClassTreeItem Current { get; set; }

		public ClassTree()
		{

		}
	}

	public class ClassTreeItem
	{
		Type elementType;
		public bool HasName => Name != null && Name.Length > 0;
		public string Name { get; }
		public string ClassName { get; }
		public object Value { get; }
		public object Parent { get; }
		List<ClassTreeItem> children;
		public IEnumerable<ClassTreeItem> Children => children;
		
		public ClassTreeItem(object element, string name, object parent = null)
		{
			this.elementType = element.GetType();
			this.ClassName = elementType.Name;
			Value = element;
			Name = name;
			Parent = parent;
		}

		public void GenerateChildren()
		{
			children = new List<ClassTreeItem>();
			foreach (var attribute in elementType.GetFields(BindingFlags.Public |
															 BindingFlags.NonPublic |
															 BindingFlags.Instance))
			{
				object value = attribute.GetValue(Value);
				if(value != null)
					children.Add(new ClassTreeItem(value, attribute.Name, Value));
			}
		}

		public override string ToString()
		{
			var methodInfo = Value.GetType().GetMethod("ToString", Type.EmptyTypes, new ParameterModifier[0]);
			if (methodInfo.DeclaringType == elementType)
				return ClassName + " " + Name + " = " + Value.ToString() + ";";
			else
				return ClassName + " " + Name + ";";
		}

		internal void SetValue(object value)
		{
			Parent.GetType().GetField(Name).SetValue(Parent, value);
		}
	}
}
