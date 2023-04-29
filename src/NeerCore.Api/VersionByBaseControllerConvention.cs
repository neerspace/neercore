using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;

namespace NeerCore.Api;

/// <summary>
/// 
/// </summary>
public class VersionByBaseControllerConvention : IControllerConvention
{
    /// <inheritdoc cref="IControllerConvention.Apply"/>
    public bool Apply(IControllerConventionBuilder controller, ControllerModel controllerModel)
    {
        if (controller == null)
            throw new ArgumentNullException(nameof(controller));

        if (controllerModel == null)
            throw new ArgumentNullException(nameof(controllerModel));

        var controllerType = controllerModel.ControllerType;
        var baseClassAttrs = controllerType.BaseType?
                .GetCustomAttributes<ApiVersionAttribute>(true)
            ?? Array.Empty<ApiVersionAttribute>();
        var attrs = controllerType
            .GetCustomAttributes<ApiVersionAttribute>(true)
            .Union(baseClassAttrs)
            .DistinctBy(x => x.Versions)
            .ToArray();

        foreach (var versionAttribute in attrs)
        foreach (var apiVersion in versionAttribute.Versions)
        {
            var deprecated = controllerModel.Attributes.OfType<ObsoleteAttribute>().Any();
            if (deprecated)
                controller.HasDeprecatedApiVersion(apiVersion!);
            else
                controller.HasApiVersion(apiVersion!);
        }

        return true;
    }
}