using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;
using NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

var serviceProvider = BuildServiceProvider();
var greetingService = serviceProvider.GetRequiredService<IGreetingService>();
greetingService.Greet("Helen");

ServiceProvider BuildServiceProvider()
{
	var services = new ServiceCollection();
	services.AddAllServices();
	return services.BuildServiceProvider();
}