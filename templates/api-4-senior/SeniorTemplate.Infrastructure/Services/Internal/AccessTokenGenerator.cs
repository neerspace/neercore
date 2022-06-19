using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection;
using SeniorTemplate.Application.Options;
using SeniorTemplate.Core.Constants;
using SeniorTemplate.Data.Entities;
using SeniorTemplate.Infrastructure.Model;

namespace SeniorTemplate.Infrastructure.Services.Internal;

[Inject]
public class AccessTokenGenerator
{
	private readonly IHashids _hashids;
	private readonly IDatabaseContext _database;
	private readonly JwtOptions _options;

	public AccessTokenGenerator(IOptions<JwtOptions> optionsAccessor, IHashids hashids, IDatabaseContext database)
	{
		_hashids = hashids;
		_database = database;
		_options = optionsAccessor.Value;
	}

	public async Task<JwtToken> GenerateAsync(AppUser user, CancellationToken cancel = default)
	{
		DateTime expires = DateTime.UtcNow.Add(_options.AccessTokenLifetime);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(await GetUserClaimsAsync(user, cancel)),
			Expires = expires,
			SigningCredentials = new SigningCredentials(_options.Secret, SecurityAlgorithms.HmacSha256Signature),
			IssuedAt = DateTime.UtcNow,
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		SecurityToken jwt = tokenHandler.CreateToken(tokenDescriptor);

		return new JwtToken(tokenHandler.WriteToken(jwt), expires);
	}

	private async Task<IEnumerable<Claim>> GetUserClaimsAsync(AppUser user, CancellationToken cancel)
	{
		var claims = new List<Claim>
		{
			new(Claims.Id, _hashids.EncodeLong(user.Id)),
			new(Claims.UserName, user.NormalizedUserName)
		};

		IEnumerable<string> roles = await GetUserRolesAsync(user.Id, cancel);

		claims.AddRange(roles.Select(role => new Claim(Claims.Role, role)));

		claims.AddRange(await GetUserClaimsAsync(user.Id, cancel));
		return claims;
	}

	public async Task<IEnumerable<Claim>> GetUserClaimsAsync(long userId, CancellationToken cancel)
	{
		// TODO: mb smth not works
		List<Claim> claims = await (from u in _database.Set<AppUserClaim>()
					where u.UserId == userId
					select new Claim(u.ClaimType, u.ClaimValue ?? "null"))
				.ToListAsync(cancel);

		claims.AddRange(await _database.Set<AppUserRole>()
				.Where(e => e.UserId == userId)
				.Join(_database.Set<AppRoleClaim>(), ur => ur.RoleId, rc => rc.RoleId, (_, rc) => rc)
				.Select(e => new Claim(e.ClaimType, e.ClaimValue ?? "null"))
				.ToListAsync(cancel));

		return claims;
	}

	public Task<List<string>> GetUserRolesAsync(long userId, CancellationToken cancel)
	{
		return (from u in _database.Set<AppUserRole>()
			where u.UserId == userId
			join r in _database.Set<AppRole>() on u.RoleId equals r.Id
			select r.Name).ToListAsync(cancel);
	}
}