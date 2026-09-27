using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Organizacoes;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class OrganizacaoController : BaseController
{
    public OrganizacaoController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var organizacaoApp = _provider.GetRequiredService<OrganizacaoApp>();
        return Ok(await organizacaoApp.GetAllAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var organizacaoApp = _provider.GetRequiredService<OrganizacaoApp>();
        var organizacao = await organizacaoApp.GetByIdAsync(id);
        return organizacao is null ? NotFound() : Ok(organizacao);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateOrganizacaoRequest request)
    {
        var organizacaoApp = _provider.GetRequiredService<OrganizacaoApp>();
        var organizacao = await organizacaoApp.UpdateAsync(id, request);
        return organizacao is null ? NotFound() : Ok(organizacao);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var organizacaoApp = _provider.GetRequiredService<OrganizacaoApp>();
        return await organizacaoApp.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
