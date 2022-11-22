using AsmTest.Root;
using AsmTest.Root.FirstLvl;
using AsmTest.Root.SecondLvl;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

var services = new ServiceCollection();
services.AddAllServices();
var serviceProvider = services.BuildServiceProvider();

serviceProvider.GetService<RootService>()?.Log();
serviceProvider.GetService<FirstService>()?.Log();
serviceProvider.GetService<SecondService>()?.Log();