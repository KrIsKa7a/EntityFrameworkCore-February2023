namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class PlayerStatistic
{
    // Here we have composite PK -> We will use FluentAPI for config it
    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }

    public virtual Game Game { get; set; } = null!;

    [ForeignKey(nameof(Player))]
    public int PlayerId { get; set; }

    public virtual Player Player { get; set; } = null!;

    // Warning: Judge may not be happy with byte
    public byte ScoredGoals { get; set; }

    public byte Assists { get; set; }

    public byte MinutesPlayed { get; set; }
}
