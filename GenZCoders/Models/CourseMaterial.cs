using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZCoders.Models
{
    public partial class CourseMaterial
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("CourseRoundID")]
        public long? CourseRoundId { get; set; }

        [Column("Created_byAccountID")]
        public long? CreatedByAccountId { get; set; }

        [Column("WeekID")]
        public long? WeekId { get; set; }

        [Column("ParentMaterialID")]
        public long? ParentMaterialId { get; set; }

        [Column("StatusID")]
        public long? StatusId { get; set; }

        [Column("MaterialTypeStatusID")]
        public long? MaterialTypeStatusId { get; set; }

        [Column("Title")]
        public string? Title { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Link")]
        public string? Link { get; set; }

        [Column("MeetingID")]
        public string? MeetingId { get; set; }

        [Column("MeetingPassword")]
        public string? MeetingPassword { get; set; }

        [Column("Score")]
        public decimal? Score { get; set; }

        // ===== Navigation properties =====

        public virtual Weeks? Week { get; set; }

        public virtual CourseMaterial? ParentMaterial { get; set; }

        public virtual ICollection<CourseMaterial> ChildMaterials { get; set; } = new List<CourseMaterial>();

        public virtual Account? CreatedBy { get; set; }

        public virtual CourseRound? CourseRound { get; set; }

        public virtual Status? Status { get; set; }

        public virtual Status? MaterialTypeStatus { get; set; }
    }
}