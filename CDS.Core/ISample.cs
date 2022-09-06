namespace CDS.Core
{
    public enum SampleTypes
    {
        Unknown,
        Standard,
        Test,
    }

    public interface ISample
    {
        string Name { get; set; }
        string ID { get; set; }
        SampleTypes Type { get; set; }
    }
}