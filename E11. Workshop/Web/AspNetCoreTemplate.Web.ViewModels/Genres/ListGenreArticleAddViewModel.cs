namespace Blog.Web.ViewModels.Genres
{
    using Data.Models;
    using Services.Mapping;

    public class ListGenreArticleAddViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
