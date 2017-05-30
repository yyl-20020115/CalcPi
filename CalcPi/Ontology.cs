using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

/// <summary>
/// 本体论
/// </summary>
namespace Ontology
{
	/// <summary>
	/// Aliases are the names we call a single something.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public class AliasesAttribute : Attribute
	{
		readonly string[] aliases;

		// This is a positional argument
		public AliasesAttribute(params string[] aliases)
		{
			this.aliases = aliases;
		}

		public string[] Aliases => this.aliases;
	}

	/// <summary>
	/// Existence is something (an object), that does not exist.
	/// 
	/// Because it does not exist, it is not limited.
	/// 
	/// As a basic concept, Existence is the super class of Nature.
	/// However, in the reality, Existence is the Nature it self.
	/// 
	/// We are following congnitive order here; it may be the opposite
	/// of the natural order.
	/// </summary>
	[Aliases(
		"Existence","存在"	
	)]
	public abstract class Existence : Object
	{
		/// <summary>
		/// The one and only (sole) existence is the nature itself.
		/// </summary>
		public readonly static Existence Sole = new Nature();

		/// <summary>
		/// It does not exist, this is its unique property.
		/// </summary>
		public virtual bool Exists => false;

		/// <summary>
		/// It's not limited, derived from the non-existent property
		/// </summary>
		public virtual bool IsLimited => false;

		/// <summary>
		/// It's opposite is itself.
		/// </summary>
		/// <param name="existence"></param>
		/// <returns></returns>
		public static Existence operator -(Existence existence)
		{
			return existence;
		}
		/// <summary>
		/// Inner storage of the members.
		/// </summary>
		protected ISet<Existence> existences = null;
		/// <summary>
		/// Existence is a set of existences
		/// </summary>
		public virtual ISet<Existence> Existences {
			get => this.existences ?? (this.existences = new HashSet<Existence>());
			set
			{
				this.existences = value ?? new HashSet<Existence>();
			}
		}

		public Existence(params Existence[] existences)
		{
			if (existences == null || existences.Any(e => e == null))
				throw new ArgumentNullException(nameof(existences));

			this.existences = new HashSet<Existence>(existences);
		}
		/// <summary>
		/// Natures equal only when natures equals (no order)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return (obj is Existence e)
				&& this.Existences.SetEquals(e.Existences);
		}

		public override int GetHashCode()
		{
			return this.Existences.Aggregate(0, (a, n) => a.GetHashCode() ^ n.GetHashCode());
		}

