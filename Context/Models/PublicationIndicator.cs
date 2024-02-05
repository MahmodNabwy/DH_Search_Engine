﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Context.Models;

[Table("publication_indicators")]
[Index("FilterId", Name = "IX_publication_indicators_filter_id")]
[Index("IndicatorId", Name = "IX_publication_indicators_indicator_id")]
[Index("PublicationDetailId", Name = "IX_publication_indicators_publication_detail_id")]
public partial class PublicationIndicator
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("publication_detail_id")]
    public long PublicationDetailId { get; set; }

    [Column("indicator_id")]
    public long IndicatorId { get; set; }

    [Column("value")]
    public double? Value { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("filter_id")]
    public long FilterId { get; set; }

    [ForeignKey("IndicatorId")]
    [InverseProperty("PublicationIndicators")]
    public virtual Indicator Indicator { get; set; }

    [ForeignKey("PublicationDetailId")]
    [InverseProperty("PublicationIndicators")]
    public virtual PublicationDetail PublicationDetail { get; set; }
}