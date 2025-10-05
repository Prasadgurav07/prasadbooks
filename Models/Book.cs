using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace prasadbooks.Models;

[Index("Isbn", Name = "UQ__Books__447D36EAD868FC63", IsUnique = true)]
public partial class Book
{
    [Key]
    public int BookId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(255)]
    public string Author { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(20)]
    public string Isbn { get; set; } = null!;

    public int? PublishedYear { get; set; }

    [Required(ErrorMessage ="Please Enter Valid No")]
    [Range(0, int.MaxValue, ErrorMessage = "Copies cannot be negative")]
    public int? AvailableCopies { get; set; }

    [InverseProperty("Book")]
    [JsonIgnore]
    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
