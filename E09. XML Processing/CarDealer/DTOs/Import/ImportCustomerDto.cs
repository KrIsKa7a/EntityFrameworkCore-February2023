namespace CarDealer.DTOs.Import
{
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        // Always read datetime, enums and other hard to parse data types as string
        // Parse it yourself in your business logic
        // JsonConvert and XmlSerializer are not capable of parsing!!!
        [XmlElement("birthDate")]
        public string BirthDate { get; set; } = null!;

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
