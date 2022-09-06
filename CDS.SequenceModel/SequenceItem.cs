using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.SequenceModel
{
    public class SequenceItem : ISequenceItem
    {
        // State
        private SequenceState _state =  new SequenceState();
        public ISequenceState State => _state;

        public void Reset() => _state.Reset();

        public void SetRunStatus()
        {
            if (new[] { SequenceStatus.Reserved, SequenceStatus.Pause }.Contains(_state.Status))
                _state.Status = SequenceStatus.Run;
        }

        public void SetErrorStatus()
        { 
            _state.Status = SequenceStatus.Error;
        }

        public void SetPauseStatus() => _state.Status = SequenceStatus.Pause;
        public void SetStopStatus() => _state.Status = SequenceStatus.Stop;
        public void SetFinishedStatus() => _state.Status = SequenceStatus.Finished;

        public void AddChromatogram(IChromatogram? chromatogram) => _state.AddChromatogram(chromatogram);

        public bool IsSkipped()
        {
            return _state.Status == SequenceStatus.Error;
        }

        // Sample
        public string? SampleName { get; set; }
        public string? SampleID { get; set; }
        public SampleTypes SampleType { get; set; }

        // Injection
        public IInjection? Injection { get; set; }

        // Method
        public IMethod? Method { get; set; }

        // Project
        public IProject? Project { get; set; }

    }
}
