using Backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;
    private readonly string[] _types;


    public AuthorizeAttribute() { }

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }


    public AuthorizeAttribute(string[] roles = null, string[] types = null)
    {
        _roles = roles;
        _types = types;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Message = "Требуется авторизация"
            });
            return;
        }

        var user = context.HttpContext.User;
        if (_roles != null && _roles.Length > 0)
        {
            var hasRequiredRole = _roles.Any(role => user.IsInRole(role));
            if (!hasRequiredRole)
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Недостаточно прав (роль)"
                })
                { StatusCode = 403 };
                return;
            }
        }

        if (_types != null && _types.Length > 0)
        {
            var userType = user.FindFirst("UserType")?.Value;
            var hasRequiredType = _types.Any(type => type == userType);

            if (!hasRequiredType)
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Недостаточно прав (тип)"
                })
                { StatusCode = 403 };
                return;
            }
        }
    }
}