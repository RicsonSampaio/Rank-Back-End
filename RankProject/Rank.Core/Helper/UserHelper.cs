using MySqlConnector;

namespace Rank.Core.Helper;

public static class UserHelper
{
    public const string EmailAlreadyInUseMessage = "Este email já está cadastrado.";

    public static bool IsEmailAlreadyInUse(Exception exception)
    {
        return exception is MySqlException { Number: 1062 };
    }
}
