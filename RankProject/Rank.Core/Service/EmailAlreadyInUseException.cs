namespace Rank.Core.Service;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException() : base("Este email já está cadastrado.")
    {
    }
}
