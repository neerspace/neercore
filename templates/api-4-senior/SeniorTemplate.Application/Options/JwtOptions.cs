using Microsoft.IdentityModel.Tokens;

namespace SeniorTemplate.Application.Options;

public record JwtOptions
{
	public SecurityKey? Secret { get; set; }
	public TimeSpan AccessTokenLifetime { get; set; }
	public TimeSpan RefreshTokenLifetime { get; set; }
}