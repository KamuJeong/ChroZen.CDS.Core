﻿namespace CDS.Core.Model
{
    public interface ISequence
    {
        IEnumerable<ISequenceItem> Items { get; }

        bool Ready();
        bool Run();
        bool Stop();
    }
}