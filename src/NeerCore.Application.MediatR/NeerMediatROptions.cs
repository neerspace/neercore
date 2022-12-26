namespace NeerCore.Application;

public class NeerMediatROptions
{
    public bool ResolveInternalHandlers { get; set; } = false;
    public bool ResolveInternalValidators { get; set; } = false;
}