namespace Blog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Validation;

    public class ApplicationUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Articles = new HashSet<Article>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(ApplicationUserValidationConstants.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(ApplicationUserValidationConstants.EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(ApplicationUserValidationConstants.PasswordMaxLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(ApplicationUserValidationConstants.PasswordSaltMaxLength)]
        public string PasswordSalt { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
