using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AdminController(IAdminService _useCase) : ControllerBase
    {
        private readonly IAdminService useCase = _useCase;
        
        // Register a admin user.
        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminDtos.AdminRegisterDto newAdmin)
        {
            var response = await useCase.RegisterAsync(newAdmin);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }

        // Logs a user in and generates a JWT token for them which will later be used for authorization endpoints.
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminDtos.LoginAdminDto loginAdmin)
        {
            var response = await useCase.LoginAsync(loginAdmin);
            return response.Success ? Ok(response) : Unauthorized(response.Message);
        }

        // Updates admin information (name, email, password).
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateAdmin(AdminDtos.UpdateAdminDto dto)
        {
            var response = await useCase.UpdateAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Deletes an admin account.
        [HttpDelete("delete/{adminId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            var response = await useCase.DeleteAsync(adminId);
            return response.Success ? Ok(response) : NotFound(response);
        }

    }
}