using System.Diagnostics;

namespace Pozitron.QuerySpecification;

internal sealed class SpecSelectIterator<TObject, TResult> : Iterator<TResult>
{
    private readonly SpecItem[] _source;
    private readonly Func<TObject, int, TResult> _selector;
    private readonly int _type;

    public SpecSelectIterator(SpecItem[] source, int type, Func<TObject, int, TResult> selector)
    {
        Debug.Assert(source != null && source.Length > 0);
        Debug.Assert(selector != null);
        _type = type;
        _source = source;
        _selector = selector;
    }

    public override Iterator<TResult> Clone() =>
        new SpecSelectIterator<TObject, TResult>(_source, _type, _selector);

    public override bool MoveNext()
    {
        var index = _state - 1;
        var source = _source;
        var type = _type;

        while (unchecked((uint)index < (uint)source.Length))
        {
            var item = source[index];
            index = _state++;
            if (item.Type == type && item.Reference is TObject reference)
            {
                _current = _selector(reference, item.Bag);
                return true;
            }
        }

        Dispose();
        return false;
    }
}
