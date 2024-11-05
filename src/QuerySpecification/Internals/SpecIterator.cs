using System.Diagnostics;

namespace Pozitron.QuerySpecification;

internal sealed class SpecIterator<TObject> : Iterator<TObject>
{
    private readonly SpecItem[] _source;
    private readonly int _type;

    public SpecIterator(SpecItem[] source, int type)
    {
        Debug.Assert(source != null && source.Length > 0);
        _type = type;
        _source = source;
    }

    public override Iterator<TObject> Clone() =>
        new SpecIterator<TObject>(_source, _type);

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
                _current = reference;
                return true;
            }
        }

        Dispose();
        return false;
    }
}
