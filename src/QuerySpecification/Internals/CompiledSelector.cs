namespace Pozitron.QuerySpecification;

// Caches a compiled Select/SelectMany delegate together with the source expression it was compiled from.
// The in-memory evaluator reuses the compiled delegate across evaluations, and recompiles only when the
// source expression reference changes (e.g. the spec's Select is updated).
internal sealed class CompiledSelector
{
    public object Source { get; }
    public object Compiled { get; }

    public CompiledSelector(object source, object compiled)
    {
        Source = source;
        Compiled = compiled;
    }
}
