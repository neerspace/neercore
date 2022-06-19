namespace SeniorTemplate.Core.Enums;

/// <summary>
/// Authorization methods.
/// The higher value means the higher priority to use this method.
/// </summary>
public enum AuthMethod
{
	Register = 0,
	Password = 10
}