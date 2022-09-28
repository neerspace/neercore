using System.Security.Claims;

namespace NeerCore.Security;

/// <summary>
///   A set of default claims.
/// </summary>
[Obsolete("Should be removed in the next version. Use your own claims class instead of this.")]
public readonly struct Claims
{
    public const string Id = "sub";
    public const string UserName = "nameid";
    public const string Role = ClaimTypes.Role;
    public const string Permission = "perm";
}