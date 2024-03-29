﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Context.Models;

[Table("publication_tags")]
[Index("PublicationId", Name = "IX_publication_tags_publication_id")]
[Index("TagId", Name = "IX_publication_tags_tag_id")]
public partial class PublicationTag
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("publication_id")]
    public long PublicationId { get; set; }

    [Column("tag_id")]
    public long TagId { get; set; }

    [Required]
    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [ForeignKey("PublicationId")]
    [InverseProperty("PublicationTags")]
    public virtual Publication Publication { get; set; }

    [ForeignKey("TagId")]
    [InverseProperty("PublicationTags")]
    public virtual Tag Tag { get; set; }
}