# Sprint 1 Decision Record

- Date: 2026-10-08
- Sprint: Sprint 1 - Foundation and Architecture
- Status: Approved for this phase

## Executive summary

For Sprint 1, the team decided to create a clean separation between the server and client layers, keep the system architecture intentionally simple, and establish a foundational .NET + Angular + PostgreSQL structure without jumping into advanced business logic or full learner features.

This decision was made because the project is still in the architecture and validation stage. The highest value action was to build a stable base that can support the learner MVP without forcing premature complexity.

## Decisions taken

### 1. Separate the repository into `server` and `client`

#### Decision

The project is now organized as:

- `/server` for backend code and solution files
- `/client` for the Angular front-end application

#### Why this was chosen

This keeps the responsibilities clear and aligns with the intended architecture:

- backend logic belongs with the .NET solution and server-side concerns
- front-end behavior belongs with Angular and browser-based concerns
- future team work is easier to review because each layer has a single home

#### Alternative considered

A single mixed project structure with frontend and backend code in the same top-level area.

#### Why the alternative was rejected

A single mixed structure makes the repository harder to reason about, especially as the team scales. It also creates confusion around build ownership, deployment, and service responsibilities. The client/server split is clearer and more maintainable.

### 2. Use a layered .NET backend foundation

#### Decision

We used a simplified layered structure:

- Domain project for core models
- Application project for services and orchestration logic
- Infrastructure project for persistence and implementation concerns
- API project as the entry point for HTTP endpoints

#### Why this was chosen

The business logic and persistence boundaries need to be explicit early. This makes the system easier to evolve as the project grows from foundation to MVP and beyond. It also fits the planned architecture for the project.

#### Alternative considered

A single ASP.NET project containing everything in one folder and one codebase.

#### Why the alternative was rejected

This would work in the short term, but it creates poor separation of concerns. It makes future scaling harder, especially when adding auth, content logic, analytics, and classroom management. We wanted the core structure to support future complexity without rework.

### 3. Use a simple API health and architecture endpoint for the initial backend status

#### Decision

The API exposes basic endpoints for platform health and architecture status instead of jumping into full business features during Sprint 1.

#### Why this was chosen

The first sprint is about establishing the foundation, not implementing learner features. A small status endpoint confirms that:

- the API is running
- the backend is connected to the expected architecture
- the team can validate that the server is operational before adding more complex functionality

#### Alternative considered

Implementing authentication, learner onboarding, or class APIs immediately.

#### Why the alternative was rejected

These are important, but they belong to Sprint 2 and later phases. Sprint 1 should prove the plumbing works first. Doing too much too early increases risk and makes debugging harder.

### 4. Use Angular as the front-end shell with a simple foundation page

#### Decision

The Angular app was initialized and styled to show the project foundation, technology stack, and a basic status display connected to the backend.

#### Why this was chosen

This gives the team a visible front-end application at the start of the project. It also allows quick validation that the client and API can communicate and that the user-facing app shell matches the product direction.

#### Alternative considered

A backend-only implementation or a static HTML prototype only.

#### Why the alternative was rejected

The project architecture explicitly requires an Angular client. A backend-only setup would not validate the front-end contract or product direction. The current UI provides a lightweight but meaningful front-end starting point.

### 5. Keep the first sprint intentionally narrow

#### Decision

Sprint 1 delivered the project structure, core backend setup, initial Angular app, and validation of both client and server communication. It did not include deeper auth, teacher, or learner features.

#### Why this was chosen

This is the correct scope for a foundation phase. Architecture should be proven before product feature work expands.

#### Alternative considered

Move directly into learner authentication and onboarding flows in the same sprint.

#### Why the alternative was rejected

That would create too much risk in the same iteration. We would be mixing architecture validation with feature delivery. The project guidance explicitly requires planning and specification before feature work, and the first sprint is meant to establish the base for the rest.

## Comparison with other alternatives

### Alternative: monolithic single-project repo

Pros:

- faster to set up initially
- easier for small teams to start with minimal structure

Cons:

- harder to reason about as the project grows
- client/server boundaries become blurred
- deployment and testing are harder to manage
- more friction when adding more feature areas

Conclusion:

For a product with Angular + .NET + PostgreSQL, a split client/server structure is the better long-term decision.

### Alternative: immediate implementation of auth and learner flows

Pros:

- faster visible product progress
- more feature-rich early result

Cons:

- weak architecture foundation
- difficult debugging and integration issues
- more rework later
- violates the principle of establishing base technical infrastructure first

Conclusion:

The first sprint should not take on complex domain features. The architecture needed to support them must come first.

### Alternative: full DDD/folder explosion from day one

Pros:

- highly structured and explicit
- good for large teams and mature systems

Cons:

- adds overhead before the business requirements are fully known
- slower for an early-stage project
- increases complexity without immediate value

Conclusion:

A moderate layered structure is more appropriate for this phase. We can deepen the architecture later if required by complexity.

## Outcome

The decisions made in Sprint 1 produce a project foundation that is:

- clear and maintainable
- aligned to the product roadmap
- ready for the next phase of work without large-scale reorganization
- suitable for incremental feature delivery in the future

## Final note

Sprint 1 is intentionally a platform foundation sprint, not a full product sprint. The choices made here prioritize maintainability, clarity, and risk reduction over early feature breadth.
