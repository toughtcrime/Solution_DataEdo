using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Seeding
{
    public class SeedDb(MainContext context)
    {
        public void Run()
        {
            var users = new List<User>()
            {
                new User { Id = 1, Login = "Biba", Password = "StrongQwertyPassword"},
                new User{ Id = 2, Login = "Boba", Password = "Strong123Password"}
            };

            context.AddRange(users);
            context.SaveChanges();

        }
    }
}
