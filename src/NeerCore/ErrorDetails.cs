namespace NeerCore;

/// <summary>
///   Represents a part of exception to describe
///   a problem for single field.
/// </summary>
/// <param name="Field">Field where problem found.</param>
/// <param name="Message">Problem description.</param>
public record ErrorDetails(string Field, string Message);