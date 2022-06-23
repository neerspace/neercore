using System.Security.Claims;
using NeerCore.Exceptions;
using SeniorTemplate.Core.Constants;

namespace SeniorTemplate.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
	/// <summary>
	/// Checks if the user's token contains this claim with value.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <param name="type">Claim type</param>
	/// <param name="value">Claim value</param>
	/// <returns>Boolean result of check</returns>
	public static bool HasClaim(this ClaimsPrincipal user, string type, string value)
	{
		return string.Equals(user.GetClaim(type).Value, value, StringComparison.OrdinalIgnoreCase);
	}

	/// <summary>
	/// Checks if the user's token contains this permission.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <param name="permission">Permission string name</param>
	/// <returns>Boolean result of check</returns>
	public static bool HasPermission(this ClaimsPrincipal user, string permission)
	{
		return user.HasClaim(Claims.Permission, permission);
	}


	/// <summary>
	/// Gets value from the claim with the given type.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <param name="type">Claim type</param>
	/// <returns>Found claim or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static Claim GetClaim(this ClaimsPrincipal user, string type)
	{
		if (!user.Identity!.IsAuthenticated)
			throw new UnauthorizedException("Claims principal is not authenticated.");

		return user.Claims.SingleOrDefault(c => string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase))
		       ?? throw new ForbidException($"Claims principal doesn't have {type} claim.");
	}

	/// <summary>
	/// Gets list of all values from the claim with the given type.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <param name="type">Claim type</param>
	/// <returns>Found list of all matched claims or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static IReadOnlyList<Claim> ListClaims(this ClaimsPrincipal user, string type)
	{
		if (!user.Identity!.IsAuthenticated)
			throw new UnauthorizedException("Claims principal is not authenticated.");

		var claims = user.Claims.Where(c => string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase)).ToList();
		if (!claims.Any())
			throw new ForbidException($"Claims principal doesn't have any {type} claim.");

		return claims;
	}

	/// <summary>
	/// Gets user id from claims.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <returns>User id or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static long GetUserId(this ClaimsPrincipal user)
	{
		Claim claim = user.GetClaim(Claims.Id);
		return long.TryParse(claim.Value, out long userId) ? userId : throw InvalidClaimValueException(Claims.Id);
	}

	/// <summary>
	/// Gets username from claims.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <returns>Username or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static string GetUserName(this ClaimsPrincipal user)
	{
		Claim claim = user.GetClaim(Claims.UserName);
		return claim.Value ?? throw InvalidClaimValueException(Claims.UserName);
	}

	/// <summary>
	/// Gets list of user roles from claims.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <returns>User roles or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal user)
	{
		return user.ListClaims(Claims.Role).Select(c => c.Value);
	}

	/// <summary>
	/// Gets list of user permissions from claims.
	/// </summary>
	/// <param name="user">Current user principal</param>
	/// <returns>User permissions or throws exception if something goes wrong</returns>
	/// <exception cref="UnauthorizedException">Throws when the user is not authenticated</exception>
	/// <exception cref="ForbidException">Throws when the user is doesn't have a claim with given type</exception>
	public static IEnumerable<string> GetUserPermissions(this ClaimsPrincipal user)
	{
		return user.ListClaims(Claims.Permission).Select(c => c.Value);
	}


	private static UnauthorizedException InvalidClaimValueException(string type)
	{
		return new UnauthorizedException($"Claims principal's {type} claim contains invalid value.");
	}
}