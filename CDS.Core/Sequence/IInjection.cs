namespace CDS.Core
{
    public interface IInjection
    {
        string? Vial { get; set; }
        int Times { get; set; }
        VolumeValue? Volume { get; set; }
    }
}