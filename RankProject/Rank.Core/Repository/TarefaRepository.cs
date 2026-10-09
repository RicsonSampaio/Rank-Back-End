using Rank.Core.DomainEntity;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Core.Repository;

public class TarefaRepository : DBDapperComponent
{
    private const string cTableName = "tarefa";
    private const string cListFields =
            "id, idColetivo, idEspaco, idEscopo, idstatus, idCategoria, idUsuarioCriacao, titulo, privada, descricao, dataCriacao, " +
            "dataAtualizacao, lastDoneDate, idTarefaPai, userListMarcados, userListParticipantes, idDocumento, " +
            "prazoInicial, prazoFinal, idResponsavel, idFase, idRelevancia";

    // novo
    private const string cListFieldsComResponsavel =
            "t.id, t.idColetivo, t.idEspaco, t.idEscopo, t.idstatus, t.idCategoria, t.idUsuarioCriacao, t.titulo, t.privada, t.descricao, t.dataCriacao, " +
            "t.dataAtualizacao, t.lastDoneDate, t.idTarefaPai, t.userListMarcados, t.userListParticipantes, t.idDocumento, " +
            "t.prazoInicial, t.prazoFinal, t.idResponsavel, t.idFase, t.idRelevancia, " +
            "u.name AS NomeResponsavel";

    public TarefaRepository() : base()
    {
    }

    public async Task<IReadOnlyList<Tarefa>> GetAllAsync(int idColetivo)
    {
        string commandText = "SELECT " + cListFieldsComResponsavel +
            " FROM " + cTableName + " t" +
            " LEFT JOIN usuario u ON u.id = t.idResponsavel" +
            " WHERE t.idColetivo = @idColetivo ORDER BY t.id";
        return await QueryListAsync<Tarefa>(commandText, new { idColetivo }).ConfigureAwait(false);
    }

    public async Task<Tarefa?> GetByIdAsync(int id)
    {
        string commandText = "SELECT " + cListFields + " FROM " + cTableName + " WHERE id = @id";
        return await QuerySingleAsync<Tarefa>(commandText, new { id }).ConfigureAwait(false);
    }

    public async Task<int> CreateAsync(Tarefa tarefa)
    {
        string commandText = "INSERT INTO " + cTableName +
            " (idColetivo, idEspaco, idEscopo, idstatus, idCategoria, idUsuarioCriacao, titulo, privada, descricao, dataCriacao, " +
            "dataAtualizacao, lastDoneDate, idTarefaPai, userListMarcados, userListParticipantes, idDocumento, " +
            "prazoInicial, prazoFinal, idResponsavel, idFase, idRelevancia) VALUES (@IdColetivo, @IdEspaco, @IdEscopo, @IdStatus, " +
            "@IdCategoria, @IdUsuarioCriacao, @Titulo, @Privada, @Descricao, @DataCriacao, @DataAtualizacao, " +
            "@LastDoneDate, @IdTarefaPai, @UserListMarcados, @UserListParticipantes, @IdDocumento, @PrazoInicial, " +
            "@PrazoFinal, @IdResponsavel, @IdFase, @IdRelevancia)";
        return await InsertAndGetIdAsync(commandText, tarefa).ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(Tarefa tarefa)
    {
        string commandText = "UPDATE " + cTableName +
            " SET idColetivo = @IdColetivo, idEspaco = @IdEspaco, idEscopo = @IdEscopo, idstatus = @IdStatus, idCategoria = @IdCategoria, " +
            "idUsuarioCriacao = @IdUsuarioCriacao, titulo = @Titulo, privada = @Privada, descricao = @Descricao, " +
            "dataAtualizacao = @DataAtualizacao, lastDoneDate = @LastDoneDate, idTarefaPai = @IdTarefaPai, " +
            "userListMarcados = @UserListMarcados, userListParticipantes = @UserListParticipantes, " +
            "idDocumento = @IdDocumento, prazoInicial = @PrazoInicial, prazoFinal = @PrazoFinal, " +
            "idResponsavel = @IdResponsavel, idFase = @IdFase, idRelevancia = @IdRelevancia WHERE id = @Id";
        return await ExecuteAsync(commandText, tarefa).ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string commandText = "DELETE FROM " + cTableName + " WHERE id = @id";
        return await ExecuteAsync(commandText, new { id }).ConfigureAwait(false) > 0;
    }
}
