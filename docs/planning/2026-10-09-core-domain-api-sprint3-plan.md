# Feature Plan

- Date: 2026-10-09
- Feature: Sprint 3 Core Domain Model and API Foundation
- Purpose: Define and implement the first durable PostgreSQL-backed domain model and API surfaces for GeoPath's content and learner learning history.
- Status: Complete

## Problem / Context

Sprint 2 established authentication and roles, but user accounts currently live only in process memory. GeoPath also has no persisted geometry content, class, assignment, attempt, or progress model. Sprint 3 should decide what the system stores and establish durable backend contracts before the Angular client is integrated.

## Product References

- `ProjectStructure/EucliGeo wireframes.html`
- `ProjectStructure/EucliGeo clickable prototype.html`
- `docs/planning/2026-10-08-phase1-sprint-plan.md`
- `docs/specs/2026-10-08-foundation-mvp-spec.md`
- `docs/specs/2026-10-09-authentication-roles-sprint2-spec.md`

## Target Users

- Learners, whose accounts, class membership, attempts, and progress must persist
- Teachers, who need classes and assignments associated with learner work
- Authors, who need structured geometry content
- Visitors, who can read public content without an account
- Backend and client developers, who need stable API and data contracts

## Goals

- agree on the first PostgreSQL data model and clarify what data is stored
- persist user accounts and roles instead of relying on process memory
- represent classes, enrollments, geometry topics, examples, theorems, definitions, and assignments
- record learner attempts and stage outcomes so progress can be retrieved
- expose read APIs for public geometry content and authenticated learner data
- provide a local Swagger UI for browsing the API contract
- keep mastery as a derived measure until its calculation is agreed

## Scope

### In scope
- PostgreSQL schema and versioned EF Core migrations
- domain entities and relationships for accounts, classes, content, assignments, attempts, and stage progress
- repository and service patterns for the initial content and learner APIs
- seeded roles and representative sample geometry content
- public list/detail endpoints for examples, theorems, and definitions
- authenticated learner endpoints for their own attempts and progress
- migrate Sprint 2 user persistence from in-memory storage to PostgreSQL

### Out of scope
- Angular auth module, login pages, route guards, or API integration
- advanced proof evaluation or automated theorem validation
- a final mastery formula or predictive analytics
- complete teacher dashboards, class heatmaps, exports, or live quizzes
- authoring UI, content review workflow, and full content-management APIs
- refresh tokens, server-side JWT sessions, or storing access JWTs in PostgreSQL

## Requirements

- PostgreSQL is the durable source of truth for accounts and core domain data.
- Store password hashes, never plaintext passwords.
- JWT access tokens are not stored in PostgreSQL; their identity claims are validated by the API.
- Persist geometry content separately from learner attempts and class activity.
- A class must be associated with its teacher, and learner membership must be represented explicitly.
- Learners may access only their own attempt and progress records.
- Public content endpoints must not disclose private learner or class data.
- New backend classes and methods must follow the repository's code documentation requirements.
- The Angular client remains deferred until the backend and API contracts are stable.

## Proposed Approach

Use EF Core with the Npgsql provider and database migrations. Keep entities and application contracts in their existing layers, and implement PostgreSQL repositories in Infrastructure. The initial model separates accounts, roles, classes, class enrollments, geometry content, assignments, learner attempts, and per-stage attempt outcomes. Track raw learning events and derive mastery from them later, rather than persisting a score with an undefined calculation. Public registration assigns Learner; privileged roles are assigned outside that endpoint.

The approved model has been implemented with a versioned initial migration, seed data, PostgreSQL repositories, and public content/authenticated learner-attempt APIs. PostgreSQL itself is supplied through configured connection settings; Sprint 3 does not install or provision a database server.

## Risks / Dependencies

- The precise proof-step content format and mastery calculation remain unresolved.
- Public registration must prevent self-assignment of privileged roles while preserving role assignments in PostgreSQL.
- PostgreSQL connection configuration and a reachable development database are required to run migrations and integration checks.
- Sample content must align with the prototype without treating prototype-only state as a final curriculum.

## Open Questions

- The exact proof-step JSON format and content versioning policy need product decisions before richer examples are authored.
- Mastery calculation remains deferred until its rules are agreed.
- Class and assignment management endpoints are deferred to a later API sprint.
