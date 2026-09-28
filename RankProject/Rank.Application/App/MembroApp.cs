using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Membros;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class MembroApp
{
    private readonly IServiceProvider _provider;

    public MembroApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<MembroResponse> CreateAsync(CreateMembroRequest request)
    {
        var membroService = _provider.GetRequiredService<MembroService>();
        return membroService.CreateAsync(request);
    }

    public Task<IReadOnlyList<MembroResponse>> GetAllAsync(int idColetivo)
    {
        var membroService = _provider.GetRequiredService<MembroService>();
        return membroService.GetAllAsync(idColetivo);
    }

    public Task<MembroResponse?> GetByIdAsync(int id)
    {
        var membroService = _provider.GetRequiredService<MembroService>();
        return membroService.GetByIdAsync(id);
    }

    public Task<MembroResponse?> UpdateAsync(int id, UpdateMembroRequest request)
    {
        var membroService = _provider.GetRequiredService<MembroService>();
        return membroService.UpdateAsync(id, request);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var membroService = _provider.GetRequiredService<MembroService>();
        return membroService.DeleteAsync(id);
    }
}
