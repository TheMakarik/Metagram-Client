namespace Metagram.Extensions;

public static class CancellationTokenExtensions
{
    public static CancellationToken LinkWith(this CancellationToken cancellationToken, params CancellationToken[] cancellationTokens)
        => CancellationTokenSource.CreateLinkedTokenSource([cancellationToken, .. cancellationTokens]).Token;
}