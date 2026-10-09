# Feature Specification

- Date: 2026-10-09
- Feature: Sprint 3 Core Domain Model and API Foundation
- Related Plan: `docs/planning/2026-10-09-core-domain-api-sprint3-plan.md`
- Status: Complete

## Summary

Sprint 3 defines the first durable GeoPath domain model and backend APIs. PostgreSQL stores account records and roles, geometry content, classroom relationships, assignments, and learner learning history. JWT access tokens remain signed API credentials and are not persisted as user data. The Angular client is deferred until the backend and API contracts are stable.

The initial deliverable is a deliberately narrow foundation, not the complete product data model. It will support content browsing and the capture/retrieval of learner attempts, while leaving advanced proof evaluation, mastery scoring, analytics, and authoring workflows for later work.

## User Stories

- As a learner, I want my account and learning attempts saved so they remain available after the API restarts.
- As a learner, I want to browse examples, theorems, and definitions to find relevant practice.
- As a learner, I want the system to remember outcomes for each of the four thinking stages.
- As a teacher, I want to create classes and assignments that can later be associated with learner work.
- As a visitor, I want to browse public geometry content without seeing private learner data.
- As a backend developer, I want stable data and API contracts before the Angular client is integrated.

## Functional Requirements

1. PostgreSQL must persist users, role assignments, classes, class enrollments, geometry content, assignments, attempts, and attempt-stage outcomes.
2. User credentials must store password hashes only.
3. Access JWTs must not be stored as account data.
4. The system must expose public read endpoints for published examples, theorems, and definitions.
5. Authenticated learners must be able to create and retrieve their own attempts.
6. Learner attempt APIs must derive ownership from the authenticated principal; a caller must not choose another learner's user ID.
7. Teachers must own the classes they create; enrollments link learner users to classes.
8. Assignments must be associated with a class and a geometry example.
9. Persist stage results for Abstraction, Decomposition, Pattern recognition, and Solving.
10. Do not report a mastery score until a product-approved formula exists.
11. Registration must not allow a caller to self-assign Teacher, Author, or Admin roles.
12. Angular implementation remains out of scope until backend contracts are stable.

## Acceptance Criteria

- AC1: A database migration creates the approved schema on PostgreSQL.
- AC2: User registration, login, and current-user retrieval operate against PostgreSQL-backed accounts.
- AC3: A process restart does not erase registered user accounts.
- AC4: Public content endpoints return published sample examples, theorems, and definitions.
- AC5: An authenticated learner can create and retrieve their own attempt and stage outcomes.
- AC6: A learner cannot retrieve another learner's attempts by changing a request identifier.
- AC7: Role and ownership constraints are enforced by API logic and relational constraints where appropriate.
- AC8: Seed data can be applied repeatably without duplicating records.
- AC9: The server builds and targeted integration tests validate persistence and authorization behavior.
- AC10: No Angular feature is added as part of Sprint 3.

## Technical Decisions

### Architecture

- Domain: core entities and role concepts.
- Application: request/response models, validation, use cases, and repository interfaces.
- Infrastructure: EF Core DbContext, PostgreSQL provider, migrations, repositories, and initial seed data.
- API: authenticated and public HTTP contracts, authorization, and dependency wiring.

### Persistence

Use EF Core with Npgsql to map the relational model and manage versioned schema migrations. PostgreSQL is the source of truth; in-memory storage is not used by the deployed API. A database server must be provisioned separately and configured through application settings or environment variables.

### JWT handling

JWTs are bearer credentials presented to the API. PostgreSQL stores account identity and authorization data, not the serialized access token. Refresh tokens and revocation/session tables remain out of scope for this sprint.

## Backend Design

### Initial entity groups

#### Identity and access
- `User`: ID, display name, normalized unique email, password hash, and timestamps.
- `Role`: supported role name and stable identifier.
- `User.Role`: foreign key to the supported `Role` record. Sprint 3 retains one active role per account, consistent with the existing authentication model.

The default roles are Visitor, Learner, Teacher, Author, and Admin. Visitor is an unauthenticated access state and does not need a registered user row.

#### Classroom and assignment
- `Class`: ID, name, unique hashed join code, owner teacher, and timestamps.
- `ClassEnrollment`: class, learner, enrollment state, and timestamps.
- `Assignment`: class, example, due date, creator, and timestamps.

