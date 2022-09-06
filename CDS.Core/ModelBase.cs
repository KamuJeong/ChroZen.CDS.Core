﻿namespace CDS.Core
{
    public class ModelBase
    {
        public ModelBase(ModelBase? parent, string? name)
        {
            Parent = parent;
            Name = name;

            if(parent != null)
            {
                parent.children.Add(this);
            }
        }

        public string? Name { get; set; }

        public virtual string? QualifiedName => Name;

        public ModelBase? Parent { get; private set; }

        public ModelBase? Root
        {
            get
            {
                if (Parent == null)
                    return this;
                return Parent.Root;
            }
        }

        public void ChangeParent(ModelBase newParent)
        {
            if (newParent == Parent)
                return;

            if (Parent != null)
            {
                Parent.children.Remove(this);
            }
            Parent = newParent;
            newParent.children.Add(this);
        }

        public void Delete()
        {
            if (Parent != null)
            {
                Parent.children.Remove(this);
            }
            Parent = null;

            foreach (var child in Children.ToArray())
            {
                child.Delete();
            }
        }

        protected List<ModelBase> children = new List<ModelBase>();

        public IEnumerable<ModelBase> Children => children;

        public IEnumerable<ModelBase> FindChildren(string name)
        {
            foreach (var child in children)
            {
                if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<ModelBase> FindChildrenRecursively(string name)
        {
            foreach (var child in children)
            {
                if (string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    yield return child;
                }
                foreach (var grand in child.FindChildren(name))
                {
                    yield return grand;
                }
            }
        }

        public IEnumerable<T> FindChildren<T>(string? name)  where T : class
        {
            foreach (var child in children)
            {
                if (child is T t && (name == null || string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)))
                {
                    yield return t;
                }
            }
        }

        public IEnumerable<T> FindChildrenRecursively<T>(string? name) where T : class
        {
            foreach (var child in children)
            {
                if (child is T t &&  (name == null  || string.Equals(child.Name, name, StringComparison.OrdinalIgnoreCase)))
                {
                    yield return t;
                }
                foreach (var grand in child.FindChildrenRecursively<T>(name))
                {
                    yield return grand;
                }
            }
        }

    }
}