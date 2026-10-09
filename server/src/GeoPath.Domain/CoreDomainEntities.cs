namespace GeoPath.Domain;

/// <summary>Groups geometry learning content into curriculum topics.</summary>
public sealed class GeometryTopic
{
    /// <summary>Gets or sets the topic's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the topic's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets an optional topic description.</summary>
    public string? Description { get; set; }
}

/// <summary>Describes a geometry exercise and the structured material used to present it.</summary>
public sealed class GeometryExample
{
    /// <summary>Gets or sets the example's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the topic containing the example.</summary>
    public Guid TopicId { get; set; }

    /// <summary>Gets or sets the example's title.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the problem prompt shown to the learner.</summary>
    public string Prompt { get; set; } = string.Empty;

    /// <summary>Gets or sets the difficulty label used to filter examples.</summary>
    public ExampleDifficulty Difficulty { get; set; } = ExampleDifficulty.Medium;

    /// <summary>Gets or sets optional versioned JSON for diagram and guided-flow data.</summary>
    public string? GuidedFlowJson { get; set; }

    /// <summary>Gets or sets whether the example is visible to public content queries.</summary>
    public bool IsPublished { get; set; }

    /// <summary>Gets or sets the UTC time the example was created.</summary>
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Gets or sets the example's curriculum topic navigation.</summary>
    public GeometryTopic? Topic { get; set; }

    /// <summary>Gets the theorem relationships associated with the example.</summary>
    public ICollection<ExampleTheorem> Theorems { get; set; } = new List<ExampleTheorem>();

    /// <summary>Gets the definition relationships associated with the example.</summary>
    public ICollection<ExampleDefinition> Definitions { get; set; } = new List<ExampleDefinition>();
}

/// <summary>Stores a geometry theorem that can be referenced by learning content.</summary>
public sealed class Theorem
{
    /// <summary>Gets or sets the theorem's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the theorem's short display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the formal theorem statement.</summary>
    public string Statement { get; set; } = string.Empty;

    /// <summary>Gets or sets an optional learner-facing explanation.</summary>
    public string? Explanation { get; set; }

    /// <summary>Gets or sets whether the theorem is visible to public content queries.</summary>
    public bool IsPublished { get; set; }
}

/// <summary>Stores a geometry definition that can be referenced by learning content.</summary>
public sealed class GeometryDefinition
{
    /// <summary>Gets or sets the definition's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the definition's short display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the definition's explanatory text.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets whether the definition is visible to public content queries.</summary>
    public bool IsPublished { get; set; }
}

/// <summary>Links an example to a theorem used in that example.</summary>
public sealed class ExampleTheorem
{
    /// <summary>Gets or sets the linked example identifier.</summary>
    public Guid ExampleId { get; set; }

    /// <summary>Gets or sets the linked theorem identifier.</summary>
    public Guid TheoremId { get; set; }

    /// <summary>Gets or sets the related example.</summary>
    public GeometryExample? Example { get; set; }

    /// <summary>Gets or sets the related theorem.</summary>
    public Theorem? Theorem { get; set; }
}

/// <summary>Links an example to a definition used in that example.</summary>
public sealed class ExampleDefinition
{
    /// <summary>Gets or sets the linked example identifier.</summary>
    public Guid ExampleId { get; set; }

    /// <summary>Gets or sets the linked definition identifier.</summary>
    public Guid DefinitionId { get; set; }

    /// <summary>Gets or sets the related example.</summary>
    public GeometryExample? Example { get; set; }

    /// <summary>Gets or sets the related definition.</summary>
    public GeometryDefinition? Definition { get; set; }
}

/// <summary>Represents a teacher-owned class in which learners can enroll.</summary>
public sealed class Classroom
{
    /// <summary>Gets or sets the class's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the teacher who owns the class.</summary>
    public Guid TeacherId { get; set; }

    /// <summary>Gets or sets the class's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the unique, one-way hash of the class join code.</summary>
    public string JoinCodeHash { get; set; } = string.Empty;

    /// <summary>Gets or sets when the class was created in UTC.</summary>
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>Associates a learner with a class and records their enrollment state.</summary>
public sealed class ClassEnrollment
{
    /// <summary>Gets or sets the enrollment's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the class the learner joined.</summary>
    public Guid ClassroomId { get; set; }

