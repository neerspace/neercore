using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
	public static void ApplyConfigurationsFromCurrentAssembly(this ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
	}
}