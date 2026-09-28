using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Tarefas;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class TarefaController : BaseController
{
    public TarefaController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTarefaRequest request)
    {
        var tarefaApp = _provider.GetRequiredService<TarefaApp>();
        var tarefa = await tarefaApp.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery, BindRequired] int idColetivo)
    {
        var tarefaApp = _provider.GetRequiredService<TarefaApp>();
        return Ok(await tarefaApp.GetAllAsync(idColetivo));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tarefaApp = _provider.GetRequiredService<TarefaApp>();
        var tarefa = await tarefaApp.GetByIdAsync(id);
        return tarefa is null ? NotFound() : Ok(tarefa);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTarefaRequest request)
    {
        var tarefaApp = _provider.GetRequiredService<TarefaApp>();
        var tarefa = await tarefaApp.UpdateAsync(id, request);
        return tarefa is null ? NotFound() : Ok(tarefa);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tarefaApp = _provider.GetRequiredService<TarefaApp>();
        return await tarefaApp.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
