using Microsoft.AspNetCore.Identity;

namespace TutorialsEUIdentity.Services
{
    public interface IUserManagerService 
    { 
        public async Task ensureUserRoles() {}
    }

    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagerService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        string email = "admin@admin.com";
        string password = "Test123,";

        public async Task ensureUserRoles()
        {
            var user = new IdentityUser();
            user.UserName = email;
            user.Email = email;

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
