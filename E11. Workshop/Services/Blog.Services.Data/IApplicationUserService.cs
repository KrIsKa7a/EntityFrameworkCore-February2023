namespace Blog.Services.Data
{
    using System.Threading.Tasks;

    using Web.ViewModels.ApplicationUser;

    public interface IApplicationUserService
    {
        Task CreateUserAsync(RegisterUserInputModel inputModel);

        Task<string> GetIdByUsernameAsync(string username);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> ValidateLoginInfoAsync(LoginInputModel inputModel);
    }
}
