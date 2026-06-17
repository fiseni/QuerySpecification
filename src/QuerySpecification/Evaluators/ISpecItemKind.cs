namespace Pozitron.QuerySpecification;

// Optional marker implemented by the built-in evaluators so the orchestrators can skip them in O(1)
// when the spec contains no item of the relevant kind, instead of letting each evaluator scan the items.
// Evaluators that do not implement it (e.g. custom/auto-discovered ones) always run.
internal interface ISpecItemKind
{
    // The ItemType this evaluator acts upon. The built-in kinds are negative (see ItemType).
    int ItemType { get; }
}
