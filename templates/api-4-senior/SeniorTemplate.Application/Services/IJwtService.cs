using SeniorTemplate.Application.Features.Auth;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Application.Services;

public interface IJwtService
{
	Task<AuthResult> GenerateAsync(AppUser user, CancellationToken cancel = default);
	Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancel = default);
}