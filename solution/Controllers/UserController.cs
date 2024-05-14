using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Seeding;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(MainContext context) : ControllerBase
    {
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(uint id)
        {
            try
            {
                User? user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                Log.Information($"The use with Login={user.Login} has been deleted.");
                return Ok(new { message = $"User with ID {user.Id} has been successfully deleted." });
            }
            catch(Exception ex)
            {
                Log.Error(ex, "An error occurred during user deletion.");
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<User>> GetAllUsers()
        {
            return await context.Users.AsNoTracking().ToListAsync();
        }
    }
}