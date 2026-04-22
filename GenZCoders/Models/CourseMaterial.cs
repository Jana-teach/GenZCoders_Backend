using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZCoders.Models
{
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
            public long CourseRoundId { get; set; }

            [Column("Created_byAccountID")]
            public long CreatedByAccountId { get; set; }

            [Column("WeekID")]
            public long WeekId { get; set; }

            [Column("ParentMaterialID")]
            public long? ParentMaterialId { get; set; }

            [Column("StatusID")]
            public long StatusId { get; set; }

            [Column("MaterialTypeStatusID")]
            public long MaterialTypeStatusId { get; set; }

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

            // ===== Navigation properties =====

            // Parent Week
            public virtual Weeks Week { get; set; }

            // Parent Material for self-referencing
            public virtual CourseMaterial? ParentMaterial { get; set; }

            // Collection of child materials
            public virtual ICollection<CourseMaterial> ChildMaterials { get; set; } = new List<CourseMaterial>();

            // Creator Account
            public virtual Account CreatedBy { get; set; }

            // Related CourseRound
            public virtual CourseRound CourseRound { get; set; }

            // Status
            public virtual Status Status { get; set; }

            // Material type status
            public virtual Status MaterialTypeStatus { get; set; }
        }
    }

}

