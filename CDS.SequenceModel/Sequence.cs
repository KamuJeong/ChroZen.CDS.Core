using CDS.Core;
using System.Diagnostics;

namespace CDS.SequenceModel
{
    public class Sequence : ModelBase, ISequence
    {
        public Sequence(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        private List<SequenceItem> _items = new List<SequenceItem>();

        public IEnumerable<ISequenceItem> Items => _items;

        public void Set(IEnumerable<SequenceItem> items) => _items = items.ToList();

        private IInstrument? Instrument => Parent?.FindChildren<IInstrument>(null).First();

        public bool Ready()
        {
            if(new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready }.Contains(Instrument.State.Status))
            {
                // find pause or reserved
                var item = Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Pause) ??
                               Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Reserved);
                if (item == null)
                    return false;

                Instrument.SetSequenceItem(item);

                Instrument.Ready();
                return true;
            }
            return false;
        }

        public bool Run()
        {
            if (new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready }.Contains(Instrument.State.Status))
            {
                // find pause or reserved
                var item = Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Reserved);
                if (item == null)
                    return false;


                Instrument.StatusChanged -= OnInstrumentStatusChanged;

                item.SetRunStatus();

                Instrument.SetSequenceItem(item);
                Instrument.PreRun();

                Instrument.StatusChanged += OnInstrumentStatusChanged;

                return true;
            }
            return false;
        }

        private void OnInstrumentStatusChanged(object? sender, InstrumentStatusChangedArgs oldStatus)
        {
            if(sender is IInstrument instrument)
            {
                if (instrument.State.Status == InstrumentStatus.NotReady)
                {
                    SequenceItem? item = GetCurrent() as SequenceItem;
                    if (item != null)
                    {
                        if (item.State.Counter > 0)
                            item.SetStopStatus();
                        else
                            item.Reset();

                        foreach (var it in Items.Where(i => new[] { SequenceStatus.Run, SequenceStatus.Pause }.Contains(i.State.Status)))
                            it.Reset();
                    }
                } 
                else if (instrument.State.Status == InstrumentStatus.PreRun)
                {
                    SequenceItem? item = GetNext() as SequenceItem;
                    if(item == null)
                    {
                        if (instrument.State.HaltAfterSequenceRun)
                            instrument.Halt();
                        else
                            instrument.Stop();
                    }
                    else if(item.State.Status == SequenceStatus.Pause)
                    {
                        instrument.SetSequenceItem(item);
                    }
                    else
                    {
                        item.SetRunStatus();
                        instrument.SetSequenceItem(item);
                    }
                }
                else if (instrument.State.Status == InstrumentStatus.Run && !instrument.State.IsSingleShot)
                {
                    Debug.Assert(instrument.State.SequenceItem == GetCurrent());

                    (GetCurrent() as SequenceItem)?.AddChromatogram(instrument.State.Chromatogram);
                }
            }
        }

        public ISequenceItem? GetNext()
        {
            var item = Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Run) as SequenceItem;
            if(item == null)
            {
                return Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Pause) ??
                           Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Reserved);
            }
            else if((item.Injection?.Times ?? 1) > item.State.Counter)
            {
                return item;
            }
            else
            {
                item.SetFinishedStatus();
                return GetNext();
            }           
        }

        public ISequenceItem? GetCurrent() => Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Run) ??
                                                Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Pause);

        public bool Pause()
        {
            bool ret = false;
            foreach(var s in  Items.Where(s => s.State.Status == SequenceStatus.Run)
                                    .Cast<SequenceItem>())
            {
                ret = true;
                s.SetPauseStatus();
            }
            return ret;        
        }

        public bool Resume()
        {
            bool ret = false;
            if (Instrument?.State.Status == InstrumentStatus.Pause)
            {
                foreach (var s in Items.Where(s => s.State.Status == SequenceStatus.Pause)
                                        .Cast<SequenceItem>())
                {
                    ret = true;
                    s.SetRunStatus();
                }
                if (ret)
                    Instrument.PreRun();
            }
            return ret;
        }
    }
}