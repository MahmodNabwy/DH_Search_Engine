﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Context.Models;

[Table("periodic_translations")]
[Index("PeriodicId", "Locale", Name = "IX_periodic_translations_periodic_id_locale", IsUnique = true)]
public partial class PeriodicTranslation
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("locale")]
    [StringLength(6)]
    public string Locale { get; set; }

    [Column("periodic_id")]
    public long PeriodicId { get; set; }

    [ForeignKey("PeriodicId")]
    [InverseProperty("PeriodicTranslations")]
    public virtual Periodic Periodic { get; set; }
}