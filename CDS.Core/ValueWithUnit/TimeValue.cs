using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public class TimeValue : ValueWithUnit
    {
        private static Dictionary<string, double> Units = new()
        {
            ["hour"] = 1.0,
            ["h"] = 1.0,
            ["min"] = 60.0,
            ["m"] = 60.0,
            ["sec"] = 3_600.0,
            ["s"] = 3_600.0,
            ["msec"] = 3_600_000.0,
            ["ms"] = 3_600_000.0,
        };

        public TimeValue(string input) : base(input)
        {
            if (!Units.Keys.Contains(Unit))
                throw new ArgumentException("Invalid unit");
        }

        public TimeValue(double value, string? unit) : base(value, unit)
        {
            if (!Units.Keys.Contains(Unit))
                throw new ArgumentException("Invalid unit");
        }

        public override ValueWithUnit Convert(string? unit)
        {
            unit ??= "hour";

            if (Unit == unit)
                return this;

            if (!Units.ContainsKey(unit))
                throw new ArgumentException("Invalid unit");

            return new TimeValue(Value / Units[Unit] * Units[unit], unit);
        }

        public override string ToString(string? fmt)
        {
            return Value.ToString(fmt) + (string.IsNullOrWhiteSpace(Unit) ? String.Empty : Unit);
        }
    }
}
