# Feature Specification

- Date: 2026-10-08
- Feature: GeoPath Foundation and MVP Implementation Specification
- Related Plan: `docs/planning/2026-10-08-product-roadmap-plan.md`
- Status: Draft

## Summary

This specification defines the first implementation stage for GeoPath. It covers the foundational architecture and the minimum viable learner-focused product flow required to validate the platform concept before building more advanced teacher and author functions.

The initial release will be built using the project’s chosen technology stack:

- Angular for the front-end
- C# and .NET for the backend API
- PostgreSQL for the database

The goal is to establish a clean architecture that supports the learner experience, role-based access, and future growth without locking the team into a weak foundation.

## User Stories

- As a visitor, I want to preview the educational product so I understand what it offers.
- As a learner, I want to create an account and join a class so I can access learning content.
- As a learner, I want to browse geometry examples so I can practice required topics.
- As a learner, I want guided stages and proof support so I can solve problems in a structured way.
- As a learner, I want my progress saved so I can continue and see mastery improvements.
- As a teacher, I want to see how learners are performing so I can identify weaknesses and assign work.
- As an author, I want a way to create and maintain geometry examples and theorem content so the platform remains accurate and useful.

## Functional Requirements

1. The system must support multiple user roles: Visitor, Learner, Teacher, Author, and Administrator.
2. A learner must be able to create an account and join a class.
3. A learner must be able to browse examples and definitions.
4. A learner must be able to work through a geometry problem using a guided flow.
5. A learner must be able to enter a proof and receive validation feedback.
6. The system must track progress, attempts, and mastery for each learner.
7. A teacher must be able to view class progress and identify weaker learners.
8. A teacher must be able to assign learning tasks or examples.
9. An author must be able to create and edit educational content for examples, definitions, and theorems.
10. The system must separate content, user, assignment, and analytics data.

## Acceptance Criteria

- AC1: A new user can register or join as a learner using the onboarding flow.
- AC2: A learner can access a problem page and complete a guided reasoning flow.
- AC3: A learner’s progress is saved and retrievable from the backend.
- AC4: A teacher can view learner progress by class.
- AC5: The backend exposes API endpoints for auth, content, progress, and assignment data.
- AC6: The database schema supports all core entities without duplication of critical information.
- AC7: The front-end routes correspond to the product flows shown in the prototype.
- AC8: The system is designed for future growth beyond the initial learner MVP.

## Technical Decisions

### Architecture

The team will implement a layered architecture with clear separation of concerns:

- Angular front-end for user interaction and UI
- .NET backend for API, business logic, and application services
- PostgreSQL as the source of truth for relational data
- repository/service pattern for clean backend logic
- DTOs for API contracts and validation

### Role-based access design

The system will use explicit roles, not ad hoc permission checks scattered throughout the codebase.

Roles:

- Visitor
- Learner
- Teacher
- Author
- Admin

Access decisions will be centralized in authorization logic.

### Data design principles

- each domain concept should map to a database table or precise collection of tables
- learner progress data should be persisted for tracking mastery over time
- content should be separated from user progress and classroom state
- the schema should allow future analytics and reporting without redesigning the data model

## Backend Design

### Backend structure

The .NET solution should be organized into logical layers:

- API project
- Application project
- Domain project
- Infrastructure project
- Shared contracts / DTOs

### Core backend responsibilities

- authentication and authorization
- user and role management
- geometry content retrieval
- theorem and definition queries
- proof validation workflow orchestration
- learner progress tracking and mastery updates
- teacher dashboard data aggregation
- class and assignment management
- authoring support for content updates

### Backend patterns

- controller endpoints for public and authenticated flows
- service layer for business logic
- repository layer for database access
- validation services for proof and assignment logic
- domain models for core entities like learners, classes, examples, and attempts

## Database Design

The PostgreSQL schema should support the following entities:

- Users
- Roles
- Learners
- Teachers
- Classes
- Enrollments
- Geometry examples
- Definitions
- Theorems
- Proof steps
- Attempts
- Progress records
- Mastery records
- Assignments
- Content review records

### Key design principles

- use relational data where strong structure and reporting are needed
- keep content, user data, and learning history distinct
- model mastery as derived from learner attempts, completed steps, and performance outcomes
- store timestamps for all created/updated events

## Frontend Design

### Angular structure

The Angular application should be organized by feature area:

- auth
- learner dashboard
- examples and problem solving
- theorem library
- teacher dashboard
- class management
- authoring
- shared components and services

### UI expectations

The front-end should match the prototype structure and product value, including:

- landing page and accessible home experience
- learner learning flow pages
- guided teaching-stage flow
- proof-builder interaction area
- theorem and definition navigation
- teacher analytics pages
- content authoring screens

### State management

Use Angular services and a predictable state pattern for:

- auth state
- learner progress state
- current problem state
- class and assignment data state
- teacher analytics summary state

## API Design

### Candidate endpoint groups

#### Auth

- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/logout
- GET /api/auth/me

#### Users and roles

- GET /api/users/{id}
- GET /api/users/me
- PATCH /api/users/{id}

#### Content

- GET /api/examples
- GET /api/examples/{id}
- GET /api/theorems
- GET /api/definitions

#### Learner progress

- GET /api/learners/{id}/progress
- POST /api/learners/{id}/attempts
- GET /api/learners/{id}/mastery

#### Classes and assignments

- GET /api/classes
- POST /api/classes
- GET /api/classes/{id}/learners
- POST /api/classes/{id}/assignments

#### Teacher analytics

- GET /api/teachers/{id}/analytics
- GET /api/classes/{id}/heatmap

#### Authoring

- POST /api/content/examples
- PUT /api/content/examples/{id}
- POST /api/content/theorems
- PUT /api/content/theorems/{id}

## Security and Access Rules

- Visitors can access public content and sample learning previews only.
- Learners can access only their own learner data and assigned content.
- Teachers can access class-level analytics and assigned learners only.
- Authors can manage content but should not be able to modify learner records outside their assigned scope.
- Admins can manage platform-wide settings and roles.
- All sensitive endpoints require authentication and proper role validation.

## Testing Strategy

The initial implementation should include:

- unit tests for service logic and validation rules
- API integration tests for the most important endpoints
- front-end component tests for key pages and flows
- role-based authorization tests
- database tests for critical relational constraints

## Risks / Open Questions

- How will mastery be formally calculated across stages and theorem categories?
- What is the exact v1 learner problem-solving flow expected by stakeholders?
- What content model is required for proofs, steps, and theorem relationships?
- Should teacher analytics be included in the first release or staged for v2?
- Are authors part of the first release or are they postponed to a later milestone?

## Next Actions

1. Finalize the exact MVP scope for the first release.
2. Define the user role permissions and authorization model.
3. Create the database schema draft for users, content, progress, and classes.
4. Start Angular and .NET project bootstrapping based on this specification.
5. Implement the learner flow before adding advanced analytics and authoring features.
