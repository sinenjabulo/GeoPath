# Phase 2 Sprint Plan

- Date: 2026-10-12
- Phase: Core Learner Experience (MVP)
- Related Plan: `docs/planning/2026-10-08-product-roadmap-plan.md`
- Related Specification: `docs/specs/2026-10-12-phase2-learner-mvp-spec.md`
- Status: Draft

## Purpose

This document breaks Phase 2 into a sprint-by-sprint plan for delivering GeoPath's first end-to-end learner experience. Phase 1 establishes the platform shell, authentication, database, and initial content and attempt APIs; Phase 2 turns those foundations into a usable geometry-learning journey.

The learner experience should follow the existing product direction and design artifacts:

- [ProjectStructure/EucliGeo and GeoPath combined design.html](../../ProjectStructure/EucliGeo%20and%20GeoPath%20combined%20design.html)
- [ProjectStructure/EucliGeo wireframes.html](../../ProjectStructure/EucliGeo%20wireframes.html)
- [ProjectStructure/EucliGeo clickable prototype.html](../../ProjectStructure/EucliGeo%20clickable%20prototype.html)

## Phase Objective

By the end of Phase 2, a learner should be able to discover GeoPath, create or access a learner account, join an existing class, browse geometry content, complete a guided example, and revisit persisted progress.

Phase 2 records attempts and stage outcomes as observable progress. It does not assign numeric mastery scores or make mastery claims; the formula and product meaning remain deferred.

## Delivery Strategy

Phase 2 is planned in four sequential sprints, continuing the numbering used in Phase 1:

1. Sprint 5: Learner onboarding and dashboard
2. Sprint 6: Example and reference library
3. Sprint 7: Guided problem solving and proof builder
4. Sprint 8: Progress history and Phase 2 readiness

## Sprint 5: Learner onboarding and dashboard

### Goal

Give visitors a clear introduction to GeoPath and let learners register, sign in, join an existing class, and reach a useful learner home page.

### Deliverables

- public landing page with a sample learning preview
- learner registration, sign-in, sign-out, and current-session handling
- protected learner routes
- learner dashboard with a clear path to browse examples and resume an in-progress attempt
- learner class join flow using a teacher-issued join code
- API support for joining a class as the authenticated learner

### Scope

- integrate the Angular client with existing authentication and current-user endpoints
- retain the access token using the agreed client-side security approach and handle expired or invalid sessions
- default public sign-up to the Learner role; never allow a learner to grant themselves a privileged role
- accept a join code for an existing class and create one active enrollment for the signed-in learner
- show useful loading, empty, validation, and API error states
- keep class creation, management, and assignment workflows out of this sprint

### User value

Learners can move from first visit to an authenticated starting point without relying on the clickable prototype or developer-only API calls.

### Exit criteria

- a visitor can view the public landing page and sample preview without signing in
- a learner can register, sign in, reload, and sign out with correct protected-route behavior
- public registration cannot assign Teacher, Author, or Admin
- a learner can join an existing class with a valid code and sees the class on their dashboard
- invalid or repeated class joins are handled without creating duplicate active enrollments
- frontend and API integration tests cover the learner entry and join flow

### Dependencies

- Phase 1 readiness: Angular shell and routing, deployed API configuration, PostgreSQL migrations, authentication, and published sample content are available
- teacher-created classes or suitable seeded classes exist for join-flow testing

### Risks

- Phase 1's integration and readiness work may not yet be complete in the implementation
- client-side token handling can expose credentials if storage and route behavior are not reviewed
- class join must not reveal private class or learner information

## Sprint 6: Example and reference library

### Goal

Help learners find and understand published geometry examples, theorems, and definitions before starting an attempt.

### Deliverables

- searchable or filterable example library
- example detail and problem-introduction view
- theorem and definition library/detail views
- contextual theorem and definition panel available from an example
- published sample content that exercises the agreed learner flow

### Scope

- integrate public example, theorem, and definition API responses
- support topic and difficulty browsing where the published data allows it
- show each example's prompt, topic, difficulty, and related theorem/definition references
- keep learners on the current problem stage when they open and close a reference panel
- provide clear empty, loading, and unavailable-content states
- keep content creation, editing, review, and publication workflows out of scope

### User value

Learners can locate a relevant example and consult its supporting geometry references without losing their place.

### Exit criteria

- a learner can browse published examples and open an example detail view
- a learner can browse theorem and definition references and open linked content from an example
- unpublished content is not exposed through public views
- the library is usable on mobile-sized screens and with keyboard navigation
- frontend and API tests cover content loading, filtering, reference links, and unavailable content

### Dependencies

- Sprint 5 route and shell foundation
- published sample examples include sufficient prompt and reference data

### Risks

- the current optional guided-flow payload may not yet contain all content needed for a meaningful problem view
- sparse seed content could make library filters appear broken or empty

## Sprint 7: Guided problem solving and proof builder

### Goal

Deliver the core learning activity: a learner can work through an example using the four thinking stages and receive deterministic, content-authored feedback.

### Deliverables

- guided four-stage solver: Abstraction, Decomposition, Pattern recognition, and Solving
- accessible problem diagram and stage navigation
- contextual theorem/definition panel and stage-appropriate guide/hints
- statement-and-reason proof builder for the Solving stage
- stage response, outcome, and attempt persistence through the existing learner APIs
- completion view that communicates effort and recorded outcomes without pass/fail framing

