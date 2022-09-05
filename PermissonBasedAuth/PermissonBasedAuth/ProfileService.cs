using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PermissonBasedAuth
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;

        public ProfileService(UserManager<IdentityUser> userManager, IUserClaimsPrincipalFactory<IdentityUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            ClaimsPrincipal userClaims = await _claimsFactory.CreateAsync(user);

            List<Claim> claims = userClaims.Claims.ToList();
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();



            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {

        }
    }
}
