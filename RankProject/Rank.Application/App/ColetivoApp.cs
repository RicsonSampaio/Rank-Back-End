using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Coletivos;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class ColetivoApp
{
    private readonly IServiceProvider _provider;

    public ColetivoApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<ColetivoResponse> CreateAsync(
        CreateColetivoRequest request, int idOrganizacao)
    {
        var coletivoService = _provider.GetRequiredService<ColetivoService>();
        return coletivoService.CreateAsync(request, idOrganizacao);
    }

    public Task<IReadOnlyList<ColetivoResponse>> GetAllAsync(int idUsuario)
    {
        var coletivoService = _provider.GetRequiredService<ColetivoService>();
        return coletivoService.GetAllAsync(idUsuario);
    }

    public Task<ColetivoResponse?> GetByIdAsync(int id)
    {
        var coletivoService = _provider.GetRequiredService<ColetivoService>();
        return coletivoService.GetByIdAsync(id);
    }

    public Task<ColetivoResponse?> UpdateAsync(
        int id, UpdateColetivoRequest request)
    {
        var coletivoService = _provider.GetRequiredService<ColetivoService>();
        return coletivoService.UpdateAsync(id, request);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var coletivoService = _provider.GetRequiredService<ColetivoService>();
        return coletivoService.DeleteAsync(id);
    }
}
