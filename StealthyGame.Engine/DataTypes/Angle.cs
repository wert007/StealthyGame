using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthyGame.Engine.DataTypes
{
	public struct Angle
	{
		public float Value { get; private set; }

		public Angle(float value)
		{
			this.Value = value;
			Normalize();
		}

		public Angle(float value, AngleUnit unit)
		{
			switch (unit)
			{
				case AngleUnit.Radians:
					this.Value = value;
					break;
				case AngleUnit.Degree:
					this.Value = MathHelper.Pi * value / 180f;
					break;
				default:
					throw new NotImplementedException();
			}
			Normalize();
		}

		private void Normalize()
		{
			Value = (Value + MathHelper.TwoPi) % MathHelper.TwoPi;
		}

		public bool LiesInBetween(Angle minor, Angle major)
		{
			return new AnglePair(minor, major).IsInRange(this);
		}

		public override string ToString() => Value.ToString("0.000");
		public override bool Equals(object obj)
		{
			if (obj is Angle angle)
				return Value == angle.Value;
			return base.Equals(obj);
		}

		public static bool operator ==(Angle a, Angle b) => a.Value == b.Value;
		public static bool operator !=(Angle a, Angle b) => a.Value != b.Value;

		public static Angle operator +(Angle a, Angle b) => new Angle(a.Value + b.Value);
		public static Angle operator -(Angle a, Angle b) => new Angle(a.Value - b.Value);
		public static Angle operator *(Angle a, Angle b) => new Angle(a.Value * b.Value);
		public static Angle operator /(Angle a, Angle b) => new Angle(a.Value / b.Value);

		public static implicit operator float(Angle a) => a.Value;
		public static implicit operator Angle(float a) => new Angle(a);

	}

	public enum AngleUnit
	{
		Radians,
		Degree
	}

	public struct AnglePair
	{
		public Angle Minor { get; set; }
		public Angle Major { get; set; }

		public AnglePair(Angle minor, Angle major)
		{
			this.Minor = minor;
			this.Major = major;
		}


		public bool IsInRange(Angle value)
		{
			if (Minor <= Major)
			{
				return Minor <= value && value <= Major;
			}
			else
			{
				return (Minor <= value && value <= MathHelper.TwoPi)
					|| (0 <= value && value <= Major);
			}
		}

		public override bool Equals(object obj)
		{
			return Minor == Minor && Major == Major;
		}

		public override string ToString()
		{
			return "Minor: " + Minor + ", Major: " + Major;
		}

		public override int GetHashCode()
		{
			var hashCode = 1075898624;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Angle>.Default.GetHashCode(Minor);
			hashCode = hashCode * -1521134295 + EqualityComparer<Angle>.Default.GetHashCode(Major);
			return hashCode;
		}

		public static bool operator ==(AnglePair a, AnglePair b) => a.Equals(b);
		public static bool operator !=(AnglePair a, AnglePair b) => !a.Equals(b);
	}
}
