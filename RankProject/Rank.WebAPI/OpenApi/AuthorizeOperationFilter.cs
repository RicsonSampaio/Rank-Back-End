using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rank.WebAPI.OpenApi;

public sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;
        if (method.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
            return;

        var needsAuthorization = method.IsDefined(typeof(AuthorizeAttribute), inherit: true)
            || method.DeclaringType?.IsDefined(typeof(AuthorizeAttribute), inherit: true) == true;
        if (!needsAuthorization)
            return;

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            }] = Array.Empty<string>()
        });
    }
}
