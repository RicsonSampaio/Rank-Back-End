using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Coletivos;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class ColetivoController : BaseController
{
    public ColetivoController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateColetivoRequest request)
    {
        var idOrganizacao = GetOrganizacaoId();
        if (idOrganizacao is null or <= 0)
            return StatusCode(StatusCodes.Status403Forbidden,
                new { message = "O token deve conter uma organização válida. Faça login novamente." });

        var coletivoApp = _provider.GetRequiredService<ColetivoApp>();
        var coletivo = await coletivoApp.CreateAsync(request, idOrganizacao.Value);
        return CreatedAtAction(nameof(GetById), new { id = coletivo.Id }, coletivo);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coletivoApp = _provider.GetRequiredService<ColetivoApp>();
        return Ok(await coletivoApp.GetAllAsync(GetUserId()));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var coletivoApp = _provider.GetRequiredService<ColetivoApp>();
        var coletivo = await coletivoApp.GetByIdAsync(id);
        return coletivo is null ? NotFound() : Ok(coletivo);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateColetivoRequest request)
    {
        var coletivoApp = _provider.GetRequiredService<ColetivoApp>();
        var coletivo = await coletivoApp.UpdateAsync(id, request);
        return coletivo is null ? NotFound() : Ok(coletivo);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var coletivoApp = _provider.GetRequiredService<ColetivoApp>();
        return await coletivoApp.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
