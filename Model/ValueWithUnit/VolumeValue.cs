using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core.Model
{
    public class VolumeValue : ValueWithUnit
    {
        private static Dictionary<string, double> VolumeUnits = new()
        { 
             ["L"] = 1.0,
             ["mL"] = 1_000.0,
             ["μL"] = 1_000_000.0,
        };

        public VolumeValue(string input) : base(input)
        {
            if(!VolumeUnits.Keys.Contains(Unit))
                throw new ArgumentException("Invalid unit");
        }

        public VolumeValue(double value, string? unit) : base(value, unit)
        {
            if (!VolumeUnits.Keys.Contains(Unit))
                throw new ArgumentException("Invalid unit");
        }

        public override ValueWithUnit Convert(string? unit)
        {
            unit ??= "L";

            if (Unit == unit)
                return this;

            if (!VolumeUnits.ContainsKey(unit))
                throw new ArgumentException("Invalid unit");

            return new VolumeValue(Value / VolumeUnits[Unit] * VolumeUnits[unit], unit);
        }
    }
}
