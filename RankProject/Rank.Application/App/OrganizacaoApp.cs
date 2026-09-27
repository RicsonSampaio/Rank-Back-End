using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Organizacoes;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class OrganizacaoApp
{
    private readonly IServiceProvider _provider;

    public OrganizacaoApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<IReadOnlyList<OrganizacaoResponse>> GetAllAsync()
    {
        var organizacaoService = _provider.GetRequiredService<OrganizacaoService>();
        return organizacaoService.GetAllAsync();
    }

    public Task<OrganizacaoResponse?> GetByIdAsync(long id)
    {
        var organizacaoService = _provider.GetRequiredService<OrganizacaoService>();
        return organizacaoService.GetByIdAsync(id);
    }

    public Task<OrganizacaoResponse?> UpdateAsync(
        long id, UpdateOrganizacaoRequest request)
    {
        var organizacaoService = _provider.GetRequiredService<OrganizacaoService>();
        return organizacaoService.UpdateAsync(id, request);
    }

    public Task<bool> DeleteAsync(long id)
    {
        var organizacaoService = _provider.GetRequiredService<OrganizacaoService>();
        return organizacaoService.DeleteAsync(id);
    }
}
