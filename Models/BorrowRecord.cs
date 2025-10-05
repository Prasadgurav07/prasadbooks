using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace prasadbooks.Models;

public partial class BorrowRecord
{
    [Key]
    public int BorrowId { get; set; }

    public int MemberId { get; set; }

    public int BookId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public bool? IsReturned { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("BorrowRecords")]
    [JsonIgnore]
    public virtual Book? Book { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("BorrowRecords")]
    [JsonIgnore]
    public virtual Member? Member { get; set; } = null!;
}
