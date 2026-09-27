using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Coletivos;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;
using System.ComponentModel.DataAnnotations;

namespace Rank.Core.Service;

public class ColetivoService
{
    private readonly IServiceProvider _provider;

    public ColetivoService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<ColetivoResponse> CreateAsync(
        CreateColetivoRequest request, int idOrganizacao)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
        if (idOrganizacao <= 0)
            throw new ArgumentOutOfRangeException(nameof(idOrganizacao));

        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        var now = DateTime.UtcNow;
        var coletivo = new Coletivo
        {
            Nome = request.Nome.Trim(),
            DataCriacao = now,
            DataAtualizacao = now,
            IdOrganizacao = idOrganizacao,
            IdTipoColetivo = (int)request.IdTipoColetivo,
            Logo = request.Logo?.Trim()
        };
        var id = await coletivoRepository.CreateAsync(coletivo).ConfigureAwait(false);
        return new ColetivoResponse(id, coletivo.Nome, coletivo.DataCriacao, coletivo.DataAtualizacao,
            coletivo.IdOrganizacao, coletivo.IdTipoColetivo, coletivo.Logo);
    }

    public async Task<IReadOnlyList<ColetivoResponse>> GetAllAsync(int idUsuario)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var user = await userRepository.GetByIdAsync(idUsuario).ConfigureAwait(false);
        if (user is null || !user.IsActive)
            return Array.Empty<ColetivoResponse>();

        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        if (user.Admin)
        {
            var coletivos = await coletivoRepository.GetAllAsync().ConfigureAwait(false);
            return coletivos.Select(ToResponse).ToArray();
        }

        IReadOnlyList<Coletivo> coletivosDaOrganizacao = Array.Empty<Coletivo>();
        if (user.IdOrganizacao is > 0)
            coletivosDaOrganizacao = await coletivoRepository.GetByOrganizacaoAsync(
                user.IdOrganizacao.Value).ConfigureAwait(false);

        var coletivosVinculados = await coletivoRepository.GetByUsuarioVinculadoAsync(idUsuario).ConfigureAwait(false);
        return coletivosDaOrganizacao.Concat(coletivosVinculados)
            .DistinctBy(coletivo => coletivo.Id)
            .OrderBy(coletivo => coletivo.Id)
            .Select(ToResponse)
            .ToArray();
    }

    public async Task<ColetivoResponse?> GetByIdAsync(int id)
    {
        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        var coletivo = await coletivoRepository.GetByIdAsync(id).ConfigureAwait(false);
        return coletivo is null ? null : ToResponse(coletivo);
    }

    public async Task<ColetivoResponse?> UpdateAsync(
        int id, UpdateColetivoRequest request)
    {
        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        var current = await coletivoRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (current is null)
            return null;

        var coletivo = new Coletivo
        {
            Id = id,
            Nome = request.Nome.Trim(),
            DataCriacao = current.DataCriacao,
            DataAtualizacao = DateTime.UtcNow,
            IdOrganizacao = current.IdOrganizacao,
            IdTipoColetivo = current.IdTipoColetivo,
            Logo = request.Logo?.Trim()
        };
        return await coletivoRepository.UpdateAsync(coletivo).ConfigureAwait(false)
            ? ToResponse(coletivo) : null;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        return coletivoRepository.DeleteAsync(id);
    }

    private static ColetivoResponse ToResponse(Coletivo coletivo) =>
        new(coletivo.Id, coletivo.Nome, coletivo.DataCriacao, coletivo.DataAtualizacao,
            coletivo.IdOrganizacao, coletivo.IdTipoColetivo, coletivo.Logo);
}
