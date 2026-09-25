using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rank.Application.App;
using Rank.Core.DTO.Request.Users;
using Rank.Core.Service;

namespace Rank.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class UsersController(UserApp users) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await users.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await users.GetAllAsync(cancellationToken));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await users.UpdateAsync(id, request, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        return await users.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
