using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Review
{
    [Key]
    public int Id { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual DogOwner Owner { get; set; } = null!;

    public int DogWalkerId { get; set; }

    [ForeignKey(nameof(DogWalkerId))]
    public virtual DogWalker DogWalker { get; set; } = null!;

    public decimal Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime Date { get; set; }
}
