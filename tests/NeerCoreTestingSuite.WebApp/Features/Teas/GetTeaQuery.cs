using NeerCore.Application.Abstractions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;

namespace NeerCoreTestingSuite.WebApp.Features.Teas;

public record GetTeaQuery(Guid Id) : IQuery<Tea>;