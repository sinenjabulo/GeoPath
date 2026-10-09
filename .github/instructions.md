# GeoPath Development Instructions

This project is a geometry learning platform for South African high school learners. The product direction, user journeys, and system behavior are defined by the design artifacts in the `ProjectStructure` folder. These files are the reference for what the product should do and how the experience should be structured.

Before any feature is built, the team must complete a planning step and an implementation specification step. No feature work may begin without both documents being created and reviewed.

## 1. Source of truth for product direction

The following files define the product scope and expected functionality:

- [ProjectStructure/EucliGeo and GeoPath combined design.html](ProjectStructure/EucliGeo%20and%20GeoPath%20combined%20design.html)
- [ProjectStructure/EucliGeo wireframes.html](ProjectStructure/EucliGeo%20wireframes.html)
- [ProjectStructure/EucliGeo clickable prototype.html](ProjectStructure/EucliGeo%20clickable%20prototype.html)

These files must be used to guide:

- who the application is for
- how learners, teachers, authors, and visitors interact with the product
- which pages and flows are required
- which learning features must exist
- what the system should measure and track
- what the platform needs to support in terms of analytics, assignments, and mastery

Everything implemented in this project must align with the behavior represented in these design files.

## 1.1 Code documentation requirements

Every new or modified source file must document its code elements with concise, useful comments:

- In C#, add XML documentation to classes, records, interfaces, enums, constructors, and methods. Describe parameters, return values, and exceptions where relevant. Document public properties and enum values.
- In Angular and other client-side TypeScript, add concise JSDoc comments to classes, components, services, interfaces, enums, and methods/functions. Describe parameters and return values where relevant, and document public properties that form part of the component or service contract.
- For top-level setup code and route declarations, add short comments that explain the purpose of each meaningful configuration or endpoint group.
- Comments must describe behavior and intent rather than restate the code. Keep them short, accurate, and updated when behavior changes; avoid redundant comments on obvious local statements.

Apply this requirement to new code in all layers, including backend and client-side code.

## 2. Mandatory process before feature development

Every feature, enhancement, or change must go through the following process:

1. Create a planning document
2. Create a technical specification document
3. Review the plan and spec
4. Only then begin implementation

If a planning file or specification file is missing, the feature is not ready to be developed.

## 3. Planning files

Before development starts, create a planning file with the following information:

- date the plan was created
- feature or task name
- purpose of the plan
- problem or opportunity being addressed
- project context and reason for the work
- users affected by the feature
- references to the relevant project structure files
- scope of the work
- in-scope and out-of-scope items
- assumptions and dependencies
- goals and expected outcome
- risks, blockers, or open questions
- high-level implementation approach

### Planning file naming convention

Use the format:

`YYYY-MM-DD - Feature Name - Plan.md`

Example:

`2026-10-08 - learner-progress-plan.md`

### Planning file location

Place planning files in:

`docs/planning/`

### Planning file template

```md
# Feature Plan

- Date: YYYY-MM-DD
- Feature: Feature Name
- Purpose: Why this feature is needed
- Status: Draft / Approved / In Progress / Complete

## Problem / Context
Describe the business problem or opportunity.

## Product References
Reference the relevant prototype or design files from `ProjectStructure`.

## Target Users
Who this feature affects.

## Goals
What outcome the feature should deliver.

## Scope
### In scope
- ...

### Out of scope
- ...

## Requirements
- Requirement 1
- Requirement 2

## Proposed Approach
Describe the high-level plan.

## Risks / Dependencies
List assumptions, dependencies, and blockers.

## Open Questions
Document anything still undecided.
```

## 4. Specification files

After the planning file is approved, create a feature specification document. This file must explain how the team will implement the plan and must record the technical decisions that are being made.

The specification must be detailed enough that another developer can understand, implement, and validate the feature without needing additional assumptions.

### Specification file contents

Each specification file must include:

