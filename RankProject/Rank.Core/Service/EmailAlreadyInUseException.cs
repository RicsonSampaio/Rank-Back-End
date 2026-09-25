namespace Rank.Core.Service;

public sealed class EmailAlreadyInUseException() : Exception("Este email já está cadastrado.");
