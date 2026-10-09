using GeoPath.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeoPath.Infrastructure;

/// <summary>Maps GeoPath domain entities to the PostgreSQL relational schema.</summary>
public sealed class GeoPathDbContext : DbContext
{
    /// <summary>Creates the context using the configured EF Core options.</summary>
    /// <param name="options">Provider and connection options for this context.</param>
    public GeoPathDbContext(DbContextOptions<GeoPathDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets the registered platform role definitions.</summary>
    public DbSet<RoleDefinition> Roles => Set<RoleDefinition>();

    /// <summary>Gets persisted user accounts.</summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>Gets curriculum topics.</summary>
    public DbSet<GeometryTopic> Topics => Set<GeometryTopic>();

    /// <summary>Gets geometry practice examples.</summary>
    public DbSet<GeometryExample> Examples => Set<GeometryExample>();

    /// <summary>Gets geometry theorems.</summary>
    public DbSet<Theorem> Theorems => Set<Theorem>();

    /// <summary>Gets geometry definitions.</summary>
    public DbSet<GeometryDefinition> Definitions => Set<GeometryDefinition>();

    /// <summary>Gets example-to-theorem relationships.</summary>
    public DbSet<ExampleTheorem> ExampleTheorems => Set<ExampleTheorem>();

    /// <summary>Gets example-to-definition relationships.</summary>
    public DbSet<ExampleDefinition> ExampleDefinitions => Set<ExampleDefinition>();

    /// <summary>Gets teacher-owned classrooms.</summary>
    public DbSet<Classroom> Classrooms => Set<Classroom>();

    /// <summary>Gets learner classroom enrollments.</summary>
    public DbSet<ClassEnrollment> ClassEnrollments => Set<ClassEnrollment>();

    /// <summary>Gets classroom learning assignments.</summary>
    public DbSet<LearningAssignment> Assignments => Set<LearningAssignment>();

    /// <summary>Gets learner attempts at practice examples.</summary>
    public DbSet<LearningAttempt> Attempts => Set<LearningAttempt>();

    /// <summary>Gets learner outcomes for individual attempt stages.</summary>
    public DbSet<AttemptStageResult> AttemptStageResults => Set<AttemptStageResult>();

    /// <summary>Configures entity tables, constraints, relationships, and initial reference content.</summary>
    /// <param name="modelBuilder">The model builder used to define the relational schema.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoleDefinition>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(role => role.Id);
            entity.Property(role => role.Id).HasConversion<string>().HasMaxLength(24);
            entity.Property(role => role.Name).HasMaxLength(24).IsRequired();
            entity.HasIndex(role => role.Name).IsUnique();
            entity.HasData(
                new RoleDefinition { Id = UserRole.Visitor, Name = "Visitor" },
                new RoleDefinition { Id = UserRole.Learner, Name = "Learner" },
                new RoleDefinition { Id = UserRole.Teacher, Name = "Teacher" },
                new RoleDefinition { Id = UserRole.Author, Name = "Author" },
                new RoleDefinition { Id = UserRole.Admin, Name = "Admin" });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Name).HasMaxLength(120).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(320).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(256).IsRequired();
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(24);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasOne<RoleDefinition>()
                .WithMany()
                .HasForeignKey(user => user.Role)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<GeometryTopic>(entity =>
        {
            entity.ToTable("topics");
            entity.HasKey(topic => topic.Id);
            entity.Property(topic => topic.Name).HasMaxLength(120).IsRequired();
            entity.Property(topic => topic.Description).HasMaxLength(2_000);
            entity.HasIndex(topic => topic.Name).IsUnique();
            entity.HasData(new GeometryTopic
            {
                Id = SeedData.CircleGeometryTopicId,
                Name = "Circle geometry",
                Description = "Angles, chords, and circle theorems.",
            });
        });

