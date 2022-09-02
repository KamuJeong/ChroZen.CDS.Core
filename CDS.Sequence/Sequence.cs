using CDS.Core;

namespace CDS.Sequence
{
    public class Sequence : ModelBase, ISequence
    {
        public Sequence(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        public IEnumerable<ISequenceItem> Items { get; } = new List<SequenceItem>();

        private IInstrument Instrument => Parent.FindChildren<IInstrument>(null).First();

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
                var item = Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Pause) ??
                               Items.FirstOrDefault(s => s.State.Status == SequenceStatus.Reserved);
                if (item == null)
                    return false;

                Instrument.SetSequenceItem(item);


                return true;
            }
            return false;
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}