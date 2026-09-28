using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class ColetivoRepository : DBDapperComponent
{
    private const string cTableName = "coletivo";
    private const string cListFields = "id, nome, dataCriacao, dataAtualizacao, idOrganizacao, idTipoColetivo, logo";

    public ColetivoRepository() : base()
    {
    }

    public async Task<Coletivo?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = @id";
        return await QuerySingleAsync<Coletivo>(commandText, new { id }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Coletivo>> GetAllAsync()
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " ORDER BY id";
        return await QueryListAsync<Coletivo>(commandText).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Coletivo>> GetByOrganizacaoAsync(int idOrganizacao)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName +
            " WHERE idOrganizacao = @idOrganizacao ORDER BY id";
        return await QueryListAsync<Coletivo>(commandText, new { idOrganizacao }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Coletivo>> GetByUsuarioVinculadoAsync(int idUsuario)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName +
            " WHERE EXISTS (SELECT 1 FROM coletivo_usuario cu WHERE cu.idColetivo = " + cTableName +
            ".id AND cu.idUsuario = @idUsuario) ORDER BY id";
        return await QueryListAsync<Coletivo>(commandText, new { idUsuario }).ConfigureAwait(false);
    }

    public async Task<int> CreateAsync(Coletivo coletivo)
    {
        string commandText = "INSERT INTO " + cTableName +
            " (nome, dataCriacao, dataAtualizacao, idOrganizacao, idTipoColetivo, logo) " +
            "VALUES (@Nome, @DataCriacao, @DataAtualizacao, @IdOrganizacao, @IdTipoColetivo, @Logo)";
        return await InsertAndGetIdAsync(commandText, coletivo).ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(Coletivo coletivo)
    {
        string commandText = "UPDATE " + cTableName +
            " SET nome = @Nome, logo = @Logo, idTipoColetivo = @IdTipoColetivo, " +
            "dataAtualizacao = @DataAtualizacao WHERE id = @Id";
        return await ExecuteAsync(commandText, coletivo).ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE id = @id";
        return await ExecuteAsync(commandText, new { id }).ConfigureAwait(false) > 0;
    }
}
