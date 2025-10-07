namespace Customer.Application.Exceptions;

public class DuplicationException: Exception
{
    public DuplicationException()
    {
        
    }

    public DuplicationException(string message) : base(message)
    {
        
    }

    public DuplicationException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}