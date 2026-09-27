using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Users;
using Rank.Core.Helper;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseController
{
    public UsersController(IServiceProvider provider) : base(provider)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var userApp = _provider.GetRequiredService<UserApp>();
            var user = await userApp.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex) when (UserHelper.IsEmailAlreadyInUse(ex))
        {
            return Conflict(new { message = UserHelper.EmailAlreadyInUseMessage });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        return Ok(await userApp.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        var user = await userApp.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var userApp = _provider.GetRequiredService<UserApp>();
            var user = await userApp.UpdateAsync(id, request);
            return user is null ? NotFound() : Ok(user);
        }
        catch (Exception ex) when (UserHelper.IsEmailAlreadyInUse(ex))
        {
            return Conflict(new { message = UserHelper.EmailAlreadyInUseMessage });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        return await userApp.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
