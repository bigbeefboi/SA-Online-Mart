using Microsoft.AspNetCore.Identity;

namespace TutorialsEUIdentity.Services
{
    public interface IRoleManagerService
    {   
        public async Task ensureManagerRoles() {}
    }

    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task ensureManagerRoles()
        {
            var roles = new[] { "Admin", "Manager", "Member" };

            foreach (var role in roles) 
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
