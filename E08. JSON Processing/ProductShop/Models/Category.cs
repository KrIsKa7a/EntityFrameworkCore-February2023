namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.CategoriesProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<CategoryProduct> CategoriesProducts { get; set; }
    }
}
