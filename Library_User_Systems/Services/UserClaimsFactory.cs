using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Library_User_Systems.Models;
using Library_User_Systems.Data;

namespace Library_User_Systems.Services
{
    /// <summary>
    /// This creates a custom claim for an applicationUser.
    /// </summary>
    public class UserClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        /// <summary>
        /// This initialize a new instance of this class.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="optionsAccessor"></param>
        public UserClaimsFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor) { }

        /// <summary>
        /// This generates a claims identity for the user that is passed through. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("FullName", user.FullName ?? ""));
            identity.AddClaim(new Claim("UserType", user.UserType ?? "Student"));

            return identity;
        }
    }
}