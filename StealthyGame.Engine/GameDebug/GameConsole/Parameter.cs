﻿using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StealthyGame.Engine.GameDebug.Console
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

		public string[] Names { get; private set; }
		public bool HasValue { get; private set; }
		public bool IsOptional { get; private set; }
		public ParameterType Type { get; private set; }
		List<object> lastValues;

		public Parameter(string name, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = new string[1] { name };
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
			lastValues = new List<object>();
		}

		public Parameter(string name, string shortName, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = new string[2] { name, shortName };
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
			lastValues = new List<object>();
		}

		public Parameter(string[] names, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = names;
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
			lastValues = new List<object>();
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
					return GameConsole.GetCommands().Select(c => c.Name);
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
	}

	public class MetaParameter : Parameter
	{
		public MetaParameter(string name, bool isOptional, ParameterType type)
			: base(name, true, isOptional, type)
		{
		}

		public MetaParameter(string name, string shortName, bool isOptional, ParameterType type)
			: base(name, shortName, true, isOptional, type)
		{
		}

		public MetaParameter(string[] names, bool isOptional, ParameterType type)
			: base(names, true, isOptional, type)
		{
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
				GameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to String!", Color.Yellow);
			return Value.ToString();
		}
		public string GetAsFile()
		{
			if (Parameter.Type != ParameterType.File)
				GameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to File(String)!", Color.Yellow);
			return Value.ToString();
		}
		public int GetAsInt()
		{
			if (Parameter.Type != ParameterType.Integer)
				GameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Integer!", Color.Yellow);
			return int.Parse(Value.ToString());
		}
		public float GetAsFloat()
		{
			if (Parameter.Type != ParameterType.Float)
				GameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Float!", Color.Yellow);
			return float.Parse(Value.ToString());
		}
		public bool GetAsBool()
		{
			if (Parameter.Type != ParameterType.Boolean)
				GameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Boolean!", Color.Yellow);
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
