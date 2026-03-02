namespace Metagram.Extensions;

public static class ExceptionExtensions
{
    public static Exception Aggregate(this Exception exception)
        => exception.InnerException != null ? exception.InnerException.Aggregate() : exception;
}
