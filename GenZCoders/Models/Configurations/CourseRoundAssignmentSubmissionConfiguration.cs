using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenZCoders.Models.Configurations;

public class CourseRoundAssignmentSubmissionConfiguration : IEntityTypeConfiguration<CourseRoundAssignmentSubmission>
{
    public void Configure(EntityTypeBuilder<CourseRoundAssignmentSubmission> entity)
    {
        entity.ToTable("CourseRoundAssignmentSubmission");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.SubmissionLink).IsRequired();
        entity.Property(e => e.Grade).HasPrecision(18, 2);
        entity.Property(e => e.SubmittedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");

        entity.HasIndex(e => new { e.AssignmentId, e.StudentId })
            .IsUnique()
            .HasDatabaseName("UQ_CourseRoundAssignmentSubmission_Assignment_Student");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.CourseRoundAssignmentSubmission)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_CourseRoundAssignmentSubmission_Student");

        entity.HasOne(d => d.Status)
            .WithMany()
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_CourseRoundAssignmentSubmission_Status");
    }
}
