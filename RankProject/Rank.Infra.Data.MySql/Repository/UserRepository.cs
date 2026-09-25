using Dapper;
using MySqlConnector;
using Rank.Core.DomainEntity;
using Rank.Core.Repository;
using Rank.Core.Service;

namespace Rank.Infra.Data.MySql.Repository;

public sealed class UserRepository(string connectionString) : IUserRepository
{
    private const string UserFields = "Id, Email, Name, PasswordHash, IsActive";

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        await using var connection = new MySqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition($"SELECT {UserFields} FROM usuario WHERE Email = @Email",
                new { Email = email }, cancellationToken: cancellationToken));
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        await using var connection = new MySqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition($"SELECT {UserFields} FROM usuario WHERE Id = @Id",
                new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = new MySqlConnection(connectionString);
        var users = await connection.QueryAsync<User>(
            new CommandDefinition("SELECT Id, Email, Name, IsActive FROM usuario ORDER BY Id",
                cancellationToken: cancellationToken));
        return users.AsList();
    }

    public async Task<long> CreateAsync(User user, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO usuario (Email, Name, PasswordHash, IsActive)
            VALUES (@Email, @Name, @PasswordHash, @IsActive)
            """;
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));
            return await connection.ExecuteScalarAsync<long>(
                new CommandDefinition("SELECT LAST_INSERT_ID()", cancellationToken: cancellationToken));
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new EmailAlreadyInUseException();
        }
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE usuario
            SET Email = @Email, Name = @Name, PasswordHash = @PasswordHash
            WHERE Id = @Id AND IsActive = 1
            """;
        await using var connection = new MySqlConnection(connectionString);
        try
        {
            return await connection.ExecuteAsync(
                new CommandDefinition(sql, user, cancellationToken: cancellationToken)) > 0;
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new EmailAlreadyInUseException();
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await using var connection = new MySqlConnection(connectionString);
        return await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM usuario WHERE Id = @Id", new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }
}
