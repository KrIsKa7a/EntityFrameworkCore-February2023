namespace MusicHub.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Enums;

public class Song
{
    public Song()
    {
        this.SongPerformers = new HashSet<SongPerformer>();
    }

    [Key]
    public int Id { get; set; }

    // In EF <= 3.1.x we use [Required] attribute
    // In Ef >= 6.x everything is required and we add "?" to be nullable
    [MaxLength(ValidationConstants.SongNameMaxLength)]
    public string Name { get; set; } = null!; //This is required!

    // TimeSpan datatype is required by default!
    // In the DB this will be stored as BIGINT <=> Ticks count
    public TimeSpan Duration { get; set; }

    public DateTime CreatedOn { get; set; }

    // Enumerations are stored in the DB as INT
    public Genre Genre { get; set; }

    [ForeignKey(nameof(Album))]
    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }

    [ForeignKey(nameof(Writer))]
    public int WriterId { get; set; }

    public virtual Writer Writer { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<SongPerformer> SongPerformers { get; set; }
}
