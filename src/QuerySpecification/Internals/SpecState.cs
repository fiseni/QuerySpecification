namespace Pozitron.QuerySpecification;

internal struct SpecState
{
    public int Type; // 0-4 bytes
    public int Bag; // 4-8 bytes
    public object? Reference; // 8-16 bytes (on x64)
}
