namespace FastFood.Web.ViewModels.Items
{
    using System.ComponentModel.DataAnnotations;
    using Common.EntityConfiguration;

    public class CreateItemInputModel
    {
        [MinLength(ViewModelsValidation.ItemNameMinLength)]
        [MaxLength(ViewModelsValidation.ItemNameMaxLength)]
        public string Name { get; set; } = null!;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