### Scope

- create or resume a learner-owned attempt for a published example
- collect stage responses and record Correct, Partial, Incorrect, or Skipped outcomes
- validate proof-builder choices against authored expected steps; do not claim general theorem proving or validate arbitrary free-form proofs
- allow learners to revisit earlier stages and preserve their saved responses
- record hint/guide usage where the attempt contract supports it
- handle network and validation errors without losing unsaved learner work
- keep automated diagram construction, natural-language proof evaluation, and advanced geometry computation out of scope

### User value

Learners get a repeatable and supportive reasoning process instead of a single answer field, while their work is saved for later review.

### Exit criteria

- a learner can complete the four stages in order and can revisit saved stages
- stage outcomes and responses persist and are scoped to the authenticated learner
- proof-builder feedback matches the authored answer key and offers a clear next action
- a learner cannot read or change another learner's attempt
- theorem/definition panels preserve the current stage and response
- integration tests cover the complete attempt lifecycle, including interrupted and resumed work

### Dependencies

- Sprint 6 provides published examples and reference data
- the agreed versioned guided-flow/proof-step content format is available for each in-scope sample
- Phase 1 attempt and stage-result APIs are stable

### Risks

- ambiguous or unversioned authored answer data can produce misleading validation
- autosave and retries may create duplicate or stale stage results unless updates are idempotent
- a narrow authored-answer validator must be clearly distinguished from general proof correctness

## Sprint 8: Progress history and Phase 2 readiness

### Goal

Let learners review persisted attempts and stage outcomes, resume unfinished work, and close Phase 2 with end-to-end quality checks.

### Deliverables

- learner progress/history view with recent and completed attempts
- per-stage and per-theorem attempt/outcome summaries based on persisted records
- dashboard links to continue an in-progress attempt and revisit completed work
- explicit progress labels with no numeric mastery score or mastery claim
- Phase 2 end-to-end QA and release-readiness checklist

### Scope

- aggregate only recorded attempts, stage outcomes, and theorems linked to the worked examples
- distinguish in-progress, completed, and not-yet-attempted content
- show attempt dates and recorded stage outcomes; avoid inferred achievement percentages
- ensure users can see only their own learner history
- validate the full visitor-to-returning-learner journey at desktop and mobile breakpoints
- record unresolved product and content decisions for a later increment

### User value

Learners can see what they have worked on, return to unfinished examples, and understand their recorded progress without being assigned an unapproved mastery rating.

### Exit criteria

- a learner can retrieve their own attempt history after signing out and back in
- the dashboard links to the correct unfinished attempt or example
- progress summaries agree with persisted stage results and make no numeric mastery claims
- end-to-end checks cover registration/sign-in, class join, content discovery, solving, and progress review
- accessibility, responsive layout, API authorization, and error-state checks are complete
- outstanding gaps and deferred decisions are recorded before Phase 3 planning begins

### Dependencies

- Sprints 5–7 are complete
- attempt and stage-result data is persisted in PostgreSQL
- theorem-to-example relationships are available for progress grouping

### Risks

- per-theorem summaries can mislead if a learner's attempt is counted against content not actually used in their solution
- incomplete, abandoned, or retried attempts need consistent presentation
- stakeholders may expect a mastery score despite the explicit deferral

## Phase 2 Definition of Done

Phase 2 is complete when all of the following are true:

- a visitor can preview the product and a learner can use the authenticated learner journey
- a learner can join an existing class without gaining access to other learners' data
- a learner can find a published geometry example and its related theorem and definition references
- a learner can complete and revisit a persisted four-stage attempt with authored proof-step feedback
- a learner can review their own attempt history and stage/theorem progress
- no numeric mastery score or mastery label is presented until a product-approved formula exists
- the primary learner journey is validated by automated tests and an end-to-end QA checklist
- teacher dashboards, class creation and assignment management, authoring, and advanced proof evaluation remain outside Phase 2

## Phase 2 Milestone Summary

### Milestone 1: Learner access ready

Completed at the end of Sprint 5.

### Milestone 2: Geometry content discoverable

Completed at the end of Sprint 6.

### Milestone 3: Guided learning journey usable

Completed at the end of Sprint 7.

### Milestone 4: Learner progress reviewable

Completed at the end of Sprint 8.

## Recommended Sprint Order

1. Sprint 5 – Learner onboarding and dashboard
2. Sprint 6 – Example and reference library
3. Sprint 7 – Guided problem solving and proof builder
4. Sprint 8 – Progress history and Phase 2 readiness

## Decisions to Confirm Before Sprint Execution

- Phase 1 integration/readiness exit criteria are met in the running implementation
- learner token storage and session-expiration behavior are approved
- the versioned content shape for diagrams, stage prompts, proof steps, expected answers, and hints is agreed
- stage scoring and hint/guide usage events are defined consistently across UI and API
- teacher-created class provisioning is available for learners to join
- numeric mastery remains deferred pending an explicit product-approved formula

## Acceptance Summary

The Phase 2 sprint plan is successful when GeoPath delivers a usable, persisted learner journey from onboarding through a guided geometry attempt and progress review. The phase prioritizes transparent evidence of learning activity and defers unsupported mastery claims, teacher analytics, and authoring functionality.
