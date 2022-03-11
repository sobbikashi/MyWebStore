using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string DefaultAdminPassword = "AdPass_123";
    }

    public class Role : IdentityRole
    {
        public const string Administrators = "Administrators";
        public const string Users = "Users";
    }
}
