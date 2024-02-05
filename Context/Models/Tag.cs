﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Context.Models;

[Table("tags")]
public partial class Tag
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("tag_name")]
    public string TagName { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [InverseProperty("Tag")]
    public virtual ICollection<PublicationTag> PublicationTags { get; set; } = new List<PublicationTag>();

    [InverseProperty("Tag")]
    public virtual ICollection<TagTranslation> TagTranslations { get; set; } = new List<TagTranslation>();
}