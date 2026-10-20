using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class TarefaStatusRepository : DBDapperComponent
{
    private const string cTableName = "tarefa_status";
    private const string cListFields = "id, nome";

    public TarefaStatusRepository() : base()
    {
    }

    public async Task<IReadOnlyList<TarefaStatus>> GetAllAsync()
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " ORDER BY id";
        return await QueryListAsync<TarefaStatus>(commandText).ConfigureAwait(false);
    }

    public async Task<TarefaStatus?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = @id";
        return await QuerySingleAsync<TarefaStatus>(commandText, new { id }).ConfigureAwait(false);
    }
}