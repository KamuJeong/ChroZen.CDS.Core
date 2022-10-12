using Microsoft.VisualStudio.TestTools.UnitTesting;
using CDS.Core;
using System.Reflection;

namespace CDS.InstrumentModel.Tests
{
    [TestClass]
    public class InstrumentTests
    {
        private ModelBase? root;
        private Instrument? instrument;

        public TestContext? TestContext { get; set; }

        [TestInitialize]
        public void CreateInstrument()
        {
            root = new ModelBase(null, "root");
            instrument = new(root, "instrument");
            instrument.StatusChanged += (s, e) =>
            {
                if (s is Instrument inst)
                    TestContext?.WriteLine($"{inst.Name}: {inst.State.Status}");
                else if (s is Device dev)
                    TestContext?.WriteLine($"\t{dev.Name}: {dev.Status}");
            };

            new DemoNotReadyDevice(instrument, "NotReadyDevice");
            new DemoReadyDevice(instrument, "ReadyDevice");
            new DemoPreRunDevice(instrument, "PreRunDevice");
            new DemoRunDevice(instrument, "RunDevice");
        }

        [TestMethod]
        public void ConnectInstrument()
        {
            instrument?.ConnectAsync().Wait();

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
            CollectionAssert.AreEqual(instrument?.Devices.Select(d => d.Status).ToArray(),
                                        instrument?.Devices.Select(d => DeviceStatus.NotReady).ToArray());
        }

        [TestMethod]
        public void ReadyInstrument()
        {
            instrument?.ConnectAsync().Wait();
            instrument?.Ready();
            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.Ready, instrument?.FindChildren<Device>("ReadyDevice").First().Status);
        }

        [TestMethod]
        public void PreRunInstrument()
        {
            instrument?.ConnectAsync().Wait();

            TestContext?.WriteLine($">> sequence <<");

            instrument?.Ready();
            instrument?.PreRun();
            Assert.AreEqual(InstrumentStatus.PreRun, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.PreRun, instrument?.FindChildren<Device>("PreRunDevice").First().Status);
            instrument?.Stop();

            TestContext?.WriteLine($">> no more <<");

            if (instrument != null)
            {
                instrument.StatusChanged += (s, e) =>
                {
                    if (s is Instrument inst && inst.State.Status == InstrumentStatus.PreRun)
                    {
                        inst.Stop();
                    }
                };
            }
            instrument?.Ready();
            instrument?.PreRun();
            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.NotReady, instrument?.FindChildren<Device>("PreRunDevice").First().Status);
        }

        [TestMethod]
        public void RunInstrument()
        {
            instrument?.ConnectAsync().Wait();

            TestContext?.WriteLine(">> single run <<");

            instrument?.Ready();
            instrument?.Run();
            Assert.AreEqual(InstrumentStatus.Run, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.Run, instrument?.FindChildren<Device>("RunDevice").First().Status);

            instrument?.FindChildren<DemoRunDevice>(null).All(d => { d.StopRun(); return true; });
            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);

            TestContext?.WriteLine(">> sequence run <<");
            instrument?.Ready();
            instrument?.PreRun();
            instrument?.Run();
            Assert.AreEqual(InstrumentStatus.Run, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.Run, instrument?.FindChildren<Device>("RunDevice").First().Status);
            instrument?.FindChildren<DemoRunDevice>(null).All(d => { d.StopRun(); return true; });
            Assert.AreEqual(InstrumentStatus.PreRun, instrument?.State.Status);
        }

        [TestMethod]
        public void PostRunInstrument()
        {
            new DemoPostRunDevice(instrument, "PostRunDevice");

            instrument?.ConnectAsync().Wait();

            instrument?.Ready();
            instrument?.Run();
            instrument?.FindChildren<DemoRunDevice>(null).All(d => { d.StopRun(); return true; });

            Assert.AreEqual(InstrumentStatus.PostRun, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.PostRun, instrument?.FindChildren<Device>("PostRunDevice").First().Status);

            instrument?.FindChildren<DemoPostRunDevice>(null).All(d => { d.StopPostRun(); return true; });

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

        [TestMethod]
        public void PostWorkInstrument()
        {
            new DemoPostWorkDevice(instrument, "PostWorkDevice");

            instrument?.ConnectAsync().Wait();

            instrument?.Ready();
            instrument?.Run();
            instrument?.FindChildren<DemoRunDevice>(null).All(d => { d.StopRun(); return true; });

            Assert.AreEqual(InstrumentStatus.PostWork, instrument?.State.Status);
            Assert.AreEqual(DeviceStatus.PostWork, instrument?.FindChildren<Device>("PostWorkDevice").First().Status);

            instrument?.FindChildren<DemoPostWorkDevice>(null).All(d => { d.StopPostWork(); return true; });

            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

        [TestMethod]
        public void TimerTickCallback()
        {
            var run = instrument?.FindChildren<DemoRunDevice>("RunDevice").FirstOrDefault();
            if (run != null)
                run.TimerTick += (s, ts) =>
                {
                    if (instrument?.State.Status == InstrumentStatus.Run && instrument?.State.ElapsedTime >= TimeSpan.FromSeconds(1.0))
                    {
                        TestContext?.WriteLine($">> Stop : {instrument?.State.ElapsedTime}");
                        run.StopRun();
                    }
                };

            instrument?.ConnectAsync().Wait();

            instrument?.Ready();
            instrument?.Run();
            Assert.AreEqual(InstrumentStatus.Run, instrument?.State.Status);

            Thread.Sleep(1500);
            Assert.AreEqual(InstrumentStatus.NotReady, instrument?.State.Status);
        }

        [TestMethod]
        public void ReferAttributeTest()
        {
            var ready = instrument?.FindChildren<Device>("ReadyDevice").FirstOrDefault();
            foreach(var attr in ready!.GetType().GetCustomAttributes<ReferAttribute>())
            {
                TestContext?.WriteLine($"{attr.Key}: {attr.Type}");
            }
        }
    }
}