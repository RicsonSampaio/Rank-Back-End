using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Membros;
using System.ComponentModel.DataAnnotations;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class MembroController : BaseController
{
    public MembroController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMembroRequest request)
    {
        try
        {
            var membroApp = _provider.GetRequiredService<MembroApp>();
            var membro = await membroApp.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = membro.Id }, membro);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery, BindRequired, Range(1, int.MaxValue)] int idColetivo)
    {
        var membroApp = _provider.GetRequiredService<MembroApp>();
        return Ok(await membroApp.GetAllAsync(idColetivo));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var membroApp = _provider.GetRequiredService<MembroApp>();
        var membro = await membroApp.GetByIdAsync(id);
        return membro is null ? NotFound() : Ok(membro);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMembroRequest request)
    {
        try
        {
            var membroApp = _provider.GetRequiredService<MembroApp>();
            var membro = await membroApp.UpdateAsync(id, request);
            return membro is null ? NotFound() : Ok(membro);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var membroApp = _provider.GetRequiredService<MembroApp>();
        return await membroApp.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
