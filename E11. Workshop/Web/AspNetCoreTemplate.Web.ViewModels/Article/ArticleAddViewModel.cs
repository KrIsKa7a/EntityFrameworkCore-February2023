namespace Blog.Web.ViewModels.Article
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Validation;
    using Genres;

    public class ArticleAddViewModel
    {
        [Required]
        [MinLength(ArticleAddValidationConstants.TitleMinLength)]
        [MaxLength(ArticleAddValidationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ArticleAddValidationConstants.ContentMinLength)]
        [MaxLength(ArticleAddValidationConstants.ContentMaxLength)]
        public string Content { get; set; }

        public int GenreId { get; set; } // Selected Genre

        public ICollection<ListGenreArticleAddViewModel> Genres { get; set; }
    }
}
