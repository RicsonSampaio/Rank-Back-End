using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Membros;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;
using System.ComponentModel.DataAnnotations;

namespace Rank.Core.Service;

public class MembroService
{
    private readonly IServiceProvider _provider;

    public MembroService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<MembroResponse> CreateAsync(CreateMembroRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
        var user = await GetUsuarioParaVinculoAsync(request.IdColetivo, request.Email).ConfigureAwait(false);
        var membroRepository = _provider.GetRequiredService<MembroRepository>();
        var membro = new Membro
        {
            IdColetivo = request.IdColetivo,
            IdUsuario = user.Id,
            Email = user.Email,
            Nome = user.Name,
            FotoAccount = user.FotoAccount
        };
        var id = await membroRepository.CreateAsync(membro).ConfigureAwait(false);
        return ToResponse(membro, id);
    }

    public async Task<IReadOnlyList<MembroResponse>> GetAllAsync(int idColetivo)
    {
        var membroRepository = _provider.GetRequiredService<MembroRepository>();
        var membros = await membroRepository.GetAllAsync(idColetivo).ConfigureAwait(false);
        return membros.Select(membro => ToResponse(membro)).ToArray();
    }

    public async Task<MembroResponse?> GetByIdAsync(int id)
    {
        var membroRepository = _provider.GetRequiredService<MembroRepository>();
        var membro = await membroRepository.GetByIdAsync(id).ConfigureAwait(false);
        return membro is null ? null : ToResponse(membro);
    }

    public async Task<MembroResponse?> UpdateAsync(int id, UpdateMembroRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
        var membroRepository = _provider.GetRequiredService<MembroRepository>();
        var current = await membroRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (current is null)
            return null;

        var user = await GetUsuarioParaVinculoAsync(request.IdColetivo, request.Email).ConfigureAwait(false);
        var membro = new Membro
        {
            Id = id,
            IdColetivo = request.IdColetivo,
            IdUsuario = user.Id,
            Email = user.Email,
            Nome = user.Name,
            FotoAccount = user.FotoAccount
        };
        return await membroRepository.UpdateAsync(membro).ConfigureAwait(false) ? ToResponse(membro) : null;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var membroRepository = _provider.GetRequiredService<MembroRepository>();
        return membroRepository.DeleteAsync(id);
    }

    private async Task<User> GetUsuarioParaVinculoAsync(int idColetivo, string email)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var user = await userRepository.GetByEmailAsync(email.Trim().ToLowerInvariant()).ConfigureAwait(false);
        if (user is null)
            throw new ValidationException("Este usuário ainda não está cadastrado no sistema.");

        var coletivoRepository = _provider.GetRequiredService<ColetivoRepository>();
        var coletivo = await coletivoRepository.GetByIdAsync(idColetivo).ConfigureAwait(false);
        if (coletivo is null)
            throw new ValidationException("O coletivo informado não existe.");

        return user;
    }

    private static MembroResponse ToResponse(Membro membro, int? id = null) =>
        new(id ?? membro.Id, membro.IdColetivo, membro.IdUsuario, membro.Email, membro.Nome, membro.FotoAccount);
}
