using MySqlConnector;
using Rank.Core.DomainEntity;
using Rank.Core.Service;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class UserRepository : DBDapperComponent
{
    private const string cTableName = "usuario";
    private const string cListFields = "Id, Email, Name, PasswordHash, IsActive";
    private const string cPublicFields = "Id, Email, Name, IsActive";

    public UserRepository() : base()
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE Email = @email";
        return await QuerySingleAsync<User>(commandText, new { email }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE Id = @id";
        return await QuerySingleAsync<User>(commandText, new { id }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        string commandText = "SELECT " + cPublicFields + " FROM " + cTableName + " ORDER BY Id";
        return await QueryListAsync<User>(commandText, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<long> CreateAsync(User user, CancellationToken cancellationToken)
    {
        string commandText = "INSERT INTO " + cTableName +
            " (Email, Name, PasswordHash, IsActive) VALUES (@Email, @Name, @PasswordHash, @IsActive)";
        try
        {
            return await InsertAndGetIdAsync(commandText, user, cancellationToken).ConfigureAwait(false);
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new EmailAlreadyInUseException();
        }
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        string commandText = "UPDATE " + cTableName +
            " SET Email = @Email, Name = @Name, PasswordHash = @PasswordHash WHERE Id = @Id AND IsActive = 1";
        try
        {
            return await ExecuteAsync(commandText, user, cancellationToken).ConfigureAwait(false) > 0;
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new EmailAlreadyInUseException();
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE Id = @id";
        return await ExecuteAsync(commandText, new { id }, cancellationToken).ConfigureAwait(false) > 0;
    }
}
