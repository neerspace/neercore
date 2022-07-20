namespace NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

public interface IGreetingService
{
	void Greet(string target);
	void Say(string target, string message);
}