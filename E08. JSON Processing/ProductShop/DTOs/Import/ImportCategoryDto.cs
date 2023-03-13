namespace ProductShop.DTOs.Import
{
    using Newtonsoft.Json;

    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
