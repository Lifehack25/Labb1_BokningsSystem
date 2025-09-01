using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Auth;

namespace Labb1_BokningsSystem.Services;

public interface IAdminService
{
    Task<Register.Response> RegisterAsync(AdminDtos.AdminRegisterDto request);
    Task<Login.Response> LoginAsync(AdminDtos.LoginAdminDto request);
    Task<UpdateAdmin.Response> UpdateAsync(AdminDtos.UpdateAdminDto request);
    Task<DeleteAdmin.Response> DeleteAsync(int adminId);
}