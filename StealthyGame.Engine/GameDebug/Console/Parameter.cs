using Microsoft.Xna.Framework;

namespace StealthyGame.Engine.GameDebug.Console
{
	public interface IParameter
	{
		ParameterType Type { get; }
		bool IsOptional { get; }
		bool HasValue { get; }
		string[] Names { get; }
	}

	public struct MetaParameter : IParameter
	{
		public string[] Names { get; private set; }
		public bool HasValue { get; private set; } 
		public bool IsOptional { get; private set; }
		public ParameterType Type { get; private set; }

		public MetaParameter(string name, bool isOptional, ParameterType type)
		{
			Names = new string[1] { name };
			HasValue = true;
			IsOptional = isOptional;
			Type = type;
		}

		public MetaParameter(string name, string shortName, bool isOptional, ParameterType type)
		{
			Names = new string[2] { name, shortName };
			HasValue = true;
			IsOptional = isOptional;
			Type = type;
		}

		public MetaParameter(string[] names, bool isOptional, ParameterType type)
		{
			Names = names;
			HasValue = true;
			IsOptional = isOptional;
			Type = type;
		}
	}

	public struct Parameter : IParameter
	{
		public static string Regex => "-" + CommandRegex + "( (" + Number + "|" + File + "|" + String + "))?";


		static string CommandRegex => @"[a-zA-Z][a-zA-Z_0-9]*";

		//Stolen from my Slides-Project (github.com/wert007/Slides)
		public static string Number => @"(" + Float + "|" + Integer + ")";
		public static string Integer => @"\d+";
		public static string Float => @"(\d*\.\d+?|\d+f)";
		public static string String => @"@?(.*)?";
		public static string File => @"'[a-zA-Z_0-9\\:. ]+'";

		public string[] Names { get; private set; }
		public bool HasValue { get; private set; }
		public bool IsOptional { get; private set; }
		public ParameterType Type { get; private set; }

		public Parameter(string name, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = new string[1] { name };
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
		}

		public Parameter(string name, string shortName, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = new string[2] { name, shortName };
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
		}

		public Parameter(string[] names, bool hasValue, bool isOptional, ParameterType type)
		{
			Names = names;
			HasValue = hasValue;
			IsOptional = isOptional;
			Type = type;
		}
	}

	public struct ParameterValue
	{
		public IParameter Parameter { get; private set; }
		public object Value { get; private set; }

		public ParameterValue(IParameter parameter, object value)
		{
			Parameter = parameter;
			Value = value;
		}

		public string GetAsString()
		{
			if (Parameter.Type != ParameterType.String)
				InGameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to String!", Color.Yellow);
			return Value.ToString();
		}
		public string GetAsFile()
		{
			if (Parameter.Type != ParameterType.File)
				InGameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to File(String)!", Color.Yellow);
			return Value.ToString();
		}
		public int GetAsInt()
		{
			if (Parameter.Type != ParameterType.Integer)
				InGameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Integer!", Color.Yellow);
			return int.Parse(Value.ToString());
		}
		public float GetAsFloat()
		{
			if (Parameter.Type != ParameterType.Float)
				InGameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Float!", Color.Yellow);
			return float.Parse(Value.ToString());
		}
		public bool GetAsBool()
		{
			if (Parameter.Type != ParameterType.Boolean)
				InGameConsole.Log("Warning: Trying to Convert from " + Parameter.Type + " to Boolean!", Color.Yellow);
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
