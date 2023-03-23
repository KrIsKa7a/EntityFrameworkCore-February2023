namespace Blog.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.Genres;

    public interface IGenreService
    {
        Task<ICollection<ListGenreArticleAddViewModel>> GetAllAsync();
    }
}
