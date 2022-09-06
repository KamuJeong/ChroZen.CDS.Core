using CDS.Core;
using CDS.InstrumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.SequenceModel.Tests
{
    internal class OddSkipableDevice : PrerunableDevice
    {
        public OddSkipableDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        protected override void OnTimerTick(TimeSpan ts)
        {
            if (Parent is Instrument instrument && State.Status == DeviceStatus.PreRun)
            {
                int rank = 1;
                foreach (var it in Root.FindChildren<ISequence>(null).First().Items)
                {
                    if (it == instrument.State.SequenceItem)
                        break;

                    rank++;
                }

                if ((rank & 1) == 1)
                {
                    instrument.State.SequenceItem?.SetErrorStatus();
                    instrument.PreRun();
                }
                else
                {
                    instrument.Run();
                }
            }
        }
    }
}
