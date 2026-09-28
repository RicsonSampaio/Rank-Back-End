using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Tarefas;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class TarefaApp
{
    private readonly IServiceProvider _provider;

    public TarefaApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TarefaResponse> CreateAsync(CreateTarefaRequest request)
    {
        var tarefaService = _provider.GetRequiredService<TarefaService>();
        return tarefaService.CreateAsync(request);
    }

    public Task<IReadOnlyList<TarefaResponse>> GetAllAsync(int idColetivo)
    {
        var tarefaService = _provider.GetRequiredService<TarefaService>();
        return tarefaService.GetAllAsync(idColetivo);
    }

    public Task<TarefaResponse?> GetByIdAsync(int id)
    {
        var tarefaService = _provider.GetRequiredService<TarefaService>();
        return tarefaService.GetByIdAsync(id);
    }

    public Task<TarefaResponse?> UpdateAsync(int id, UpdateTarefaRequest request)
    {
        var tarefaService = _provider.GetRequiredService<TarefaService>();
        return tarefaService.UpdateAsync(id, request);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var tarefaService = _provider.GetRequiredService<TarefaService>();
        return tarefaService.DeleteAsync(id);
    }
}
