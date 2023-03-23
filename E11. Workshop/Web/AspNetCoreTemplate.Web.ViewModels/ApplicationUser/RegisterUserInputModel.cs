namespace Blog.Web.ViewModels.ApplicationUser
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Common.Validation;
    using Data.Models;
    using Services.Mapping;

    public class RegisterUserInputModel
    {
        [Required]
        [MinLength(ApplicationUserValidationConstants.UsernameMinLength)]
        [MaxLength(ApplicationUserValidationConstants.UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(ApplicationUserValidationConstants.EmailMinLength)]
        [MaxLength(ApplicationUserValidationConstants.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(ApplicationUserValidationConstants.PasswordMinLength)]
        [MaxLength(ApplicationUserValidationConstants.PasswordMaxLength)]
        public string Password { get; set; } // Not hashed, just password

        [Required]
        [MinLength(ApplicationUserValidationConstants.PasswordMinLength)]
        [MaxLength(ApplicationUserValidationConstants.PasswordMaxLength)]
        public string PasswordConfirmation { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<RegisterUserInputModel, ApplicationUser>()
        //        .ForMember(d => d.Password, opt => opt.Ignore())
        //        .ForSourceMember(s => s.PasswordConfirmation, opt => opt.DoNotValidate())
        //        .ForMember(d => d.Articles, opt => opt.Ignore())
        //        .ForMember(d => d.PasswordSalt, opt => opt.Ignore());
        //}
    }
}
