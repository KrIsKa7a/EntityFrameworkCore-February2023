namespace Blog.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data;
    using ViewModels.Article;
    using ViewModels.Genres;

    public class ArticleController : Controller
    {
        private readonly IGenreService genreService;

        public ArticleController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ICollection<ListGenreArticleAddViewModel> genres =
                await this.genreService.GetAllAsync();
            ArticleAddViewModel vm = new ArticleAddViewModel()
            {
                Genres = genres,
            };

            return View(vm);
        }
    }
}
