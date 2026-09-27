using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Organizacoes;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;
using System.ComponentModel.DataAnnotations;

namespace Rank.Core.Service;

public class OrganizacaoService
{
    private readonly IServiceProvider _provider;

    public OrganizacaoService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<OrganizacaoResponse> CreateAsync(
        CreateOrganizacaoRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
        var organizacaoRepository = _provider.GetRequiredService<OrganizacaoRepository>();
        var organizacao = new Organizacao
        {
            Nome = request.Nome.Trim(),
            DataCriacao = DateTime.UtcNow,
            Logo = request.Logo?.Trim()
        };
        var id = await organizacaoRepository.CreateAsync(organizacao).ConfigureAwait(false);
        return new OrganizacaoResponse(id, organizacao.Nome, organizacao.DataCriacao, organizacao.Logo);
    }

    public async Task<IReadOnlyList<OrganizacaoResponse>> GetAllAsync()
    {
        var organizacaoRepository = _provider.GetRequiredService<OrganizacaoRepository>();
        var organizacoes = await organizacaoRepository.GetAllAsync().ConfigureAwait(false);
        return organizacoes.Select(ToResponse).ToArray();
    }

    public async Task<OrganizacaoResponse?> GetByIdAsync(int id)
    {
        var organizacaoRepository = _provider.GetRequiredService<OrganizacaoRepository>();
        var organizacao = await organizacaoRepository.GetByIdAsync(id).ConfigureAwait(false);
        return organizacao is null ? null : ToResponse(organizacao);
    }

    public async Task<OrganizacaoResponse?> UpdateAsync(
        int id, UpdateOrganizacaoRequest request)
    {
        var organizacaoRepository = _provider.GetRequiredService<OrganizacaoRepository>();
        var current = await organizacaoRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (current is null)
            return null;

        var organizacao = new Organizacao
        {
            Id = id,
            Nome = request.Nome.Trim(),
            DataCriacao = current.DataCriacao,
            Logo = request.Logo?.Trim()
        };
        return await organizacaoRepository.UpdateAsync(organizacao).ConfigureAwait(false)
            ? ToResponse(organizacao) : null;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var organizacaoRepository = _provider.GetRequiredService<OrganizacaoRepository>();
        return organizacaoRepository.DeleteAsync(id);
    }

    private static OrganizacaoResponse ToResponse(Organizacao organizacao) =>
        new(organizacao.Id, organizacao.Nome, organizacao.DataCriacao, organizacao.Logo);
}
