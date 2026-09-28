using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class OrganizacaoRepository : DBDapperComponent
{
    private const string cTableName = "organizacao";
    private const string cListFields = "id, nome, dataCriacao, logo";

    public OrganizacaoRepository() : base()
    {
    }

    public async Task<Organizacao?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = @id";
        return await QuerySingleAsync<Organizacao>(commandText, new { id }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Organizacao>> GetAllAsync()
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " ORDER BY id";
        return await QueryListAsync<Organizacao>(commandText).ConfigureAwait(false);
    }

    public async Task<int> CreateAsync(Organizacao organizacao)
    {
        string commandText = "INSERT INTO " + cTableName +
            " (nome, dataCriacao, logo) VALUES (@Nome, @DataCriacao, @Logo)";
        return await InsertAndGetIdAsync(commandText, organizacao).ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(Organizacao organizacao)
    {
        string commandText = "UPDATE " + cTableName + " SET nome = @Nome, logo = @Logo WHERE id = @Id";
        return await ExecuteAsync(commandText, organizacao).ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE id = @id";
        return await ExecuteAsync(commandText, new { id }).ConfigureAwait(false) > 0;
    }
}
