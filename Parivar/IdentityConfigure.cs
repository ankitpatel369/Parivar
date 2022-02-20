using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Parivar.Data.DbContext;
using Parivar.Dto.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parivar
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<FamilyUser, Role>
    {
        public ClaimsPrincipalFactory(UserManager<FamilyUser> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(FamilyUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault()?.Value ?? "";
            var claims = new List<Claim>()
            {
                new Claim("UserRole", role),
                new Claim("DisplayUserRole", ""),
                new Claim("UserId", user.Id.ToString() ?? ""),
                new Claim("FullName", user.FullName ??"" ),
                new Claim("ProfilePic", $@"\{FilePathList.ProfilePic}\{user.ProfilePic}" ?? @"/UploadFile/UserProfile/user.png"),
            };
            identity.AddClaims(claims);
            return identity;
        }
    }
}
