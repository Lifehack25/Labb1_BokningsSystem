using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases;
using Labb1_BokningsSystem.Services.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AdminController(
        IUseCase<AdminDtos.AdminRegisterDto, Register.Response> registerUseCase,
        IUseCase<AdminDtos.LoginAdminDto, Login.Response> loginUseCase,
        IUseCase<AdminDtos.UpdateAdminDto, UpdateAdmin.Response> updateAdminUseCase,
        IUseCase<int, DeleteAdmin.Response> deleteAdminUseCase
    ) : ControllerBase
    {
        // Register a admin user.
        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminDtos.AdminRegisterDto newAdmin)
        {
            var response = await registerUseCase.ExecuteAsync(newAdmin);
            return response.Success ? Ok() : BadRequest(response.Message);
        }

        // Logs a user in and generates a JWT token for them which will later be used for authorization endpoints.
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminDtos.LoginAdminDto loginAdmin)
        {
            var response = await loginUseCase.ExecuteAsync(loginAdmin);
            return response.Success ? Ok(new { token = response.Token }) : Unauthorized(response.Message);
        }

        // Updates admin information (name, email, password).
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateAdmin([FromBody] AdminDtos.UpdateAdminDto dto)
        {
            var response = await updateAdminUseCase.ExecuteAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Deletes an admin account.
        [HttpDelete("delete/{adminId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            var response = await deleteAdminUseCase.ExecuteAsync(adminId);
            return response.Success ? Ok(response) : NotFound(response);
        }

    }
}