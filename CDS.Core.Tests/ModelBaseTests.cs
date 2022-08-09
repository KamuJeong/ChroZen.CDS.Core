using Microsoft.VisualStudio.TestTools.UnitTesting;
using CDS.Core;
using System.Linq;

namespace CDS.Core.Tests
{
    [TestClass]
    public class ModelBaseTests
    {
        private ModelBase? root;

        [TestInitialize]
        public void CreateRoot()
        {
            root = new ModelBase(null, "root");
        }

        [TestMethod]
        public void AddChild()
        {
            var child = new ModelBase(root, "first child");

            Assert.AreSame(root, child.Parent);
            Assert.AreEqual(1, root.Children.Count());
            Assert.AreSame(root.Children.First(), child);
        }

        [TestMethod]
        public void AddGrandChild()
        {
            var child = new ModelBase(root, "first child");
            var grand = new ModelBase(child, "first grand child");

            Assert.AreSame(child, grand.Parent);
            Assert.AreEqual(1, child.Children.Count());
            Assert.AreSame(child.Children.First(), grand);
        }

        [TestMethod]
        public void ChangeParent()
        {
            var child = new ModelBase(root, "first child");
            var grand = new ModelBase(child, "first grand child");
            grand.ChangeParent(root);

            Assert.AreSame(root, grand.Parent);
            Assert.IsFalse(child.Children.Any());
            Assert.AreEqual(2, root.Children.Count());
            Assert.AreSame(grand, root.FindChildren("first grand child").First());
        }

        [TestMethod]
        public void DeleteChild()
        {
            var child = new ModelBase(root, "first child");
            var grand = new ModelBase(child, "first grand child");
            child.Delete();

            Assert.IsFalse(root.Children.Any());
        }

        [TestMethod]
        public void FindChildRecursively()
        {
            var child = new ModelBase(root, "first child");
            var grand = new ModelBase(child, "first grand child");

            Assert.IsFalse(root.FindChildren("first grand child").Any());
            Assert.AreSame(grand, root.FindChildrenRecursively("first grand child").First());
        }

        [TestMethod]
        public void GetAllDescendentModelBases()
        {
            var child = new ModelBase(root, "first child");
            var grand = new ModelBase(child, "first grand child");

            var models = root.FindChildrenRecursively<ModelBase>(null);
            Assert.AreEqual(2, models.Count());
            Assert.IsFalse (models.Except(new[] { child, grand }).Any());
        }

        [TestCleanup]
        public void DeleteRoot()
        {
            root.Delete();
        }
    }
}