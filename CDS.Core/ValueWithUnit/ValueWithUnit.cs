using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CDS.Core
{
    public class ValueWithUnit : IEquatable<ValueWithUnit>, IComparable<ValueWithUnit>
    {
        public double Value { get; init; }
        public string? Unit { get; private set; }

        public ValueWithUnit(double value, string? unit)
        {
            Value = value;
            Unit = unit?.Trim();
            if (Unit == "")
                Unit = null;
        }

        public ValueWithUnit(string input)
        {
            string pattern = @"(.*\d+)\s?(\D+)\s?$";
            var match = Regex.Match(input, pattern);
            if (match.Success && match.Groups.Count > 2)
            {
                Value = double.Parse(match.Groups[1].Value.Trim());
                Unit = match.Groups[2].Value.Trim();
                if (Unit == "")
                    Unit = null;
            }
            else
            {
                Value = double.Parse(input.Trim());
                Unit = null;
            }
        }

        public void Deconstruct(out double value, out string? unit)
        {
            value = Value;
            unit = Unit;
        }

        public virtual ValueWithUnit Convert(string? unit)
        {
            return new ValueWithUnit(Value, unit);
        }

        public override string ToString() => ToString(null);

        public virtual string ToString(string? fmt)
        {
            return Value.ToString(fmt) + (string.IsNullOrWhiteSpace(Unit)? String.Empty : " " + Unit);
        }

        public override bool Equals(object? obj) => Equals(obj as ValueWithUnit);

        public bool Equals(ValueWithUnit? other)
        {
            if (other == null)
                return false;

            var v1 = Convert(null);
            var v2 = other.Convert(null);

            return v1.Value == v2.Value && string.Equals(v1.Unit, v2.Unit);
        }

        public int CompareTo(ValueWithUnit? other)
        {
            if(other == null)
                throw new ArgumentNullException(nameof(other));

            var v1 = Convert(null);
            var v2 = other.Convert(null);

            return Comparer<double>.Default.Compare(v1.Value, v2.Value);
        }

        public override int GetHashCode()
        {
            return Convert(null).ToString().GetHashCode();
        }
    }
}
