using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rank.WebAPI.Authentication;

namespace Rank.WebAPI.Controllers;

[Authorize]
[ApiController]
public class BaseController : Controller
{
    protected readonly IServiceProvider _provider;
    protected readonly CurrentUser _currentUser;

    public BaseController(IServiceProvider provider)
    {
        _provider = provider;
        _currentUser = _provider.GetRequiredService<CurrentUser>();
    }

    [NonAction]
    public int GetUserId()
    {
        return _currentUser.Id;
    }

    [NonAction]
    public string GetUserEmail()
    {
        return _currentUser.Email;
    }

    [NonAction]
    public string GetUserName()
    {
        return _currentUser.UserName;
    }

    [NonAction]
    public bool IsAdmin()
    {
        return _currentUser.Admin;
    }

    [NonAction]
    public int? GetOrganizacaoId()
    {
        return _currentUser.IdOrganizacao;
    }

    [NonAction]
    public string? GetAvatar()
    {
        return _currentUser.FotoAccount;
    }
}
