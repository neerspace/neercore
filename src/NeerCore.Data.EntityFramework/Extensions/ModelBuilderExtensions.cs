using Microsoft.EntityFrameworkCore;
using NeerCore.DependencyInjection;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
	public static void ApplyConfigurationsFromCurrentAssembly(this ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(StackTraceUtility.GetCallerAssembly());
	}
}