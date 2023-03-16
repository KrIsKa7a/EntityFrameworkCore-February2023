namespace CarDealer.DTOs.Import
{
    using System.Xml.Serialization;

    [XmlType("Car")]
    public class ImportCarDto
    {
        [XmlElement("make")]
        public string Make { get; set; } = null!;

        [XmlElement("model")]
        public string Model { get; set; } = null!;

        [XmlElement("traveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public ImportCarPartDto[] Parts { get; set; } = null!;
    }
}
