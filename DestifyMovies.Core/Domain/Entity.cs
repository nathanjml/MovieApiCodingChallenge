using System.ComponentModel.DataAnnotations;

namespace DestifyMovies.Core.Domain;
public abstract class Entity
{
    [Key]
    public long Id { get; set; }
}