#### Geometry content
- `Topic`: curriculum grouping for related content.
- `Example`: published learning activity with title, prompt, difficulty, topic, and structured content needed by the guided flow.
- `Theorem`: name, statement, explanation, and publication status.
- `Definition`: name, description, and publication status.
- `ExampleTheorem`: many-to-many link between examples and theorems.
- `ExampleDefinition`: many-to-many link between examples and definitions.

Represent diagram and proof-flow payloads as versioned JSONB only where the structure is inherently flexible; retain identity, status, searchable fields, and relationships in typed relational columns/tables. The exact proof-step schema is deferred until the required learner flow is agreed.

#### Learning history
- `Attempt`: learner, example, start/completion timestamps, and overall outcome.
- `AttemptStageResult`: attempt, one of the four thinking stages, outcome, and optional response/feedback metadata.

Attempt history is the durable source for future progress and mastery calculations. Do not store an unqualified `mastery_percent` in this sprint.

### Role assignment rules

Registration creates a Learner account by default. Teacher, Author, and Admin assignment must be performed by a privileged administrative process, not accepted from public registration input. This closes the self-assigned privilege path in the Sprint 2 API.

## Database Design

### Relationships and constraints

- Normalized email is unique.
- `User.Role` references a seeded `Role` record, preventing unsupported role values.
- `Class` references its owning teacher user.
- `ClassEnrollment` references one class and one learner; one learner can enroll in a class at most once.
- `Assignment` references one class, its creating teacher, and one example.
- Content relationship tables prevent duplicate example-to-theorem and example-to-definition links.
- `Attempt` references one learner and one example.
- `AttemptStageResult` references one attempt and has no more than one result per defined stage.
- Foreign keys should use restricted deletion for records needed to preserve learning history; content may use soft publication/archival state.

### Data lifecycle

- PostgreSQL stores durable accounts, roles, content, classroom data, assignments, and attempts.
- Password hashes are persisted; plaintext passwords are not.
- JWT access tokens are not persisted.
- Content remains separate from class activity and learning history.
- Mastery is derived from attempts until the calculation is defined.

## Frontend Design

No Angular implementation is included in Sprint 3. API contracts should be documented and stable enough for a later Angular auth/content/progress integration sprint.

## API Design

Implemented endpoints:

During Development, the built-in OpenAPI JSON document is served at `/openapi/v1.json` and Swagger UI is served at `/swagger`. Swagger UI is disabled in other environments.

### Public content
- `GET /api/examples`
- `GET /api/examples/{id}`
- `GET /api/theorems`
- `GET /api/definitions`

### Learner attempts
- `POST /api/attempts` - create an attempt for the authenticated learner and example.
- `GET /api/attempts` - list only the authenticated learner's attempts.
- `GET /api/attempts/{id}` - retrieve an owned attempt with its stage outcomes.
- `PUT /api/attempts/{id}/stages/{stage}` - record/update the learner's outcome for a stage in an owned attempt.

### Account integration
- Existing auth endpoints continue to register, login, and return current-user details using PostgreSQL-backed accounts.
- Public registration assigns Learner role by default; the request cannot grant privileged roles.

### Deferred APIs

Teacher analytics, class dashboards, full assignment workflows, theorem/definition detail editing, and authoring APIs are not part of the initial Sprint 3 implementation unless the scope is expanded.

## Security and Access

- Public content contains no private learner data.
- Attempt endpoints require authentication and always scope results to the authenticated user.
- Teacher/class authorization verifies class ownership or membership rather than trusting IDs in the request.
- Public registration cannot assign privileged roles.
- Passwords are hashed before persistence.
- Database connection strings and JWT signing secrets must be configured outside source control.
- All database access uses parameterized EF Core queries.

## Testing Strategy

- Built the full .NET solution with zero warnings or errors.
- Applied the initial migration to a disposable PostgreSQL 17 database and confirmed schema tables and seed rows.
- Smoke-tested registration, login, current-user identity, content retrieval, attempt creation/listing, and stage-result persistence over HTTP.
- Verified public registration cannot assign Teacher, role-restricted endpoints deny Learner access, a learner cannot retrieve another learner's attempt, and invalid JSON/stage input is rejected.
- No automated test project existed; the API/database validation was performed as an isolated smoke test.

## Risks / Open Questions

- The exact proof-step payload, content versioning policy, and mastery formula remain future decisions.
- Class and assignment management endpoints and class join-code lifecycle remain future work; join codes are modeled as hashes.
