# Feature Plan

- Date: 2026-10-09
- Feature: Sprint 2 Authentication and Role Foundation
- Purpose: Establish the identity, authorization, and role model needed to protect GeoPath user journeys and prepare the platform for learner, teacher, author, and admin flows.
- Status: In Progress

## Problem / Context

GeoPath has a strong concept and UI prototype, but the system currently has no real user identity model, authentication flow, or role enforcement. Without a secure and consistent authentication foundation, protected learning journeys cannot be trusted, and future teacher, learner, and author features would be built on unstable assumptions.

## Product References

- `ProjectStructure/EucliGeo wireframes.html`
- `ProjectStructure/EucliGeo clickable prototype.html`
- `docs/planning/2026-10-08-phase1-sprint-plan.md`
- `docs/specs/2026-10-08-foundation-mvp-spec.md`

## Target Users

- Learners who need to sign up, access learning content, and keep progress protected
- Teachers who need controlled access to class and analytics screens
- Authors who need access to content creation and review tools
- Admins who need platform-wide oversight and safe authorization boundaries
- Visitors who can browse public content without signing in

## Goals

- define the user and role model for GeoPath
- add secure registration and login flows
- issue and validate access tokens for authenticated users
- enforce role-based access rules for protected resources
- establish the backend foundation for future learner onboarding and class features

## Scope

### In scope
- user entity, role enum, and default role definitions
- registration and login endpoints
- password hashing and secure token generation
- role-based authorization checks in the API
- protected route and user identity examples
- backend documentation and configuration needed for auth flows

### Out of scope
- full PostgreSQL migration and production infrastructure setup
- front-end page styling beyond a thin backend/auth API contract
- classroom enrollment logic and teacher dashboards
- authoring workflows and advanced analytics

## Requirements
- The API must support registering a user with a role.
- The API must support signing in with email and password.
- User passwords must never be stored in plain text.
- Authenticated users must be identifiable via claims and token payloads.
- Protected endpoints must allow or deny access based on role.
- Admin, teacher, learner, and author roles should be represented explicitly.
- The design should be compatible with future PostgreSQL-backed persistence.

## Proposed Approach

Implement the Sprint 2 foundation in the .NET backend first: add domain models for users and roles, an application-layer authentication service, and an in-memory repository to support local development and validation. Add secure JWT-based authentication and role requirement checks in the API project. Capture the decisions in a sprint specification so the implementation is reviewable and reusable for later database integration.

## Risks / Dependencies

- auth design must be clear before other user and learning features are built
- role semantics need to remain consistent across future endpoints
- password and token handling must avoid accidental insecure defaults
- future database integration will need to replace the in-memory implementation without changing the API contract

## Open Questions

- Should the project use JWT only or a hybrid token strategy for future refresh flows?
- Will the final PostgreSQL design store user roles as an enum value or normalized reference table?
- Should the Angular front-end be added in this sprint or deferred until the backend contract is stable?
