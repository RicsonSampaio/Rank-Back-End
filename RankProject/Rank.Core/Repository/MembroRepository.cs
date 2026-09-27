using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class MembroRepository : DBDapperComponent
{
    private const string cTableName = "coletivo_usuario";
    private const string cListFields = "cu.id, cu.idColetivo, cu.idUsuario, u.email, u.name AS nome, u.fotoAccount";
    private const string cUserJoin = " cu LEFT JOIN usuario u ON u.id = cu.idUsuario";

    public MembroRepository() : base()
    {
    }

    public async Task<IReadOnlyList<Membro>> GetAllAsync(int idColetivo)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + cUserJoin +
            " WHERE cu.idColetivo = @idColetivo ORDER BY cu.id";
        return await QueryListAsync<Membro>(commandText, new { idColetivo }).ConfigureAwait(false);
    }

    public async Task<Membro?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + cUserJoin + " WHERE cu.id = @id";
        return await QuerySingleAsync<Membro>(commandText, new { id }).ConfigureAwait(false);
    }

    public async Task<int> CreateAsync(Membro membro)
    {
        string commandText = "INSERT INTO " + cTableName +
            " (idColetivo, idUsuario) VALUES (@IdColetivo, @IdUsuario)";
        return await InsertAndGetIdAsync(commandText, membro).ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(Membro membro)
    {
        string commandText = "UPDATE " + cTableName +
            " SET idColetivo = @IdColetivo, idUsuario = @IdUsuario WHERE id = @Id";
        return await ExecuteAsync(commandText, membro).ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE id = @id";
        return await ExecuteAsync(commandText, new { id }).ConfigureAwait(false) > 0;
    }
}
