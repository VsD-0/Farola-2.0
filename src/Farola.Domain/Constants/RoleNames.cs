using Farola.Domain.Enums;

namespace Farola.Domain.Constants
{
    public static class RoleNames
    {
        public const string Client = "Client";
        public const string Professional = "Professional";
        public const string Admin = "Admin";

        public static string GetName(RoleType role) => role switch
        {
            RoleType.Client => Client,
            RoleType.Professional => Professional,
            RoleType.Admin => Admin,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
