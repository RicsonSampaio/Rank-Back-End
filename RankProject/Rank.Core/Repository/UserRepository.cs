using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class UserRepository : DBDapperComponent
{
    private const string cTableName = "usuario";
    private const string cListFields = "id, email, name, passwordhash, isactive, idOrganizacao, admin, dataCriacao, fotoAccount";
    private const string cPublicFields = "id, email, name, isactive, idOrganizacao, admin, dataCriacao, fotoAccount";

    public UserRepository() : base()
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE email = @email";
        return await QuerySingleAsync<User>(commandText, new { email }).ConfigureAwait(false);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = @id";
        return await QuerySingleAsync<User>(commandText, new { id }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        string commandText = "SELECT " + cPublicFields + " FROM " + cTableName + " ORDER BY id";
        return await QueryListAsync<User>(commandText).ConfigureAwait(false);
    }

    public async Task<User> CreateAsync(User user)
    {
        // LAST_INSERT_ID() é o ID da organização no INSERT do usuário.
        // Após esse INSERT, passa a ser o ID do usuário para o SELECT final.
        string commandText = "INSERT INTO organizacao (nome, dataCriacao, logo) VALUES (@Name, @DataCriacao, NULL); " +
            "INSERT INTO " + cTableName +
            " (email, name, passwordhash, isactive, idOrganizacao, admin, dataCriacao, fotoAccount) " +
            "VALUES (@Email, @Name, @PasswordHash, @IsActive, LAST_INSERT_ID(), @Admin, @DataCriacao, @FotoAccount); " +
            "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = LAST_INSERT_ID();";
        return await QuerySingleTransactionAsync<User>(commandText, user).ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        string commandText = "UPDATE " + cTableName +
            " SET email = @Email, name = @Name, passwordhash = @PasswordHash WHERE id = @Id AND isactive = 1";
        return await ExecuteAsync(commandText, user).ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE id = @id";
        return await ExecuteAsync(commandText, new { id }).ConfigureAwait(false) > 0;
    }
}
