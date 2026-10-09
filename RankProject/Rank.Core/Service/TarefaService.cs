using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Tarefas;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;

namespace Rank.Core.Service;

public class TarefaService
{
    private readonly IServiceProvider _provider;

    public TarefaService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TarefaResponse> CreateAsync(CreateTarefaRequest request)
    {
        var tarefaRepository = _provider.GetRequiredService<TarefaRepository>();
        var tarefa = new Tarefa
        {
            IdColetivo = request.IdColetivo,
            IdEspaco = request.IdEspaco,
            IdEscopo = request.IdEscopo,
            IdStatus = request.IdStatus,
            IdCategoria = request.IdCategoria,
            IdUsuarioCriacao = request.IdUsuarioCriacao,
            Titulo = request.Titulo.Trim(),
            Privada = request.Privada,
            Descricao = request.Descricao,
            LastDoneDate = request.LastDoneDate,
            IdTarefaPai = request.IdTarefaPai,
            UserListMarcados = request.UserListMarcados,
            UserListParticipantes = request.UserListParticipantes,
            IdDocumento = request.IdDocumento,
            PrazoInicial = request.PrazoInicial,
            PrazoFinal = request.PrazoFinal,
            IdResponsavel = request.IdResponsavel,
            IdFase = request.IdFase,
            IdRelevancia = request.IdRelevancia,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = null
        };
        var id = await tarefaRepository.CreateAsync(tarefa).ConfigureAwait(false);
        return ToResponse(tarefa, id);
    }

    public async Task<IReadOnlyList<TarefaResponse>> GetAllAsync(int idColetivo)
    {
        var tarefaRepository = _provider.GetRequiredService<TarefaRepository>();
        var tarefas = await tarefaRepository.GetAllAsync(idColetivo).ConfigureAwait(false);
        return tarefas.Select(tarefa => ToResponse(tarefa)).ToArray();
    }

    public async Task<TarefaResponse?> GetByIdAsync(int id)
    {
        var tarefaRepository = _provider.GetRequiredService<TarefaRepository>();
        var tarefa = await tarefaRepository.GetByIdAsync(id).ConfigureAwait(false);
        return tarefa is null ? null : ToResponse(tarefa);
    }

    public async Task<TarefaResponse?> UpdateAsync(int id, UpdateTarefaRequest request)
    {
        var tarefaRepository = _provider.GetRequiredService<TarefaRepository>();
        var current = await tarefaRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (current is null)
            return null;

        var tarefa = new Tarefa
        {
            Id = id,
            IdColetivo = request.IdColetivo,
            IdEspaco = request.IdEspaco,
            IdEscopo = request.IdEscopo,
            IdStatus = request.IdStatus,
            IdCategoria = request.IdCategoria,
            IdUsuarioCriacao = request.IdUsuarioCriacao,
            Titulo = request.Titulo.Trim(),
            Privada = request.Privada,
            Descricao = request.Descricao,
            LastDoneDate = request.LastDoneDate,
            IdTarefaPai = request.IdTarefaPai,
            UserListMarcados = request.UserListMarcados,
            UserListParticipantes = request.UserListParticipantes,
            IdDocumento = request.IdDocumento,
            PrazoInicial = request.PrazoInicial,
            PrazoFinal = request.PrazoFinal,
            IdResponsavel = request.IdResponsavel,
            IdFase = request.IdFase,
            IdRelevancia = request.IdRelevancia,
            DataCriacao = current.DataCriacao,
            DataAtualizacao = DateTime.UtcNow
        };
        return await tarefaRepository.UpdateAsync(tarefa).ConfigureAwait(false)
            ? ToResponse(tarefa) : null;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var tarefaRepository = _provider.GetRequiredService<TarefaRepository>();
        return tarefaRepository.DeleteAsync(id);
    }

    private static TarefaResponse ToResponse(Tarefa tarefa, int? id = null) => new()
    {
        Id = id ?? tarefa.Id,
        IdColetivo = tarefa.IdColetivo,
        IdEspaco = tarefa.IdEspaco,
        IdEscopo = tarefa.IdEscopo,
        IdStatus = tarefa.IdStatus,
        IdCategoria = tarefa.IdCategoria,
        IdUsuarioCriacao = tarefa.IdUsuarioCriacao,
        Titulo = tarefa.Titulo,
        Privada = tarefa.Privada,
        Descricao = tarefa.Descricao,
        DataCriacao = tarefa.DataCriacao,
        DataAtualizacao = tarefa.DataAtualizacao,
        LastDoneDate = tarefa.LastDoneDate,
        IdTarefaPai = tarefa.IdTarefaPai,
        UserListMarcados = tarefa.UserListMarcados,
        UserListParticipantes = tarefa.UserListParticipantes,
        IdDocumento = tarefa.IdDocumento,
        PrazoInicial = tarefa.PrazoInicial,
        PrazoFinal = tarefa.PrazoFinal,
        IdResponsavel = tarefa.IdResponsavel,
        NomeResponsavel = tarefa.NomeResponsavel,
        IdFase = tarefa.IdFase,
        IdRelevancia = tarefa.IdRelevancia
    };
}
