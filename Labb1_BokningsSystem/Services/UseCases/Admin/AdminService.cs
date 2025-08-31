using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Auth;

namespace Labb1_BokningsSystem.Services;

public class AdminService(Register register, Login login, UpdateAdmin updateAdmin, DeleteAdmin deleteAdmin) : IAdminService
{
    public async Task<Register.Response> RegisterAsync(AdminDtos.AdminRegisterDto request)
        => await register.ExecuteAsync(request);

    public async Task<Login.Response> LoginAsync(AdminDtos.LoginAdminDto request)
        => await login.ExecuteAsync(request);

    public async Task<UpdateAdmin.Response> UpdateAsync(AdminDtos.UpdateAdminDto request)
        => await updateAdmin.ExecuteAsync(request);

    public async Task<DeleteAdmin.Response> DeleteAsync(int adminId)
        => await deleteAdmin.ExecuteAsync(adminId);
}