namespace Blog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Validation;

    public class Genre
    {
        public Genre()
        {
            this.Articles = new HashSet<Article>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreValidationConstants.GenreNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
