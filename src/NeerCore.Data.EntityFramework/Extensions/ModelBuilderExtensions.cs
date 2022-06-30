using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
	/// <summary>
	///		Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}"/>
	///		instances that are defined in provided <b>calling assembly</b>.
	/// </summary>
	/// <param name="builder">DB models builder</param>
	public static void ApplyConfigurationsFromCurrentAssembly(this ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
	}
}