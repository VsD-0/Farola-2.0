using Farola.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Farola.Infrastructure.Services
{
    public class DeviceFingerprintService : IDeviceFingerprintService
    {
        private readonly string _salt;
        public DeviceFingerprintService(IConfiguration configuration)
        {
            _salt = configuration["SecuritySettings:DeviceFingerprintSalt"]
                ?? throw new InvalidOperationException("DeviceFingerprintSalt not configured");
        }

        public string ComputeFingerprint(string deviceId, string userAgent)
        {
            using var sha256 = SHA256.Create();
            var input = $"{deviceId}:{userAgent}:{_salt}";
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
