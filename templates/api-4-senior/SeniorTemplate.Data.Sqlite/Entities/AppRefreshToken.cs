using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppRefreshToken : IEntity
{
	/// <summary>
	/// Used as primary key
	/// </summary>
	public string Token { get; set; } = default!;

	public int UserId { get; set; }
	public string UserAgent { get; set; } = default!;

	/// <summary>
	/// 255.255.255.255 => 3*4+3 = 15 length
	/// 0000:0000:0000:0000:0000:0000:0000:0000 => 8*4+7 = 39
	/// 0000:0000:0000:0000:0000:FFFF:192.168.100.228 => (ipv6)+1+(ipv4) = 29+1+15 = 45
	/// </summary>
	public string IpAddress { get; set; } = default!;

	public DateTime Created { get; set; } = DateTime.UtcNow;


	public virtual AppUser? User { get; set; }
}