namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;

public class Player
{
    public Player()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
    }

    [Key]
    public int PlayerId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.PlayerNameMaxLength)]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }

    // SQL Type -> BIT
    // By default bool is NOT NULL, by default is required
    public bool IsInjured { get; set; }

    // This FK should not be NOT NULL
    // Warning: This may cause a problem in Judge!!!
    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }
    
    public virtual Position Position { get; set; } = null!; 

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}