		public override string ToString()
		{
			return "(" + string.Join(",", this.Existences.Select(n => n.ToString())) + ")";
		}
	}

	/// <summary>
	/// Nature an existence that exists from unknown beginning.
	/// </summary>
	[Aliases(
		"Nature","自然", 
		"Tao", "道",
		"Onto","本体",
		"Limitless","无限"
	)]
	public class Nature : Existence
	{
		public static Nature operator -(Nature nature)
		{
			return nature;
		}

		/// <summary>
		/// It exists as it does not
		/// </summary>
		public override bool Exists => true;

		/// <summary>
		/// Unknown beginning.
		/// </summary>
		public DateTime? Beginning = null;

		public Nature(params Existence[] existences)
			:base(existences)
		{

		}
	}

	/// <summary>
	/// Being is a kind of Nature
	/// </summary>
	[Aliases(
		"Being","有","存有"
	)]
	public class Being : Nature
	{
		/// <summary>
		/// Inherites existence property
		/// </summary>
		public override bool Exists => base.Exists;

		/// <summary>
		/// Not Being Is Void: Void = - Being
		/// </summary>
		/// <param name="being"></param>
		/// <returns></returns>
		public static Void operator -(Being being)
		{
			return being == null ? null : new Void(being.Existences.ToArray());
		}

		/// <summary>
		/// Being Is Void: Void = Being
		/// </summary>
		/// <param name="being"></param>
		public static implicit operator Void (Being being)
		{
			return being == null ? null : new Void(being.Existences.ToArray());
		}
		public Being(params Existence[] existences)
			:base(existences)
		{

		}
		public override bool Equals(object obj)
		{
			return obj is Being && base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return this.Existences.Count == 0 ? 1 : base.GetHashCode();
		}
		public override string ToString()
		{
			return this.Existences.Count == 0 ? nameof(Being) : base.ToString();
		}
	}

	/// <summary>
	/// Void is a kind of Nature
	/// </summary>
	[Aliases(
		"Void","不存在","虚无"	
	)]
	public class Void : Nature
	{
		/// <summary>
		/// Inherites existence property
		/// </summary>
		public override bool Exists => base.Exists;

		/// <summary>
		/// Not Void Is Being: Being = - Void
		/// </summary>
		/// <param name="void"></param>
		/// <returns></returns>
		public static Being operator -(Void @void)
		{
			return @void == null ? null : new Being(@void.Existences.ToArray());
		}
		
		/// <summary>
		/// Void Is Being: Void = Being
		/// </summary>
		/// <param name="being"></param>
		public static implicit operator Being (Void @void)
		{
			return @void == null ? null : new Being(@void.Existences.ToArray());
		}

		public Void(params Existence[] existences)
			:base(existences)
		{

		}

		public override bool Equals(object obj)
		{
			return obj is Void && base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this.Existences.Count == 0 ? 0 : base.GetHashCode();
		}

		public override string ToString()
		{
			return this.Existences.Count == 0 ? nameof(Void) : base.ToString();
		}
	}

	/// <summary>
	/// Number is a kind of Being
	/// </summary>
	public abstract class Number: Being
	{
		public virtual object NumberValue { get; set; }
	}

	public abstract class SignedNumber : Number
	{
		public virtual bool IsPositive { get; set; } = true;
	}
	public class Integer: SignedNumber
	{
		public static readonly Integer Zero = new Integer();

		public BigInteger Value =BigInteger.Zero;

		public override object NumberValue {
			get => this.Value;
			set
			{
				if(value is BigInteger bi)
				{
					this.Value = bi;
				}
			}
		}

	}

	public class Real : SignedNumber
	{
		public static  readonly Real Zero = new Real();

		public double Value = 0.0;

		public override object NumberValue
		{
			get => this.Value;
			set
			{
				if (value is double d)
				{
					this.Value = d;
				}
			}
		}
	}

	public class Rational: Real
	{

	}

	public class Irrational : Real
	{


	}

	public class Natural: Integer
	{
		public static new readonly Natural Zero = new Natural();

		public override bool IsPositive => true;
	}

	[Aliases("RealComplex", "复数", "实复数")]
	public class RealComplex : Number
	{
		public static readonly RealComplex Zero = new RealComplex();

		/// <summary>
		/// This is real part
		/// </summary>
		[Aliases("Real", "实部", "LowDimension", "低维")]
		public Real Real = Real.Zero;
		/// <summary>
		/// This is imaginary part
		/// </summary>
		[Aliases("Imaginary","虚部", "HighDimension","高维")]
		public Real Imaginary = Real.Zero;
	}

	[Aliases("NaturalComplex", "自然复数")]
	public class NaturalComplex: Number
	{
		public static readonly NaturalComplex Zero = new NaturalComplex();

		/// <summary>
		/// This is real part
		/// </summary>
		[Aliases("Real","实部", "LowDimension","低维")]
		public Natural Real = Natural.Zero;
		/// <summary>
		/// This is imaginary part
		/// </summary>
		[Aliases("Imaginary", "虚部", "HighDimension", "高维")]
		public Natural Imaginary = Natural.Zero;
	}

	/// <summary>
	/// Infinite is a kind of Number
	/// </summary>
	[Aliases(
		"Infinite","无穷","无穷大"
	)]
	public class Infinite : SignedNumber
	{
		public override bool IsPositive => true;

		public static implicit operator byte(Infinite infinity)
		{
			return byte.MaxValue;
		}

		public static implicit operator sbyte(Infinite infinity)
		{
			return infinity == null ? (sbyte)0 : (infinity.IsPositive ? sbyte.MaxValue : sbyte.MinValue);
		}

		public static implicit operator ushort(Infinite infinity)
		{
			return ushort.MaxValue;
		}

		public static implicit operator short(Infinite infinity)
		{
			return infinity == null ? (short)0 : (infinity.IsPositive ? short.MaxValue : short.MinValue);
		}

		public static implicit operator uint(Infinite infinity)
		{
			return uint.MaxValue;
		}

		public static implicit operator int(Infinite infinity)
		{
			return infinity == null ? (int)0 : (infinity.IsPositive ? int.MaxValue : int.MinValue);
		}

		public static implicit operator ulong(Infinite infinity)
		{
			return ulong.MaxValue;
		}

		public static implicit operator long(Infinite infinity)
		{
			return infinity == null ? (long)0 : (infinity.IsPositive ? long.MaxValue : long.MinValue);
		}

		public static implicit operator float(Infinite infinity)
		{
			return infinity == null ? float.Epsilon : (infinity.IsPositive ? float.PositiveInfinity: float.NegativeInfinity);
		}

		public static implicit operator double(Infinite infinity)
		{
			return infinity == null ? double.Epsilon : (infinity.IsPositive ? double.PositiveInfinity : double.NegativeInfinity);
		}

		public static implicit operator decimal(Infinite infinity)
		{
			return infinity == null ? decimal.Zero : (infinity.IsPositive ? decimal.MaxValue : decimal.MinValue);
		}

	}

	/// <summary>
	/// Zero is a kind of Number, but you can treat it as void
	/// </summary>
	[Aliases(
		"Zero", "0"
	)]
	public class Zero : SignedNumber
	{
		public static implicit operator byte(Zero zero)
		{
			return byte.MinValue;
		}

		public static implicit operator sbyte(Zero zero)
		{
			return (sbyte)0;
		}

		public static implicit operator ushort(Zero zero)
		{
			return ushort.MinValue;
		}

		public static implicit operator short(Zero zero)
		{
			return (short)0;
		}

		public static implicit operator uint(Zero zero)
		{
			return uint.MinValue;
		}

		public static implicit operator int(Zero zero)
		{
			return 0;
		}

		public static implicit operator ulong(Zero zero)
		{
			return ulong.MinValue;
		}

		public static implicit operator long(Zero zero)
		{
			return 0L;
		}

		public static implicit operator float(Zero zero)
		{
			return 0.0f;
		}

		public static implicit operator double(Zero zero)
		{
			return 0.0;
		}

		public static implicit operator decimal(Zero zero)
		{
			return decimal.Zero;
		}
	}


}
