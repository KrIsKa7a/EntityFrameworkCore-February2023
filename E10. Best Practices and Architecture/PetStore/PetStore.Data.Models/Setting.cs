namespace PetStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Value { get; set; } = null!;
    }
}
