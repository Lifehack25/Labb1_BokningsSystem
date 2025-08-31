using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        // Register a admin user.
        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminDtos.AdminRegisterDto newAdmin)
        {
            var response = await adminService.RegisterAsync(newAdmin);
            return response.Success ? Ok() : BadRequest(response.Message);
        }

        // Logs a user in and generates a JWT token for them which will later be used for authorization endpoints.
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminDtos.LoginAdminDto loginAdmin)
        {
            var response = await adminService.LoginAsync(loginAdmin);
            return response.Success ? Ok(new { token = response.Token }) : Unauthorized(response.Message);
        }

        // Updates admin information (name, email, password).
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateAdmin([FromBody] AdminDtos.UpdateAdminDto dto)
        {
            var response = await adminService.UpdateAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Deletes an admin account.
        [HttpDelete("delete/{adminId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            var response = await adminService.DeleteAsync(adminId);
            return response.Success ? Ok(response) : NotFound(response);
        }

    }
}