using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OAuth
{
    public class AuthRequirement: IAuthorizationRequirement
    {

    }

    public class JwtRequirementHandler : AuthorizationHandler<AuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement)
        {
            context.Succeed(requirement);
        }
    }
}
