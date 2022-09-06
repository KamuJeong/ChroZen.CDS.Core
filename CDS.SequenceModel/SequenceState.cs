using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.SequenceModel
{
    public class SequenceState : ISequenceState
    {
        public SequenceStatus Status { get; set; }
        public int Counter => Chromatograms.Count();

        private List<IChromatogram?> _chromatogram = new();
        public IEnumerable<IChromatogram?> Chromatograms => _chromatogram;

        public void AddChromatogram(IChromatogram? chromatogram) => _chromatogram.Add(chromatogram);
        public void Reset()
        {
            Status = SequenceStatus.Reserved;
            _chromatogram.Clear();
        }
    }
}