        modelBuilder.Entity<GeometryExample>(entity =>
        {
            entity.ToTable("examples");
            entity.HasKey(example => example.Id);
            entity.Property(example => example.Title).HasMaxLength(180).IsRequired();
            entity.Property(example => example.Prompt).HasMaxLength(8_000).IsRequired();
            entity.Property(example => example.Difficulty).HasConversion<string>().HasMaxLength(16);
            entity.Property(example => example.GuidedFlowJson).HasColumnType("jsonb");
            entity.HasIndex(example => new { example.IsPublished, example.Title });
            entity.HasOne(example => example.Topic)
                .WithMany()
                .HasForeignKey(example => example.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(new GeometryExample
            {
                Id = SeedData.SameSegmentExampleId,
                TopicId = SeedData.CircleGeometryTopicId,
                Title = "Angles in the same segment",
                Prompt = "Identify the angles subtended by chord AB at points C and D on the same segment.",
                Difficulty = ExampleDifficulty.Medium,
                IsPublished = true,
                CreatedAtUtc = new DateTimeOffset(2026, 10, 9, 0, 0, 0, TimeSpan.Zero),
            });
        });

        modelBuilder.Entity<Theorem>(entity =>
        {
            entity.ToTable("theorems");
            entity.HasKey(theorem => theorem.Id);
            entity.Property(theorem => theorem.Name).HasMaxLength(180).IsRequired();
            entity.Property(theorem => theorem.Statement).HasMaxLength(4_000).IsRequired();
            entity.Property(theorem => theorem.Explanation).HasMaxLength(8_000);
            entity.HasIndex(theorem => new { theorem.IsPublished, theorem.Name });
            entity.HasData(new Theorem
            {
                Id = SeedData.SameSegmentTheoremId,
                Name = "Angles in the same segment",
                Statement = "Angles subtended by the same chord at the circumference, on the same segment, are equal.",
                Explanation = "The two angles stand on the same chord and lie in the same segment of the circle.",
                IsPublished = true,
            });
        });

        modelBuilder.Entity<GeometryDefinition>(entity =>
        {
            entity.ToTable("definitions");
            entity.HasKey(definition => definition.Id);
            entity.Property(definition => definition.Name).HasMaxLength(180).IsRequired();
            entity.Property(definition => definition.Description).HasMaxLength(4_000).IsRequired();
            entity.HasIndex(definition => new { definition.IsPublished, definition.Name });
            entity.HasData(new GeometryDefinition
            {
                Id = SeedData.ChordDefinitionId,
                Name = "Chord",
                Description = "A chord is a straight line segment joining two points on a circle.",
                IsPublished = true,
            });
        });

        modelBuilder.Entity<ExampleTheorem>(entity =>
        {
            entity.ToTable("example_theorems");
            entity.HasKey(link => new { link.ExampleId, link.TheoremId });
            entity.HasOne(link => link.Example).WithMany(example => example.Theorems)
                .HasForeignKey(link => link.ExampleId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(link => link.Theorem).WithMany()
                .HasForeignKey(link => link.TheoremId).OnDelete(DeleteBehavior.Restrict);
            entity.HasData(new ExampleTheorem
            {
                ExampleId = SeedData.SameSegmentExampleId,
                TheoremId = SeedData.SameSegmentTheoremId,
            });
        });

        modelBuilder.Entity<ExampleDefinition>(entity =>
        {
            entity.ToTable("example_definitions");
            entity.HasKey(link => new { link.ExampleId, link.DefinitionId });
            entity.HasOne(link => link.Example).WithMany(example => example.Definitions)
                .HasForeignKey(link => link.ExampleId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(link => link.Definition).WithMany()
                .HasForeignKey(link => link.DefinitionId).OnDelete(DeleteBehavior.Restrict);
            entity.HasData(new ExampleDefinition
            {
                ExampleId = SeedData.SameSegmentExampleId,
                DefinitionId = SeedData.ChordDefinitionId,
            });
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.ToTable("classes");
            entity.HasKey(classroom => classroom.Id);
            entity.Property(classroom => classroom.Name).HasMaxLength(160).IsRequired();
            entity.Property(classroom => classroom.JoinCodeHash).HasMaxLength(128).IsRequired();
            entity.HasIndex(classroom => classroom.JoinCodeHash).IsUnique();
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(classroom => classroom.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ClassEnrollment>(entity =>
        {
            entity.ToTable("class_enrollments");
            entity.HasKey(enrollment => enrollment.Id);
            entity.Property(enrollment => enrollment.Status).HasConversion<string>().HasMaxLength(24);
            entity.HasIndex(enrollment => new { enrollment.ClassroomId, enrollment.LearnerId }).IsUnique();
            entity.HasOne<Classroom>().WithMany()
                .HasForeignKey(enrollment => enrollment.ClassroomId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>().WithMany()
                .HasForeignKey(enrollment => enrollment.LearnerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LearningAssignment>(entity =>
        {
            entity.ToTable("assignments");
            entity.HasKey(assignment => assignment.Id);
            entity.HasOne<Classroom>().WithMany()
                .HasForeignKey(assignment => assignment.ClassroomId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GeometryExample>().WithMany()
                .HasForeignKey(assignment => assignment.ExampleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>().WithMany()
                .HasForeignKey(assignment => assignment.CreatedByTeacherId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(assignment => new { assignment.ClassroomId, assignment.DueAtUtc });
        });

        modelBuilder.Entity<LearningAttempt>(entity =>
        {
            entity.ToTable("attempts");
            entity.HasKey(attempt => attempt.Id);
            entity.Property(attempt => attempt.Status).HasConversion<string>().HasMaxLength(24);
            entity.HasIndex(attempt => new { attempt.LearnerId, attempt.StartedAtUtc });
            entity.HasOne<User>().WithMany()
                .HasForeignKey(attempt => attempt.LearnerId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GeometryExample>().WithMany()
                .HasForeignKey(attempt => attempt.ExampleId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AttemptStageResult>(entity =>
        {
            entity.ToTable("attempt_stage_results");
            entity.HasKey(result => result.Id);
            entity.Property(result => result.Stage).HasConversion<string>().HasMaxLength(32);
            entity.Property(result => result.Outcome).HasConversion<string>().HasMaxLength(24);
            entity.Property(result => result.ResponseJson).HasColumnType("jsonb");
            entity.HasIndex(result => new { result.AttemptId, result.Stage }).IsUnique();
            entity.HasOne<LearningAttempt>().WithMany(attempt => attempt.StageResults)
                .HasForeignKey(result => result.AttemptId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}

/// <summary>Defines stable identifiers for content included in the initial database seed.</summary>
internal static class SeedData
{
    /// <summary>The seeded circle geometry topic identifier.</summary>
    internal static readonly Guid CircleGeometryTopicId = Guid.Parse("ef08a50d-dc56-47ab-9c99-2104ce69c001");

    /// <summary>The seeded same-segment example identifier.</summary>
    internal static readonly Guid SameSegmentExampleId = Guid.Parse("ef08a50d-dc56-47ab-9c99-2104ce69c002");

    /// <summary>The seeded same-segment theorem identifier.</summary>
    internal static readonly Guid SameSegmentTheoremId = Guid.Parse("ef08a50d-dc56-47ab-9c99-2104ce69c003");

    /// <summary>The seeded chord definition identifier.</summary>
    internal static readonly Guid ChordDefinitionId = Guid.Parse("ef08a50d-dc56-47ab-9c99-2104ce69c004");
}
