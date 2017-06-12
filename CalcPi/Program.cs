using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CalcPi
{
    class Program
    {
        double Zeta(double s = 0.0, ulong max = 100, ulong n = 1)
        {
            double sum = 0.0;
            for (ulong t = n; t <= max; t++)
            {
                sum += Math.Pow(t, -s);
            }
            return sum;
        }
        /// <summary>
        /// Pi = 2 + (1/3)(2+(2/5)(2+(3/7(...))))
        /// Pi(n) = 2 + (n/(2*n+1))Pi(n+1) (n&lt;=max)
        /// Pi(max) = 2*max + 1;
        /// </summary>
        /// <param name="n"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        double Pi(ulong n = 1, ulong max = 100)
        {
            return n < max
                ? 2.0 + (1.0 * n / (2.0 * n + 1.0) * Pi(n + 1, max))
                : 2.0 * max + 1.0;
        }
        double PiX(ulong n = 1, ulong max = 100)
        {
            return n < max
                ? 2.0 + 1.0 / 2.0 * PiX(n + 1, max)
                : 4.0;
        }
        double PiY(ulong n = 1, ulong max = 100, ulong m = 10000)
        {
            if (m == 0)
                throw new System.ArgumentOutOfRangeException(nameof(m));
            return n < max
                ? 2.0 + 1.0 * m / (2.0 * m + 1.0) * PiY(n + 1, max, m)
                : (2.0 * m + 1.0) / m;
        }
        double E(ulong n = 1, ulong max = 100)
        {
            return n == 0
                ? 1.0
                : n < max
                ? 1.0 + (1.0 / n) * E(n + 1, max)
                : 1.0;
        }
        double OneLess(ulong n = 0, ulong max = 100, ulong m = 10000)
        {
            if (m == 0)
                throw new System.ArgumentOutOfRangeException(nameof(m));

            if (n < max)
            {
                return 1.0 - OneLess(n + 1, max, m) / m;
            }
            return 1.0;
        }
        double OneMore(ulong n = 0, ulong max = 100, ulong m = 10000)
        {
            if (m == 0)
                throw new System.ArgumentOutOfRangeException(nameof(m));

            if (n < max)
            {
                return 1.0 + OneMore(n + 1, max, m) / m;
            }
            return 1.0;
        }
        double E(ulong n = 1, ulong max = 100, double x = 1.0)
        {
            return n == 0 || n >= max
                ? 1.0
                : 1.0 + x / n * E(n + 1, max, x);
        }
        double Em(ulong n = 1, ulong max = 100, double x = 1.0, ulong m = 10000)
        {
            if (m == 0)
                throw new System.ArgumentOutOfRangeException(nameof(m));

            return n == 0 || n >= max
                ? 1.0
                : 1.0 + x / m * Em(n + 1, max, x, m);
        }
        void DoCalc()
        {
            Trace.WriteLine(Pi(1, 100));
            Trace.WriteLine(E(1, 100));
            Trace.WriteLine(OneLess(0, 100, 100));
            Trace.WriteLine(OneMore(0, 100, 100));

            N n1 = (P)1 * (P)2 * (P)3 * (P)5 * (P)7;
            N n2 = (P)(2, 3) * (3, 5);

            Console.WriteLine("n1= {0}", n1);
            Console.WriteLine("n2 = {0}", n2);

            double s = 0.0;

            Trace.WriteLine($"Zeta({s=1.0}) = " + Zeta(s,10000));

            Trace.WriteLine($"Zeta({s=0.5}) = " + Zeta(s, 10000));

            Trace.WriteLine($"Zeta({s=0.0}) = " + Zeta(s, 10000));

            Trace.WriteLine($"Zeta({s=-1.0}) = " + Zeta(s, 10000));

        }
        void PrintPi(int c = 8400)
        {
            int[] f = new int[c + 1];

            int a = 10000, b = 0, d = 0, e = 0, g = 0;

            for (b = e = 0; b != c;) f[b++] = a / 5;

            for (; (g = c * 2) != 0; c -= 14)
            {
                for (d = 0, b = c; --b != 0; d *= b)
                {
                    d += f[b] * a;
                    f[b] = d % --g;
                    d /= g--;
                }
                Console.Write("{0:4}", e + d / a);
                e = d % a;
            }
        }

        class N
        {
            public static readonly N One = new N();

            public static implicit operator N(int i)
            {
                return new P(i);
            }
            public static implicit operator N(long i)
            {
                return new P(i);
            }
            public static implicit operator N(BigInteger i)
            {
                return new P(i);
            }
            public static implicit operator N(P p)
            {
                return new N(p);
            }

            public static implicit operator BigInteger(N n)
            {
                return n.Value;
            }
            public static implicit operator N((BigInteger i, BigInteger n) v)
            {
                return new P(v.i, v.n);
            }
            public static N operator *(N n, P p)
            {
                return n.AppendFactor(p);
            }

            public List<P> Factors = new List<P>();

            public BigInteger Value
            {
                get
                {
                    BigInteger r = BigInteger.One;
                    foreach (P p in this.Factors)
                    {
                        r *= p.Value;
                    }
                    return r;
                }
            }
            public N(params P[] factors)
            {
                if (factors == null) throw new ArgumentNullException(nameof(factors));

                this.Factors.AddRange(factors);
            }

            public N AppendFactor(P p)
            {
                if (p == null) throw new ArgumentNullException(nameof(p));

                this.Factors.Add(p);

                return this;
            }

            public override string ToString()
            {
                return this.Factors.Count == 0
                    ? BigInteger.One.ToString()
                    : string.Join(" * ", this.Factors.Select(p => p.ToString()));
            }
            public override bool Equals(object obj)
            {
                return obj is N n ? this.Value == n.Value : base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }
        }

        class P
        {
            public static implicit operator BigInteger(P p)
            {
                return p.Value;
            }
            public static implicit operator P((BigInteger i, BigInteger n) v)
            {
                return new P(v.i, v.n);
            }

            public static implicit operator P(int i)
            {
                return new P(i);
            }
            public static implicit operator P(long i)
            {
                return new P(i);
            }
            public static implicit operator P(BigInteger i)
            {
                return new P(i);
            }
            public static N operator *((BigInteger i, BigInteger n) v, P right)
            {
                return (P)v * right;
            }

            public static N operator *(P left, P right)
            {
                return new N(left, right);
            }

            public BigInteger Value
            {
                get { return FullRangePow(p, n); }
            }

            public BigInteger p = BigInteger.One;
            public N n = N.One;

            public static BigInteger FullRangePow(BigInteger p, BigInteger n)
            {
                if (p < 0) throw new ArgumentOutOfRangeException(nameof(p));
                if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

                BigInteger c = BigInteger.One;

                if (n > int.MaxValue)
                {
                    BigInteger pm = BigInteger.Pow(p, int.MaxValue);

                    for (; n > int.MaxValue; n -= int.MaxValue)
                    {
                        c *= pm;
                    }
                }
                return c * BigInteger.Pow(p, (int)n);
            }

            public P() : this(BigInteger.One) { }
            public P(BigInteger p, N n = null)
            {
                this.p = p;
                this.n = n ?? new N();
            }
            public override string ToString()
            {
                return "[" + this.p.ToString() + " ^ (" + this.n.ToString() + ")]";
            }
            public override bool Equals(object obj)
            {
                return obj is P p ? this.Value == p.Value : base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }
        }

        const double c = 299792458;//m/s
        static double CalcT(double v, double t)
        {
            return Math.Sqrt(1.0 - Math.Pow(v / c, 2.0));
        }

        static Complex CalcTComplex(double v, double t)
        {
            return Complex.Sqrt(1.0 - (v / c));
        }

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            new Program().DoCalc();

        }
    }
}
