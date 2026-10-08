# Phase 1 Sprint Plan

- Date: 2026-10-08
- Phase: Foundation and Architecture
- Related Plan: `docs/planning/2026-10-08-product-roadmap-plan.md`
- Status: Draft

## Purpose

This document breaks Phase 1 into a sprint-by-sprint delivery plan. The goal is to establish the technical foundation and core system architecture needed before the learner MVP is built.

Phase 1 focuses on creating the platform foundation for the product direction described in the design artifacts:

- [ProjectStructure/EucliGeo and GeoPath combined design.html](../../ProjectStructure/EucliGeo%20and%20GeoPath%20combined%20design.html)
- [ProjectStructure/EucliGeo wireframes.html](../../ProjectStructure/EucliGeo%20wireframes.html)
- [ProjectStructure/EucliGeo clickable prototype.html](../../ProjectStructure/EucliGeo%20clickable%20prototype.html)

## Phase Objective

By the end of Phase 1, the project should have:

- a working Angular frontend shell
- a .NET backend skeleton with a clean architecture
- a PostgreSQL schema draft for the core domain
- role-based authentication and access control
- the base infrastructure for learner, teacher, and author workflows
- a clear path into the learner MVP

## Delivery Strategy

Phase 1 is planned in four sprints:

1. Sprint 1: Project bootstrap and architecture design
2. Sprint 2: Authentication, roles, and user foundation
3. Sprint 3: Core domain model and API foundation
4. Sprint 4: Frontend shell, integration, and Phase 1 readiness

## Sprint 1: Project bootstrap and architecture design

### Goal

Set up the repository and architecture so the team can build confidently across Angular, .NET, and PostgreSQL.

### Deliverables

- repo structure for frontend and backend projects
- Angular app initialised and configured
- .NET solution skeleton created
- PostgreSQL database setup plan
- shared architecture documentation
- environment configuration for local development

### Scope

- create Angular app structure and base modules
- create .NET solution and API project
- define project folders for API, domain, infrastructure, and shared contracts
- define local development configuration
- define required environment variables and secrets handling
- create architecture notes and service boundaries
- define coding conventions for the team

### User value

This sprint creates the foundation that every subsequent feature depends on. It does not deliver end-user functionality yet, but it sets the path for all future work.

### Exit criteria

- Angular app runs locally
- .NET API project builds successfully
- repo has a clear project layout
- team agrees on architecture boundaries
- local environment configuration is documented

### Dependencies

- product direction and roadmap reviewed
- chosen service layout agreed by team

### Risks

- architecture is too broad and slows delivery
- unclear boundaries between API, domain, and data layers

## Sprint 2: Authentication, roles, and user foundation

### Goal

Create the identity and authorization foundation for all user types.

### Deliverables

- authentication endpoints in the backend
- user model and role model
- default role definitions
- role-based access control rules
- Angular auth module and login page shell
- user session handling and protected route setup

### Scope

- define user entities and role mapping
- implement sign-up and sign-in APIs
- create auth middleware and role checks
- add JWT or equivalent secure token strategy
- define permission rules for Visitor, Learner, Teacher, Author, and Admin
- create Angular auth service and route guards
- build account creation and sign-in UI shell

### User value

This sprint enables all user journeys to be gated correctly and creates the base for learners, teachers, and authors to access the right features.

### Exit criteria

- users can register and log in
- role-based access is enforced
- protected routes reject unauthorized users
- user data is persisted in PostgreSQL
- base auth flow is ready for integration with learning features

### Dependencies

- Sprint 1 architecture complete
- database schema draft approved

### Risks

- role confusion between teacher, author, and admin
- security gaps in improper authorization checks

## Sprint 3: Core domain model and API foundation

### Goal

Define the domain data needed for the product and expose the main API surfaces used by the learner and teacher experience.

### Deliverables

- PostgreSQL schema draft for users, roles, classes, examples, theorems, definitions, progress, and mastery
- domain entities and relationships in the backend
- repository layer for core entities
- initial API routes for content and learner data
- seed or sample data for testing

### Scope

- define database tables and relationships
- implement migrations or schema setup scripts
- model core domain concepts for geometry education
- create content endpoints for examples, definitions, and theorems
- create learner progress endpoints
- create class and assignment entity models
- implement data access layer patterns

### User value

The product becomes data-backed rather than purely prototype-driven. This provides the platform with the core system structure needed for the actual learning experience.

### Exit criteria

- PostgreSQL database can store the core entities
- backend exposes at least the base content and learner endpoints
- data models align with the project prototype and user flows
- system can track students and classes in a structured way

### Dependencies

- Sprint 2 user and role model
- clear understanding of required domain entities

### Risks

- database design is too generic and will require large rework later
- content model does not reflect real learning flow requirements

## Sprint 4: Frontend shell, integration, and Phase 1 readiness

### Goal

Connect the frontend, backend, and database so the project moves from setup to a usable, testable foundation.

### Deliverables

- Angular app shell with routing and layout
- basic dashboard and shared UI components
- initial learner landing experience
- API integration layer for auth and content data
- Phase 1 QA checklist and readiness review

### Scope

- create app layout and base navigation
- implement landing and main dashboard views
- connect Angular services to the backend
- render sample content from PostgreSQL-backed APIs
- test protected routes and unauthorized access
- confirm the application skeleton aligns with user flows in the prototype
- document outstanding gaps prior to Phase 2

### User value

This sprint transforms the foundation into a working platform shell. It creates the visible starting point for the learner journey and confirms the project can proceed into the MVP phase.

### Exit criteria

- Angular app loads and navigates correctly
- backend endpoints respond successfully
- app can authenticate and display data from the backend
- core product shell is aligned with prototype structure
- the team is ready to start Phase 2: learner MVP development

### Dependencies

- Sprints 1–3 complete
- backend and database stable enough for integration

### Risks

- integration effort reveals schema mismatch or incomplete domain modeling
- UI architecture becomes too large before the MVP is validated

## Phase 1 Definition of Done

Phase 1 is complete when all of the following are true:

- Angular frontend shell is running and structured
- .NET backend is built and organized into clean layers
- PostgreSQL schema supports the core domain
- learners, teachers, authors, and admins have a defined role model
- authentication and authorization are working
- initial API routes exist for core domain data
- the app shell supports the product structure defined in the prototype
- Phase 2 MVP can begin without major architecture change

## Phase 1 Milestone Summary

### Milestone 1: Technical foundation ready

Completed at the end of Sprint 1 and Sprint 2.

### Milestone 2: Core domain and data model ready

Completed at the end of Sprint 3.

### Milestone 3: Frontend and backend working together

Completed at the end of Sprint 4.

## Recommended Sprint Order

1. Sprint 1 – Project bootstrap and architecture design
2. Sprint 2 – Authentication, roles, and user foundation
3. Sprint 3 – Core domain model and API foundation
4. Sprint 4 – Frontend shell, integration, and Phase 1 readiness

## Decisions to Confirm Before Sprint Execution

- exact user role permissions for Learner, Teacher, Author, and Admin
- whether the initial backend uses JWT or another auth model
- whether migration-based database setup is required for day-one development
- whether seed data will be included for testing and demos
- what the minimum viable content model is for the first phase

## Acceptance Summary

The Phase 1 sprint plan is successful when the team has a stable architecture and system skeleton that can support the learner MVP without requiring a major rebuild. The goal is not to deliver the full product yet, but to establish the right foundation for the work that follows.
