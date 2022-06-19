using SeniorTemplate.Core.Enums;

namespace SeniorTemplate.Application.Features.Auth.Check;

public record AuthCheckResult(
	bool UserExists,
	string? Username,
	AuthMethod PreferAuthMethod,
	IEnumerable<AuthMethod> AllowedAuthMethod
);