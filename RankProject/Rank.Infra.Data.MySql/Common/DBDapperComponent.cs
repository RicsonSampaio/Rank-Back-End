using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Rank.Infra.Data.MySql.Common;

public abstract class DBDapperComponent
{
    private static IConfiguration? _configuration;

    protected DBDapperComponent()
    {
    }

    public static void Configure(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private MySqlConnection CreateConnection()
    {
        var connectionString = _configuration?.GetConnectionString("Rank");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Configure ConnectionStrings:Rank.");

        return new MySqlConnection(connectionString);
    }

    protected async Task<T?> QuerySingleAsync<T>(
        string commandText, object? parameters = null) where T : class
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        return await connection.QuerySingleOrDefaultAsync<T>(
            new CommandDefinition(commandText, parameters)).ConfigureAwait(false);
    }

    protected async Task<IReadOnlyList<T>> QueryListAsync<T>(
        string commandText, object? parameters = null)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        var result = await connection.QueryAsync<T>(
            new CommandDefinition(commandText, parameters)).ConfigureAwait(false);
        return result.AsList();
    }

    protected async Task<int> ExecuteAsync(
        string commandText, object? parameters = null)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        return await connection.ExecuteAsync(
            new CommandDefinition(commandText, parameters)).ConfigureAwait(false);
    }

    protected async Task<long> InsertAndGetIdAsync(
        string commandText, object parameters)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await connection.ExecuteAsync(
            new CommandDefinition(commandText, parameters)).ConfigureAwait(false);
        return await connection.ExecuteScalarAsync<long>(
            new CommandDefinition("SELECT LAST_INSERT_ID()")).ConfigureAwait(false);
    }

    protected async Task<T> QuerySingleTransactionAsync<T>(
        string commandText, object? parameters = null) where T : class
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var result = await connection.QuerySingleAsync<T>(
                new CommandDefinition(commandText, parameters, transaction)).ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }
}
