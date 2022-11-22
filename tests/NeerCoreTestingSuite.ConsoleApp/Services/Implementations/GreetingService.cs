using NeerCore.DependencyInjection;
using NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

namespace NeerCoreTestingSuite.ConsoleApp.Services.Implementations;

[Service]
internal class GreetingService : IGreetingService
{
	private readonly IMessageSender _sender;
	public GreetingService(IMessageSender sender) => _sender = sender;

	public void Greet(string target)
	{
		Say(target, "Hello World!");
	}

	public void Say(string target, string message)
	{
		_sender.Send($"Hey, {target}! {message}");
	}
}