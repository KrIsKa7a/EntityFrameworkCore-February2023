namespace CarDealer.DTOs.Import
{
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class ImportSupplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}
