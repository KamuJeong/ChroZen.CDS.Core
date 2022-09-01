using Microsoft.VisualStudio.TestTools.UnitTesting;
using CDS.Core;

namespace CDS.Instrument.Tests
{
    [TestClass]
    public class InstrumentTests
    {
        private ModelBase? root;
        private Instrument? instrument;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void CreateInstrument()
        {
            root = new ModelBase(null, "root");
            instrument = new(root, "instrument");
            instrument.StatusChanged += (s, e) =>
            {
                if (s is Instrument inst)
                    TestContext.WriteLine($"{inst.Name}: {inst.State.Status}");
                else if (s is Device dev)
                    TestContext.WriteLine($"{dev.Name}: {dev.State.Status}");
            };

            new DemoNotReadyDevice(instrument, "NotReadyDevice");
            new DemoReadyDevice(instrument, "ReadyDevice");
            new DemoPreRunDevice(instrument, "PreRunDevice");
        }

        [TestMethod]
        public void ConnectInstrument()
        {
            instrument.ConnectAsync().Wait();
           
            Assert.AreEqual(InstrumentStatus.NotReady, instrument.State.Status);
            CollectionAssert.AreEqual(instrument.Devices.Select(d => d.State.Status).ToArray(), 
                instrument.Devices.Select(d => DeviceStatus.NotReady).ToArray());
        }

        [TestMethod]
        public void ReadyInstrument()
        {
            instrument.ConnectAsync().Wait();
            instrument.Ready();
            Assert.AreEqual(InstrumentStatus.NotReady, instrument.State.Status);
            Assert.AreEqual(DeviceStatus.Ready, instrument.FindChildren<Device>("ReadyDevice").First().State.Status);
        }

        [TestMethod]
        public void PreRunInstrument()
        {
            instrument.ConnectAsync().Wait();

            TestContext.WriteLine($">> sequence <<");
            
            instrument.Ready();
            instrument.PreRun();
            Assert.AreEqual(InstrumentStatus.PreRun, instrument.State.Status);
            Assert.AreEqual(DeviceStatus.PreRun, instrument.FindChildren<Device>("PreRunDevice").First().State.Status);
            instrument.Stop();

            TestContext.WriteLine($">> no more <<");

            instrument.StatusChanged += (s, e) =>
            {
                if (s is Instrument inst && inst.State.Status == InstrumentStatus.PreRun)
                {
                    inst.Stop();
                }
            };
            instrument.Ready();
            instrument.PreRun();
            Assert.AreEqual(InstrumentStatus.NotReady, instrument.State.Status);
            Assert.AreEqual(DeviceStatus.NotReady, instrument.FindChildren<Device>("PreRunDevice").First().State.Status);
        }

    }
}