namespace Customer.Application.Exceptions;

public class CustomerCreationFailedException
    : Exception
{
    public CustomerCreationFailedException()
    {
    }

    public CustomerCreationFailedException(string message)
        : base(message)
    {
    }

    public CustomerCreationFailedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}