- feature name
- date
- related planning document
- summary of the plan
- user stories
- functional requirements
- non-functional requirements
- system behavior and expected flow
- architecture decisions
- backend design decisions
- database design decisions
- front-end design decisions
- API contracts and endpoints
- validation and error handling rules
- security and access rules
- testing strategy
- rollout or deployment notes
- outstanding questions and future considerations

### Specification file naming convention

Use the format:

`YYYY-MM-DD - Feature Name - Spec.md`

Example:

`2026-10-08 - learner-progress-spec.md`

### Specification file location

Place specification files in:

`docs/specs/`

### Specification file template

```md
# Feature Specification

- Date: YYYY-MM-DD
- Feature: Feature Name
- Related Plan: `docs/planning/...`
- Status: Draft / Approved / In Progress / Complete

## Summary
Describe what the feature is and why it exists.

## User Stories
- As a learner, I want ...
- As a teacher, I want ...
- As an author, I want ...

## Functional Requirements
- Requirement 1
- Requirement 2

## Acceptance Criteria
- AC1: ...
- AC2: ...

## Technical Decisions
Explain the chosen architecture and implementation direction.

## Backend Design
Describe the .NET design, services, repositories, DTOs, controllers, and business logic.

## Database Design
Describe PostgreSQL tables, relationships, constraints, indexes, and data ownership.

## Frontend Design
Describe Angular components, pages, services, routes, and client-side state management.

## API Design
List endpoints, request models, response models, and validation.

## Security and Access
Document authorization rules, roles, and restrictions.

## Testing Strategy
Describe unit, integration, and end-to-end validation expectations.

## Risks / Open Questions
Document unresolved issues.
```

## 5. Required workflow for every feature

The normal workflow is:

1. Review the design files in `ProjectStructure`
2. Identify the feature or problem to solve
3. Create a planning document with date, purpose, scope, and goals
4. Review and approve the plan
5. Create a specification document with implementation decisions
6. Build only after both docs are in place
7. Keep the spec updated if requirements change during implementation

## 6. Product alignment expectations

The product implementation must remain consistent with the intended system vision. The planned architecture is:

- Front-end: Angular
- Back-end: C# and .NET
- Database: PostgreSQL
- Domain: learner geometry practice, theorem guidance, proof building, mastery tracking, and class analytics

The final implementation should preserve the core experience represented in the project structure:

- visitor access and trials
- learner sign-up and class join flow
- example browsing and learning pages
- theorem and definition views
- guided reasoning process with four thinking stages
- proof builder and validation flow
- learner progress and mastery tracking
- teacher analytics and reporting
- assignment and class management
- authoring and content review workflow

## 7. Decision tracking and documentation discipline

All major implementation decisions must be recorded in the relevant specification document.

This includes:

- architecture choices
- system boundaries
- user flow decisions
- data model decisions
- API decisions
- front-end/component decisions
- role and access control decisions
- performance decisions
- security decisions
- database schema decisions
- future considerations

No important decision should be left implicit or undocumented.

## 8. Mandatory rule

Before any development work begins, the following must exist:

- a planning file with the date, purpose, and implementation plan
- a specification file with technical decisions and implementation details

If either document is missing, the feature is not ready to begin.

## 9. Summary

This project must be developed with disciplined planning and specification-first execution. The design files in `ProjectStructure` explain what the product should do and how the experience should feel. Every future feature must reflect that direction, be documented in a planning file, and be implemented according to a detailed spec that captures the decisions behind it.

The expected architecture for the project is:

- Angular frontend
- C# / .NET backend
- PostgreSQL database

This ensures the implementation stays aligned with the product vision and remains maintainable, testable, and scalable.

## Summary

This project needs clear evidence-driven planning before coding begins. The design files in `ProjectStructure` define the product vision, user workflows, and expected behavior. All future feature work must be grounded in those files, documented with planning and specification artifacts, and kept aligned with the intended architecture: Angular front-end, .NET backend, and PostgreSQL database.
