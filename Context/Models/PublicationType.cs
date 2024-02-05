﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Context.Models;

[Table("publication_types")]
[Index("Name", Name = "IX_publication_types_name", IsUnique = true)]
public partial class PublicationType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Required]
    [Column("is_system")]
    public bool? IsSystem { get; set; }

    [InverseProperty("PublicationType")]
    public virtual ICollection<PublicationTypeTranslation> PublicationTypeTranslations { get; set; } = new List<PublicationTypeTranslation>();

    [InverseProperty("PublicationType")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}