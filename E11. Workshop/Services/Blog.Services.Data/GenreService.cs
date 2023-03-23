namespace Blog.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Blog.Data.Common.Repositories;
    using Blog.Data.Models;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Genres;

    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> genreRepository;

        public GenreService(IRepository<Genre> genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<ICollection<ListGenreArticleAddViewModel>> GetAllAsync()
            => await this.genreRepository
                .AllAsNoTracking()
                .To<ListGenreArticleAddViewModel>()
                .ToArrayAsync();
    }
}
