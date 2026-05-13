using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenZCoders.Models.Configurations;

public class CourseRoundAssignmentConfiguration : IEntityTypeConfiguration<CourseRoundAssignment>
{
    public void Configure(EntityTypeBuilder<CourseRoundAssignment> entity)
    {
        entity.ToTable("CourseRoundAssignment");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Title).IsRequired();
        entity.Property(e => e.TotalGrade).HasPrecision(18, 2);
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");

        entity.HasOne(d => d.CourseRound)
            .WithMany()
            .HasForeignKey(d => d.CourseRoundId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_CourseRoundAssignment_CourseRound");

        entity.HasOne(d => d.Instructor)
            .WithMany(p => p.CourseRoundAssignments)
            .HasForeignKey(d => d.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_CourseRoundAssignment_Instructor");

        entity.HasOne(d => d.CourseMaterial)
            .WithMany()
            .HasForeignKey(d => d.CourseMaterialId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_CourseRoundAssignment_CourseMaterial");

        entity.HasOne(d => d.Status)
            .WithMany()
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_CourseRoundAssignment_Status");

        entity.HasMany(d => d.CourseRoundAssignmentSubmissions)
            .WithOne(s => s.Assignment)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CourseRoundAssignmentSubmission_Assignment");
    }
}
