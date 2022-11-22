using AsmTest.Root;
using AsmTest.Root.FirstLvl;
using AsmTest.Root.SecondLvl;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

var services = new ServiceCollection();
services.AddAllServices(options =>
{
    options.ResolveInternalImplementations = true;
});
var serviceProvider = services.BuildServiceProvider();

serviceProvider.GetService<IRootService>()?.Log();
serviceProvider.GetService<FirstService>()?.Log();
serviceProvider.GetService<SecondService>()?.Log();