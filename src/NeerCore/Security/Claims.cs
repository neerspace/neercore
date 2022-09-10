using System.Security.Claims;

namespace NeerCore.Security;

/// <summary>
///   A set of default claims.
/// </summary>
public struct Claims
{
    public const string Id = "sub";
    public const string UserName = "nameid";
    public const string Role = ClaimTypes.Role;
    public const string Permission = "perm";
}