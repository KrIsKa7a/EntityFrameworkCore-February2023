namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;

public class Game
{
    public Game()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
        this.Bets = new HashSet<Bet>();
    }

    // In real project it is good the PK to be string -> GUID
    [Key]
    public int GameId { get; set; }

    //[ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }

    public virtual Team HomeTeam { get; set; } = null!;

    //[ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }

    public virtual Team AwayTeam { get; set; } = null!;

    public byte HomeTeamGoals { get; set; }

    public byte AwayTeamGoals { get; set; }

    // DateTime is required by default! 
    // DateTime? is nullable
    public DateTime DateTime { get; set; }

    public double HomeTeamBetRate { get; set; }

    public double AwayTeamBetRate { get; set; }

    public double DrawBetRate { get; set; }
    
    [MaxLength(ValidationConstants.GameResultMaxLength)]
    public string? Result { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }
}
