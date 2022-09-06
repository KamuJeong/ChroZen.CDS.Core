using CDS.Core;
using CDS.InstrumentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CDS.SequenceModel.Tests
{
    [TestClass]
    public class SeqeunceTests
    {
        private ModelBase? root;
        private Instrument? instrument;
        private Sequence? sequence;

        public TestContext? TestContext { get; set; }

        [TestInitialize]
        public void CreateInstrument()
        {
            root = new ModelBase(null, "root");
            instrument = new Instrument(root, "instrument");
            instrument.StatusChanged += (s, e) =>
            {
                if (s is Instrument inst)
                {
                    TestContext?.WriteLine($"{inst.Name}: {inst.State.Status}");
                    if (inst.State.Status == InstrumentStatus.Run)
                    {
                        TestContext?.WriteLine($">> {sequence?.GetCurrent()?.SampleID ?? "Unknown"} <<");
                    }
                }
                else if (s is Device dev)
                {
                    TestContext?.WriteLine($"\t{dev.Name}: {dev.State.Status}");
                }
            };
            new RunableDevice(instrument, "RunnableFor1sec");
            new PrerunableDevice(instrument, "Prerunable");

            sequence = new Sequence(root, "sequence");
        }

        [TestMethod]
        public void MakeUpSequenceTable()
        {
            sequence?.Set(new SequenceItem[] {
                            new SequenceItem { SampleID = "Sample01", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample02", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample03", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample04", SampleName = "Test" },
                        });
            Assert.AreEqual(4, sequence?.Items.Count());
        }

        [TestMethod]
        public void RunnableDeviceTest()
        {
            instrument?.ConnectAsync().Wait();
            instrument?.Ready();
            instrument?.Run();
            Thread.Sleep(500);

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

        [TestMethod]
        public void SequenceRun()
        {
            sequence?.Set(new SequenceItem[] {
                            new SequenceItem { SampleID = "Sample01", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample02", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample03", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample04", SampleName = "Test" },
                        });

            instrument?.ConnectAsync().Wait();
            sequence?.Ready();
            sequence?.Run();

            Thread.Sleep(1500);

            Assert.IsTrue(sequence?.Items.All(i => i.State.Status == SequenceStatus.Finished));
            Assert.IsTrue(sequence?.Items.All(i => i.State.Counter == 1));

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

        [TestMethod]
        public void SequenceRunWithErrors()
        {
            instrument?.FindChildren<PrerunableDevice>(null).First()?.Delete();
            new OddSkipableDevice(instrument, "OddSkipable");

            sequence?.Set(new SequenceItem[] {
                            new SequenceItem { SampleID = "Sample01", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample02", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample03", SampleName = "Test" },
                            new SequenceItem { SampleID = "Sample04", SampleName = "Test" },
                        });

            instrument?.ConnectAsync().Wait();
            sequence?.Ready();
            sequence?.Run();

            Thread.Sleep(1000);

            Assert.AreEqual(SequenceStatus.Error, sequence?.Items.ElementAt(0).State.Status);
            Assert.AreEqual(SequenceStatus.Finished, sequence?.Items.ElementAt(1).State.Status);
            Assert.AreEqual(SequenceStatus.Error, sequence?.Items.ElementAt(2).State.Status);
            Assert.AreEqual(SequenceStatus.Finished, sequence?.Items.ElementAt(3).State.Status);

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

    }
}