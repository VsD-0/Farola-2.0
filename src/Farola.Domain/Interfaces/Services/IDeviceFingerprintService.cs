namespace Farola.Domain.Interfaces.Services
{
    public interface IDeviceFingerprintService
    {
        string ComputeFingerprint(string deviceId, string userAgent);
    }
}
