# Feature Plan

- Date: 2026-10-08
- Feature: GeoPath Product Roadmap and Initial Feature Priorities
- Purpose: Define the first feature set to build in order, identify the core MVP, and establish the correct starting point for the platform before implementation begins.
- Status: Draft

## Problem / Context

The project currently contains product concept documents, wireframes, and a clickable prototype for a geometry learning platform. These design artifacts show a clear product direction: a learner-focused Euclidean geometry platform with guided reasoning, theorem access, proof building, mastery tracking, and teacher analytics.

To move from prototype to production, the team needs a structured roadmap. This prevents building in the wrong order, prevents scope drift, and ensures every feature aligns with the product direction shown in the project files.

## Product References

The following files are the source of truth for the expected product behavior and structural direction:

- [ProjectStructure/EucliGeo and GeoPath combined design.html](../../ProjectStructure/EucliGeo%20and%20GeoPath%20combined%20design.html)
- [ProjectStructure/EucliGeo wireframes.html](../../ProjectStructure/EucliGeo%20wireframes.html)
- [ProjectStructure/EucliGeo clickable prototype.html](../../ProjectStructure/EucliGeo%20clickable%20prototype.html)

## Target Users

- Learners: South African high school students studying Euclidean geometry
- Teachers: classroom managers who assign work and review learner progress
- Authors / Content managers: people who create and maintain examples, definitions, and theorems
- Visitors: non-registered users who can preview content and try a sample example

## Goals

1. Build a production-ready learning platform aligned to the design prototype.
2. Deliver the highest-value user journeys first.
3. Establish the architecture needed for a scalable product.
4. Prioritize features that support learning outcomes, progress tracking, and teacher visibility.
5. Keep implementation structured and reviewable through planning and specification documents.

## Scope

### In scope

- learner landing and onboarding experience
- example browsing and guided geometry learning flow
- four-stage reasoning model
- theorem and definition content access
- proof builder and step validation
- learner mastery and progress tracking
- class and teacher management
- teacher analytics and reports
- authoring workflow for content creation and review
- backend services, data models, and API structure
- Angular front-end application shell and feature modules
- PostgreSQL schema for product data

### Out of scope

- advanced social features
- marketplace or external integrations beyond core product needs
- large-scale multiplayer classroom features in v1
- non-geometry educational modules outside the defined domain
- highly advanced gamification systems before core learning features are stable

## Requirements Summary

The platform must support the following core experiences:

- A learner can view geometry examples and work through them in a guided structure.
- A learner can understand theorems and definitions in context.
- A learner can complete a problem using a structured reasoning flow.
- A learner can build a proof and receive validation feedback.
- A teacher can view class-level performance trends and weak areas.
- A teacher can assign practice and monitor progress.
- An author can create or update geometry examples and supporting learning content.
- The system stores progress, mastery data, and learning records reliably.

## Proposed Roadmap

### Phase 1: Foundation and architecture

This stage establishes the platform foundation and ensures all later work is built on a sound structure.

#### Features

- Angular application shell and routing
- .NET API project structure and dependency setup
- PostgreSQL data model design
- authentication and authorization baseline
- user roles and access controls
- environment configuration and deployment basics
- project documentation structure for planning and specs

#### Deliverables

- base Angular app with module structure
- .NET backend skeleton with API layer
- PostgreSQL schema for users, roles, and core domain entities
- base security model for learner, teacher, author, and admin roles

#### Why this comes first

Without the platform foundation, all downstream features would be inconsistent and hard to scale.

### Phase 2: Core learner experience (MVP)

This is the most important phase and should be treated as the first meaningful product milestone.

#### Features

- landing page and public education content
- sign-in / account creation and class join flow
- learner dashboard
- example library
- interactive geometry problem views
- guided four-stage reasoning flow
- theorem and definition pages
- proof builder interface
- answer validation and step feedback
- progress tracking per learner
- mastery model per theorem and stage

#### Deliverables

- working learner journey from onboarding to example completion
- functional proof-solving experience
- persistent learner progress tracking
- ability to review example progress and mastery status

#### Why this comes first

This is the central value proposition of the platform. If the learner experience is not strong, the rest of the system has limited value.

### Phase 3: Teacher and class management

Once the learner flow is working, the teacher product becomes essential.

#### Features

- class creation and management
- learner enrollment and invite flow
- assignment creation
- per-student and per-class analytics
- heatmap views by theorem and stage
- recommendations for weak areas
- export or reporting flows

#### Deliverables

- teacher dashboard with learner progress overview
- class-level insights and assignment management
- ability to identify learner difficulties quickly

#### Why this comes second

Teachers add accountability and classroom value, but the learner experience remains the product core.

### Phase 4: Authoring and content lifecycle

This phase supports content creation and quality control.

#### Features

- example authoring workflow
- theorem and definition editing
- lesson and question publishing flow
- content review and approval process
- quality checks to prevent incorrect proofs or diagrams

#### Deliverables

- authoring interface for geometry content
- content publishing workflow
- governance for learning material accuracy

#### Why this comes later

The authoring tool is important, but it should not block the first learner and teacher flows.

### Phase 5: Performance, hardening, and scale

This phase covers the production-readiness layer.

#### Features

- performance tuning for geometry interactions
- analytics and caching strategy
- security review and access hardening
- audit logging
- automated tests and CI validation
- deployment readiness

#### Deliverables

- stable production-ready baseline
- monitoring and maintainability improvements
- reliability improvements for scaled usage

## Recommended Delivery Order

The most sensible order for this project is:

1. Foundation and architecture
2. Learner experience MVP
3. Teacher workflows and analytics
4. Authoring/content tools
5. Hardening and scale improvements

## Dependencies and Risks

### Technical dependencies

- Angular app structure must support modular front-end feature growth
- .NET backend must provide clean separation between API, business logic, and data access
- PostgreSQL schema must support learner, teacher, class, content, and progress data
- authentication and authorization must be designed for multiple roles

### Product risks

- prototype may overestimate how much interactive geometry functionality can be built quickly in v1
- learner experience may require more design iteration around solving flows and difficulty curves
- class analytics must be based on reliable mastery logic or they will not be trustworthy

### Operational risks

- content quality must be controlled to prevent wrong answers or misleading proofs
- access restrictions must be clear for learner, teacher, and author roles

## Proposed Next Steps

1. Create the architecture plan and technical spec for the foundation.
2. Define the MVP user flows for learner onboarding and example solving.
3. Model the core database schema for users, roles, classes, progress, geometry content, and mastery data.
4. Build the Angular shell and .NET API skeleton.
5. Implement the learner journey before adding advanced analytics or authoring features.

## Decisions Needed

The following decisions should be made before full implementation begins:

- exact first release scope (MVP vs. later phases)
- minimum learner role functionality for v1
- whether teacher analytics is part of v1 or v2
- whether authoring is included in v1 or deferred
- how mastery is calculated and tracked across the different thinking stages
- how geometry content will be stored and versioned

## Acceptance Summary

This plan is considered successful when:

- the project has a clear sequence of feature delivery
- the MVP focuses on learner value first
- the architecture supports the chosen .NET, PostgreSQL, and Angular stack
- the roadmap aligns with the existing design files
- feature work is documented in planning and specification files before coding starts
