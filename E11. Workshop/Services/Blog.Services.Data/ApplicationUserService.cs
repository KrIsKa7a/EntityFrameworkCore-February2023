namespace Blog.Services.Data
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using Blog.Data.Common.Repositories;
    using Blog.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.ApplicationUser;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;

        public ApplicationUserService(IRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task CreateUserAsync(RegisterUserInputModel inputModel)
        {
            string passwordSalt = this.GenerateSalt();
            string hashedPassword = this.ComputeSha256Hash(inputModel.Password, passwordSalt);

            // In the service we assume that we have valid input model
            ApplicationUser user = new ApplicationUser()
            {
                Username = inputModel.Username,
                Email = inputModel.Email,
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
            };

            await this.userRepository.AddAsync(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task<string> GetIdByUsernameAsync(string username)
        {
            ApplicationUser user = await this.userRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

            return user?.Id;
        }

        public async Task<bool> UsernameExistsAsync(string username)
            => await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(u => u.Username == username);

        public async Task<bool> EmailExistsAsync(string email)
            => await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(u => u.Email == email);

        public async Task<bool> ValidateLoginInfoAsync(LoginInputModel inputModel)
        {
            ApplicationUser user = await this.userRepository
                .AllAsNoTracking()
                .FirstAsync(u => u.Username == inputModel.Username);

            string hashedPassword = 
                this.ComputeSha256Hash(inputModel.Password, user.PasswordSalt);
            return hashedPassword == user.Password;
        }

        private string ComputeSha256Hash(string rawData, string salt)
        {
            // Create a SHA256
            using SHA256 sha256Hash = SHA256.Create();

            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash
                .ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder
                    .Append(bytes[i].ToString("x2"));
            }

            builder.Append(salt);
            return builder.ToString();
        }

        private string GenerateSalt()
        {
            var bytes = new byte[128 / 8];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
