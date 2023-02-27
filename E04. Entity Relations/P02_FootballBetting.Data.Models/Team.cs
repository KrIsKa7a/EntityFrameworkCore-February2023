namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;

public class Team
{
    public Team()
    {
        this.HomeGames = new HashSet<Game>();
        this.AwayGames = new HashSet<Game>();

        this.Players = new HashSet<Player>();
    }

    [Key] // Sets PK of the table -> UNIQUE, NOT NULL
    public int TeamId { get; set; }

    [MaxLength(ValidationConstants.TeamNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.TeamLogoUrlMaxLength)]
    public string? LogoUrl { get; set; }

    [MaxLength(ValidationConstants.TeamInitialsMaxLength)]
    public string Initials { get; set; } = null!;

    // Required (NOT NULL) by default because decimal data type is not nullable
    public decimal Budget { get; set; }

    //[ForeignKey(nameof(PrimaryKitColor))]
    public int PrimaryKitColorId { get; set; }

    public virtual Color PrimaryKitColor { get; set; } = null!;

    //[ForeignKey(nameof(SecondaryKitColor))]
    public int SecondaryKitColorId { get; set; }

    public virtual Color SecondaryKitColor { get; set; } = null!;

    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }

    public virtual Town Town { get; set; } = null!;

    //[InverseProperty(nameof(Game.HomeTeam))]
    public virtual ICollection<Game> HomeGames { get; set; }

    //[InverseProperty(nameof(Game.AwayTeam))]
    public virtual ICollection<Game> AwayGames { get; set; }

    public virtual ICollection<Player> Players { get; set; }
}