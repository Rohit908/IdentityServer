using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.AuthorizationRequirements
{
    public class CustomRequirementClaim:IAuthorizationRequirement
    {
        public CustomRequirementClaim(string claimType)
        {
            CaimType = claimType;
        }
        public string CaimType { get; }

    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequirementClaim>
    {

        public CustomRequireClaimHandler()
        {

        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CustomRequirementClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.CaimType);
            if(hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtentions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomRequirementClaim(claimType));
            return builder;
        }
    }

}