    /// <summary>Gets or sets the enrolled learner's user identifier.</summary>
    public Guid LearnerId { get; set; }

    /// <summary>Gets or sets the current enrollment status.</summary>
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    /// <summary>Gets or sets when the learner joined the class in UTC.</summary>
    public DateTimeOffset EnrolledAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>Assigns a geometry example to a teacher-owned class.</summary>
public sealed class LearningAssignment
{
    /// <summary>Gets or sets the assignment's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the class receiving the assignment.</summary>
    public Guid ClassroomId { get; set; }

    /// <summary>Gets or sets the example learners are assigned.</summary>
    public Guid ExampleId { get; set; }

    /// <summary>Gets or sets the teacher who created the assignment.</summary>
    public Guid CreatedByTeacherId { get; set; }

    /// <summary>Gets or sets the optional due date in UTC.</summary>
    public DateTimeOffset? DueAtUtc { get; set; }

    /// <summary>Gets or sets when the assignment was created in UTC.</summary>
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>Records one learner's work session for a geometry example.</summary>
public sealed class LearningAttempt
{
    /// <summary>Gets or sets the attempt's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the learner who performed the attempt.</summary>
    public Guid LearnerId { get; set; }

    /// <summary>Gets or sets the example worked on.</summary>
    public Guid ExampleId { get; set; }

    /// <summary>Gets or sets the attempt's current state.</summary>
    public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

    /// <summary>Gets or sets when the attempt started in UTC.</summary>
    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Gets or sets when the attempt was completed in UTC, if complete.</summary>
    public DateTimeOffset? CompletedAtUtc { get; set; }

    /// <summary>Gets the persisted outcomes for the attempt's thinking stages.</summary>
    public ICollection<AttemptStageResult> StageResults { get; set; } = new List<AttemptStageResult>();
}

/// <summary>Stores a learner's outcome and optional response for one attempt stage.</summary>
public sealed class AttemptStageResult
{
    /// <summary>Gets or sets the stage-result's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the attempt this result belongs to.</summary>
    public Guid AttemptId { get; set; }

    /// <summary>Gets or sets which thinking stage this result describes.</summary>
    public AttemptStage Stage { get; set; }

    /// <summary>Gets or sets the learner's outcome for the stage.</summary>
    public StageOutcome Outcome { get; set; }

    /// <summary>Gets or sets optional versioned JSON for the learner response.</summary>
    public string? ResponseJson { get; set; }

    /// <summary>Gets or sets when the stage outcome was recorded in UTC.</summary>
    public DateTimeOffset RecordedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>Defines the difficulty labels used for geometry examples.</summary>
public enum ExampleDifficulty
{
    /// <summary>Introductory practice difficulty.</summary>
    Easy,

    /// <summary>Standard practice difficulty.</summary>
    Medium,

    /// <summary>Advanced practice difficulty.</summary>
    Hard,
}

/// <summary>Defines the four thinking stages used in the guided learner flow.</summary>
public enum AttemptStage
{
    /// <summary>Identify the useful information and goal.</summary>
    Abstraction,

    /// <summary>Break the problem into smaller reasoning parts.</summary>
    Decomposition,

    /// <summary>Recognize relevant patterns and geometry facts.</summary>
    PatternRecognition,

    /// <summary>Complete the solution using the established reasoning.</summary>
    Solving,
}

/// <summary>Defines a learner's result for a thinking stage.</summary>
public enum StageOutcome
{
    /// <summary>The learner's response is incorrect.</summary>
    Incorrect,

    /// <summary>The learner's response is partially correct.</summary>
    Partial,

    /// <summary>The learner's response is correct.</summary>
    Correct,

    /// <summary>The learner skipped the stage.</summary>
    Skipped,
}

/// <summary>Defines whether a learning attempt is active or completed.</summary>
public enum AttemptStatus
{
    /// <summary>The learner has started but not completed the attempt.</summary>
    InProgress,

    /// <summary>The learner has completed the attempt.</summary>
    Completed,
}

/// <summary>Defines whether a learner remains enrolled in a class.</summary>
public enum EnrollmentStatus
{
    /// <summary>The learner is currently enrolled.</summary>
    Active,

    /// <summary>The enrollment has been ended.</summary>
    Withdrawn,
}
