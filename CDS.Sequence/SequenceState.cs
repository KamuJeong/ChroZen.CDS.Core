using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Sequence
{
    public class SequenceState : ISequenceState
    {
        public SequenceStatus Status { get; set; }
        public int Counter { get; set; }

        public IEnumerable<IChromatogram> Chromatograms { get; } = new List<IChromatogram>();
    }
}
