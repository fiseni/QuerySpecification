namespace Pozitron.QuerySpecification;

/// <summary>
/// Specifies the strategy for discovering evaluators and validators.
/// </summary>
public enum DiscoveryStrategy
{
    /// <summary>
    /// Discovery is disabled.
    /// </summary>
    Disable = 1,

    /// <summary>
    /// Only built-in evaluators/validators from this library are discovered.
    /// </summary>
    BuiltInOnly = 2,

    /// <summary>
    /// All evaluators from all loaded assemblies are discovered.
    /// </summary>
    All = 3
}
