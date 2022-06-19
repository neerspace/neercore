namespace SeniorTemplate.Infrastructure.Model;

public record struct JwtToken(string Token, DateTime Expires);