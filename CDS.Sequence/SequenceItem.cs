using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Sequence
{
    public class SequenceItem : ISequenceItem
    {
        private SequenceState _state =  new SequenceState();
        public ISequenceState State => _state;

        public void Reset() => _state.Status = SequenceStatus.Reserved;

        public void SetRunStatus()
        {
            if (new[] { SequenceStatus.Reserved, SequenceStatus.Pause }.Contains(_state.Status))
                _state.Status = SequenceStatus.Run;
        }

        public void SetErrorStatus()
        { 
            _state.Status = SequenceStatus.Error;
        }

        internal void SetPauseStatus() => _state.Status = SequenceStatus.Pause;
        internal void SetStopStatus() => _state.Status = SequenceStatus.Stop;
        internal void SetFinishedStatus() => _state.Status = SequenceStatus.Finished;


        public ISample? Sample { get; set; }

        public IInjection? Injection { get; set; }

        public IMethod? Method { get; set; }

        public IProject? Project { get; set; }
    }
}
