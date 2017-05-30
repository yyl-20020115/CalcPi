﻿using System;
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
		protected string[] aliases;
		public string[] Aliases => this.aliases;

		public AliasesAttribute(params string[] aliases)
		{
			this.aliases = aliases;
		}
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
		"Existence", "存在"
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
		/// Existences equal when the are referencing same object.
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator ==(Existence e1, Existence e2)
		{
			return object.ReferenceEquals(e1, e2);
		}
		/// <summary>
		/// Existences do not equal when the are referencing same object,
		/// this means, it is not itself.
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator !=(Existence e1, Existence e2)
		{
			return object.ReferenceEquals(e1, e2);
		}

		/// <summary>
		/// Inner storage of the members.
		/// </summary>
		protected ISet<Existence> existences = null;
		/// <summary>
		/// Existence is a set of existences
		/// </summary>
		public virtual ISet<Existence> Existences
		{
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
		"Nature", "自然",
		"Tao", "道",
		"Onto", "本体",
		"Limitless", "无限"
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
			: base(existences)
		{

		}
	}

	/// <summary>
	/// Being is a kind of Nature
	/// </summary>
	[Aliases(
		"Being", "有", "存有"
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
		public static implicit operator Void(Being being)
		{
			return being == null ? null : new Void(being.Existences.ToArray());
		}
		public Being(params Existence[] existences)
			: base(existences)
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
		"Void", "不存在", "虚无"
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
		public static implicit operator Being(Void @void)
		{
			return @void == null ? null : new Being(@void.Existences.ToArray());
		}

		public Void(params Existence[] existences)
			: base(existences)
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
	/// Number is a kind of Being which has a property named 
	/// NumberValue. This value is originally the count of 
	/// the sub existences.
	/// </summary>
	[Aliases("Number", "数")]
	public abstract class Number : Being
	{
		public virtual object NumberValue => this.Existences.Count;

		public Number(params Existence[] existences)
			: base(existences)
		{

		}
	}

	public class LinearNumber : Number
	{
		public virtual bool IsZero { get; } = false;

		public virtual bool IsPositive { get; } = true;

		public LinearNumber(params Existence[] existences)
			: base(existences)
		{

		}
	}
	public class StructuralNumber : Number
	{

	}

	public class FiniteNumber: LinearNumber
	{

	}
	[Aliases("Integer", "整数")]
	public class Integer : FiniteNumber
	{
		public static readonly Integer Zero = new Integer(0);
		public static readonly Integer One = new Integer(1);
		public static readonly Integer MinusOne = new Integer(-1);

		public static implicit operator Real(Integer integer)
		{
			return integer == null ? null : new Real((double)integer.Value);
		}

		public static Infinite operator +(Integer integer, Infinite infinite)
		{
			return infinite;
		}

		public readonly BigInteger Value = BigInteger.Zero;

		public override bool IsPositive => this.Value > 0;

		public override bool IsZero => this.Value.IsZero;

		public override object NumberValue => this.Value;

		public Integer(int value = 0)
			: this((BigInteger)value)
		{

		}
		public Integer(BigInteger value)
		{
			this.Value = value;
		}
	}

	[Aliases("Natural", "自然数")]
	public class Natural : Integer
	{
		public static new readonly Natural Zero = new Natural(0);
		public static new readonly Natural One = new Natural(1);

		public override bool IsPositive => true;

		public Natural(uint value = 0)
			: this((BigInteger)value)
		{

		}
		public Natural(BigInteger value)
			: base(value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value)))
		{
		}
	}

	[Aliases("Real", "实数")]
	public class Real : FiniteNumber
	{
		public static readonly Real Zero = new Real(0.0);
		public static readonly Real One = new Real(1.0);
		public static readonly Real MinusOne = new Real(-1.0);

		public static explicit operator Integer(Real real)
		{
			return real == null ? null : new Integer((BigInteger)real.Value);
		}


		public static Infinite operator +(Real real, Infinite infinite)
		{
			return infinite;
		}

		public readonly double Value = 0.0;

		public override bool IsPositive => this.Value > 0.0;

		public override bool IsZero => this.Value == 0.0;

		public override object NumberValue => this.Value;

		public Real(double value = 0.0)
		{
			this.Value = value;
		}
	}

	[Aliases("Rational", "有理数")]
	public class Rational : Real
	{
		public readonly Real Numerator = Real.Zero;
		public readonly Real Denominator = Real.One;

		public Rational(Real Numerator = null, Real Denominator = null)
			: base((Numerator = (Numerator ?? Real.Zero)).Value
				 / ((Denominator = (Denominator ?? Real.One)).Value))
		{
			this.Numerator = Numerator;
			this.Denominator = Denominator;
		}
	}

	[Aliases("Irrational", "无理数")]
	public class Irrational : Real
	{
		public static double CalcPi(ulong n = 1, ulong max = 100)
		{
			return n < max
				? 2.0 + (1.0 * n / (2.0 * n + 1.0) * CalcPi(n + 1, max))
				: 2.0 * max + 1.0;
		}
		public static double CalcE(ulong n = 1, ulong max = 100, double x = 1.0)
		{
			return n == 0 || n >= max
				? 1.0
				: 1.0 + x / n * CalcE(n + 1, max, x);
		}
		public static readonly Irrational Pi = new Irrational(CalcPi());
		public static readonly Irrational E = new Irrational(CalcE());

		public Irrational(double value = 0.0)
			: base(value)
		{

		}
	}

	[Aliases("Complex", "复数", "实复数")]
	public class Complex : StructuralNumber
	{
		public static readonly Complex Zero = new Complex();
		public static readonly Complex One = new Complex(Real.One);
		public static readonly Complex MinusOne = new Complex(Real.MinusOne);
		public static readonly Complex I = new Complex(Real.Zero, Real.One);
		public static readonly Complex MinusI = new Complex(Real.Zero, Real.MinusOne);

		/// <summary>
		/// This is real part
		/// </summary>
		[Aliases("Real", "实部", "LowDimension", "低维")]
		public readonly Real Real = Real.Zero;
		/// <summary>
		/// This is imaginary part
		/// </summary>
		[Aliases("Imaginary", "虚部", "HighDimension", "高维")]
		public readonly Real Imaginary = Real.Zero;

		public virtual Infinite FlatValue => this.Real + Infinite.TheInfinite * this.Imaginary;

		public virtual Real Theta => new Real(Math.Atan2(this.Imaginary.Value, this.Real.Value));

		public virtual Real Length => new Real(Math.Sqrt(this.Real.Value * this.Real.Value + this.Imaginary.Value * this.Imaginary.Value));

		public Complex(Real Real = null, Real Imaginary = null)
		{
			this.Real = Real ?? this.Real;
			this.Imaginary = Imaginary ?? this.Imaginary;
		}
	}

	[Aliases("NaturalComplex", "自然复数")]
	public class NaturalComplex : StructuralNumber
	{
		public static readonly NaturalComplex Zero = new NaturalComplex();
		public static readonly NaturalComplex One = new NaturalComplex(Natural.One);
		public static readonly NaturalComplex J = new NaturalComplex(Natural.Zero, Natural.One);
		public static readonly NaturalComplex OneAndJ = new NaturalComplex(Natural.One, Natural.One);

		public static implicit operator Complex(NaturalComplex nc)
		{
			return nc == null ? null : new Complex(nc.Real, nc.Imaginary);
		}

		/// <summary>
		/// This is real part
		/// </summary>
		[Aliases("Real", "实部", "LowDimension", "低维")]
		public readonly Natural Real = Natural.Zero;
		/// <summary>
		/// This is imaginary part
		/// </summary>
		[Aliases("Imaginary", "虚部", "HighDimension", "高维")]
		public readonly Natural Imaginary = Natural.Zero;

		public virtual Infinite FlatValue => this.Real + Infinite.TheInfinite * this.Imaginary;

		public virtual Real Theta => new Real(Math.Atan2((double)this.Imaginary.Value, (double)this.Real.Value));
		
		public virtual Real Length => new Real(Math.Sqrt((double)this.Real.Value * (double)this.Real.Value + (double)this.Imaginary.Value * (double)this.Imaginary.Value));
		
		public NaturalComplex(Natural Real = null, Natural Imaginary = null)
		{
			this.Real = Real ?? this.Real;
			this.Imaginary = Imaginary ?? this.Imaginary;
		}
	}

	public delegate LinearNumber RotateOp(LinearNumber n);


	/// <summary>
	/// Infinite is a kind of Number
	/// </summary>
	[Aliases(
		"Infinite", "无穷", "无穷大"
	)]
	public class Infinite : LinearNumber
	{
		public new virtual bool IsPositive { get; set; } = true;

		public override bool IsZero => false;

		public static readonly Infinite TheInfinite = new Infinite();

		/// <summary>
		/// IOP is a rotate operation (function or operator)
		/// </summary>
		public static RotateOp IOP => (n) => (TheInfinite - Real.One) * n;

		/// <summary>
		/// I is the value represents the rotated 1.
		/// </summary>
		public static LinearNumber I1 = IOP(Real.One);

		/// <summary>
		/// I^2 is the value represents the rotated rotated 1, 
		/// this is -1.
		/// </summary>
		public static LinearNumber I2 = IOP(IOP(Real.One));

		public static Infinite operator +(Infinite infinite, LinearNumber n)
		{
			return infinite;
		}
		public static Infinite operator -(Infinite infinite, LinearNumber n)
		{
			return infinite;
		}
		public static Infinite operator *(Infinite infinite, LinearNumber n)
		{
			return infinite;
		}
		public Infinite(bool IsPositive = true)
		{
			this.IsPositive = IsPositive;
		}

		public static implicit operator Zero(Infinite infinite)
		{
			return new Zero(infinite.Existences.ToArray());
		}
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
			return infinity == null ? float.Epsilon : (infinity.IsPositive ? float.PositiveInfinity : float.NegativeInfinity);
		}

		public static implicit operator double(Infinite infinity)
		{
			return infinity == null ? double.Epsilon : (infinity.IsPositive ? double.PositiveInfinity : double.NegativeInfinity);
		}

		public static implicit operator decimal(Infinite infinity)
		{
			return infinity == null ? decimal.Zero : (infinity.IsPositive ? decimal.MaxValue : decimal.MinValue);
		}

		public Infinite(params Existence[] existences)
			: base(existences)
		{

		}
	}

	/// <summary>
	/// Zero is a kind of Number, but you can treat it as void
	/// </summary>
	[Aliases(
		"Zero", "0"
	)]
	public class Zero : LinearNumber
	{
		public new virtual bool IsPositive { get; set; } = true;
		
		public override bool IsZero => true;

		public Zero(bool IsPositive = true)
		{
			this.IsPositive = IsPositive;
		}

		public static readonly Zero TheZero = new Zero();

		public static implicit operator Infinite(Zero zero)
		{
			return new Infinite(zero?.Existences.ToArray());
		}

		public static implicit operator Zero(Void @void)
		{
			return new Zero(@void?.Existences.ToArray());
		}
		public static implicit operator Void(Zero zero)
		{
			return new Void(zero?.Existences.ToArray());
		}

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


		public Zero(params Existence[] existences)
			: base(existences)
		{

		}
	}
}