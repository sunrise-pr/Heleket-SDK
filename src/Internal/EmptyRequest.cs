namespace Heleket.Internal;

internal sealed class EmptyRequest
{
    internal static readonly EmptyRequest Instance = new();

    private EmptyRequest()
    {
    }
}
