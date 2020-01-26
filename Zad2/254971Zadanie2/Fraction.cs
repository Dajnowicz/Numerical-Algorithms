using System;
using System.Numerics;

namespace GaussLinear
{
    public class Fraction
    {
        public BigInteger Numerator;
        private BigInteger _denominator;
        public Fraction(BigInteger n, BigInteger d)
        {
            Numerator = n;
            _denominator = d;
        }
        public static Fraction operator +(Fraction a, Fraction b)
        {
            var f = (a._denominator == b._denominator)
                ? new Fraction((a.Numerator + b.Numerator), a._denominator)
                : new Fraction((a.Numerator * b._denominator) + (b.Numerator * a._denominator), (a._denominator * b._denominator));
            return f.Fix();
        }
        public static Fraction operator -(Fraction a, Fraction b)
        {
            var f = (a._denominator == b._denominator)
                ? new Fraction((a.Numerator - b.Numerator), a._denominator)
                : new Fraction((a.Numerator * b._denominator) - (b.Numerator * a._denominator), (a._denominator * b._denominator));
            return f.Fix();
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            var f = new Fraction((a.Numerator * b.Numerator), (a._denominator * b._denominator));
            return f.Fix();
        }
        public static Fraction operator /(Fraction a, Fraction b)
        {
            var f = new Fraction((a.Numerator * b._denominator), (a._denominator * b.Numerator));
            return f.Fix();
        }
        public static bool operator <(Fraction a, Fraction b)
        {
            return a._denominator == b._denominator
                ? a.Numerator < b.Numerator ? true : false
                : (a.Numerator * b._denominator) < (b.Numerator * a._denominator) ? true : false;
        }
        public static bool operator >(Fraction a, Fraction b)
        {
            return a._denominator == b._denominator
                ? a.Numerator > b.Numerator ? true : false
                : (a.Numerator * b._denominator) > (b.Numerator * a._denominator) ? true : false;
        }
        public static bool operator ==(Fraction a, Fraction b)
        {
            a.Fix();
            b.Fix();
            return a.Numerator == b.Numerator ? true : false;
        }
        public static bool operator !=(Fraction a, Fraction b)
        {
            return a._denominator == b._denominator
                ? a.Numerator != b.Numerator ? true : false
                : (a.Numerator * b._denominator) != (b.Numerator * a._denominator) ? true : false;
        }
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var second = obj as Fraction;

            return second != null && Numerator == second.Numerator;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static implicit operator Fraction(float x)
        {
            var f = new Fraction((BigInteger)(x * (float)(65536)), (BigInteger)(65536));
            return f.Fix();
        }
        public static implicit operator double(Fraction f)
        {
            var num = (double)f.Numerator;
            var den = (double)f._denominator;
            return num / den;
        }
        public Fraction Fix()
        {
            var a = Numerator < 0 ? -Numerator : Numerator;
            var b = _denominator < 0 ? -_denominator : _denominator;
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            var d = (a == 0 ? b : a);
            if (d > 1)
            {
                Numerator /= d;
                _denominator /= d;
            }
            if (_denominator < 0)
            {
                Numerator *= -1;
                _denominator *= -1;
            }
            return this;
        }
        public override string ToString()
        {
            return Numerator.ToString() + "/" + _denominator.ToString();
        }
    }
}
