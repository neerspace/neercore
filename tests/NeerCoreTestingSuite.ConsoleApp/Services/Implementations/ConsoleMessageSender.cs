using NeerCore.DependencyInjection;
using NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

namespace NeerCoreTestingSuite.ConsoleApp.Services.Implementations;

[Dependency]
public class ConsoleMessageSender : IMessageSender
{
	public void Send(string message)
	{
		Console.WriteLine(message);
	}
}