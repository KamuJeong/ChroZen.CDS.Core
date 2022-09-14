namespace CDS.Core
{
    public interface IMethod : ICloneable
    {
        object GetMethod(IDevice device);
        void SetMethod(IDevice device, object value);
    }
}