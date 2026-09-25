using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rank.WebAPI.Controllers;

[Authorize]
[ApiController]
public class BaseController : Controller
{
    protected readonly IServiceProvider _provider;

    public BaseController(IServiceProvider provider)
    {
        _provider = provider;
    }
}
