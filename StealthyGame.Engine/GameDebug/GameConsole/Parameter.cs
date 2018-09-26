using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StealthyGame.Engine.GameDebug.GameConsole
{
	public class Parameter
	{
		//Stolen from my Slides-Project (github.com/wert007/Slides)
		public static string Regex => "-" + Command + "( (" + Number + "|" + File + "|" + String + "))?";
		public static string Command => @"[a-zA-Z][a-zA-Z_0-9]*";
		public static string Number => @"(" + Float + "|" + Integer + ")";
		public static string Integer => @"\d+";
		public static string Float => @"(\d*\.\d+?|\d+f)";
		public static string String => @"@?(.*)?";
		public static string File => @"'[a-zA-Z_0-9\\\/:. ]+'";

		public string[] Names { get; set; }
		public string Name => Names[0];
		public bool HasValue => Type != ParameterType.Boolean;
		public ParameterType Type { get; set; }
		public bool HasShort { get; set; }
		public bool IsMeta { get; set; }
		public Parameter[] Others { get; private set; }
		public bool IsExclusive => Others == null;

		List<object> lastValues;

		public Parameter(string name, ParameterType type)
		{
			Names = new string[1] { name };
			Type = type;
			lastValues = new List<object>();
		}

		public Parameter(string name, string shortName, ParameterType type)
		{
			if (string.IsNullOrWhiteSpace(shortName))
				Names = new string[1] { name };
			else
				Names = new string[2] { name, shortName };
			Type = type;
			lastValues = new List<object>();
		}

		public Parameter(string[] names, ParameterType type)
		{
			Names = names;
			Type = type;
			lastValues = new List<object>();
		}

		public Parameter()
		{
		}

		public ParameterValue CreateValue(object value)
		{
			lastValues.Add(value);
			return new ParameterValue(this, value);
		}

		public IEnumerable<string> GetSuggestions(string text)
		{
			switch (Type)
			{
				case ParameterType.Command:
					return GameConsole.GetCommands().Select(c => c.Names[0]); //TODO
				case ParameterType.File:
					return StdConsoleCommands.FileTree.Current.GetChildren().Select(f => f.ShortName);
				case ParameterType.String:
				case ParameterType.Integer:
				case ParameterType.Float:
					return lastValues.ConvertAll(o => o.ToString());
				case ParameterType.Boolean:
					break;
				default:
					throw new NotImplementedException();
			}
			return new string[0];
		}

		internal void SetOthers(Parameter[] others)
		{
			this.Others = others;
		}
	}

	public struct ParameterValue
	{
		public Parameter Parameter { get; private set; }
		public object Value { get; private set; }

		public ParameterValue(Parameter parameter, object value)
		{
			Parameter = parameter;
			Value = value;
		}

		public string GetAsString()
		{
			if (Parameter.Type != ParameterType.String)
				GameConsole.Warning("Warning: Trying to Convert from " + Parameter.Type + " to String!");
			return Value.ToString();
		}
		public string GetAsFile()
		{
			if (Parameter.Type != ParameterType.File)
				GameConsole.Warning("Warning: Trying to Convert from " + Parameter.Type + " to File(String)!");
			return Value.ToString();
		}
		public int GetAsInt()
		{
			if (Parameter.Type != ParameterType.Integer)
				GameConsole.Warning("Warning: Trying to Convert from " + Parameter.Type + " to Integer!");
			return int.Parse(Value.ToString());
		}
		public float GetAsFloat()
		{
			if (Parameter.Type != ParameterType.Float)
				GameConsole.Warning("Warning: Trying to Convert from " + Parameter.Type + " to Float!");
			return float.Parse(Value.ToString());
		}
		public bool GetAsBool()
		{
			if (Parameter.Type != ParameterType.Boolean)
				GameConsole.Warning("Warning: Trying to Convert from " + Parameter.Type + " to Boolean!");
			return bool.Parse(Value.ToString());
		}
	}

	public enum ParameterType
	{
		String,
		File,
		Integer,
		Float,
		Boolean,
		Command
	}
}
