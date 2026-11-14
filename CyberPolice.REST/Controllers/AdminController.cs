using CyberPolice.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CyberPolice.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("Користувача не знайдено");
            if (!await _roleManager.RoleExistsAsync(model.Role)) return BadRequest("Такої ролі не існує");

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(new { model.Email, model.Role });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var list = new List<object>();
            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new { user.Email, Roles = roles });
            }
            return Ok(list);
        }
    }

    public class AssignRoleModel
    {
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }
}