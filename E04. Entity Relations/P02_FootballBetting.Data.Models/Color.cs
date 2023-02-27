namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;

public class Color
{
    public Color()
    {
        this.PrimaryKitTeams = new HashSet<Team>();
        this.SecondaryKitTeams = new HashSet<Team>();
    }

    [Key]
    public int ColorId { get; set; }
    
    [MaxLength(ValidationConstants.ColorNameMaxLength)]
    public string Name { get; set; } = null!;

    //[InverseProperty(nameof(Team.PrimaryKitColor))]
    public virtual ICollection<Team> PrimaryKitTeams { get; set; }

    //[InverseProperty(nameof(Team.SecondaryKitColor))]
    public virtual ICollection<Team> SecondaryKitTeams { get; set; }
}
