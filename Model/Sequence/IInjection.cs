namespace CDS.Core.Model
{
    public interface IInjection
    {
        string? Vial { get; set; }
        int Times { get; set; }
        VolumeValue Volume { get; set; }
    }
}