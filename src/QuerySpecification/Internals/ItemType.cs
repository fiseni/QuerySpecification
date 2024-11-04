namespace Pozitron.QuerySpecification;

internal static class ItemType
{
    public const int Where = -1;
    public const int Order = -2;
    public const int Include = -3;
    public const int IncludeString = -4;
    public const int Like = -5;
    public const int Select = -6;
    public const int Compiled = -7;

    // We can save 16  bytes (on x64) by storing both Flags and Paging in the same item.
    public const int Paging = -8; // Stored in the reference
    public const int Flags = -8; // Stored in the bag
}
