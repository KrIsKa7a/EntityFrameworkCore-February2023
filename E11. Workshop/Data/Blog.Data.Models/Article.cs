namespace Blog.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Validation;

    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ArticleValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ArticleValidationConstants.ContentMaxLength)]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = null!;

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
