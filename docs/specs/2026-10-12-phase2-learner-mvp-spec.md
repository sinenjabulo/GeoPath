# Feature Specification

- Date: 2026-10-12
- Feature: Phase 2 Core Learner Experience (MVP)
- Related Plan: `docs/planning/2026-10-12-phase2-sprint-plan.md`
- Related Plans:
  - `docs/planning/2026-10-08-product-roadmap-plan.md`
  - `docs/planning/2026-10-08-phase1-sprint-plan.md`
- Related Specifications:
  - `docs/specs/2026-10-08-foundation-mvp-spec.md`
  - `docs/specs/2026-10-09-authentication-roles-sprint2-spec.md`
  - `docs/specs/2026-10-09-core-domain-api-sprint3-spec.md`
- Status: Draft

## Summary

Phase 2 delivers GeoPath's first end-to-end learner-facing product journey on top of the Phase 1 .NET, Angular, authentication, PostgreSQL, content, and attempt foundations. A visitor can preview the product; a learner can register or sign in, join an existing class, find geometry content, complete a guided example, and review saved progress.

The interface follows the four-stage reasoning model in the product design: Abstraction, Decomposition, Pattern recognition, and Solving. The Solving stage includes a content-authored statement-and-reason proof builder with deterministic feedback. It is not a general-purpose geometry prover.

Phase 2 records attempts, stage outcomes, and related theorem activity. It does not calculate or display numeric mastery, a mastery percentage, or an inferred mastery label. A mastery formula requires a separate product decision.

## User Stories

- As a visitor, I want to understand GeoPath and try a sample preview before registering.
- As a learner, I want to register and sign in so that my work is saved to my account.
- As a learner, I want to join an existing class using a code provided by my teacher.
- As a learner, I want to browse examples and supporting theorems and definitions.
- As a learner, I want to solve a geometry problem in clear reasoning stages.
- As a learner, I want specific feedback on my selected proof statements and reasons.
- As a learner, I want my responses and stage outcomes saved so I can return to unfinished work.
- As a learner, I want to review my attempts and stage/theorem activity without receiving an unapproved mastery score.

## Functional Requirements

### Learner entry and account

1. Public landing and sample-preview content must be available without authentication.
2. A learner must be able to register with the existing auth API; public registration assigns the Learner role and cannot assign Teacher, Author, or Admin.
3. A learner must be able to sign in, sign out, and retrieve the current authenticated identity.
4. Protected learner routes must redirect unauthenticated users to sign in and return them to the requested route after successful sign-in where practical.
5. Invalid credentials, expired sessions, validation errors, and API unavailability must have visible, actionable UI states.
6. Learner identity and authorization must be derived from the validated API principal, never from a client-supplied learner ID.

### Class joining

7. An authenticated learner must be able to join an existing class with a teacher-issued join code.
8. A join request must create at most one active enrollment for the learner and class.
9. Invalid or unavailable codes must not expose private class or learner information.
10. Class creation, code issuance/rotation, learner roster management, and assignments are not part of this learner join flow.

### Content discovery

11. Visitors and learners must be able to browse published examples, theorems, and definitions.
12. The example library must support topic and difficulty filtering when those fields are present in the API response.
13. An example view must show its prompt, topic, difficulty, and linked theorem/definition references.
14. A learner must be able to open a reference from the example without losing the current route, stage, or unsaved response.
15. Unpublished content and private class or learner data must not be exposed by public content views.

### Guided solving

16. A learner must be able to start an attempt for a published example or resume their own in-progress attempt.
17. Each attempt must present the stages in this order:
    1. Abstraction
    2. Decomposition
    3. Pattern recognition
    4. Solving
18. Each stage must present an age-appropriate prompt that guides the learner to identify givens and goals, decompose the diagram, recognize applicable patterns, or complete a proof.
19. Learners must be able to revisit earlier stages; saved responses must be restored.
20. The Solving stage must allow a learner to construct authored proof steps as statement-and-reason pairs.
21. Proof validation must compare learner choices against the example's authored expected steps and return useful step-level feedback.
22. Proof validation must not claim to prove arbitrary text, establish general geometric correctness, or replace content review.
23. Each recorded stage must support the existing outcomes: Correct, Partial, Incorrect, or Skipped.
24. Hints or guide usage must be recorded if the attempt contract supports it; the UI must not imply that asking for help is a failure.
25. The completion view must report completion and recorded outcomes without pass/fail language.

### Progress and history

26. A learner must be able to review their own attempts, including example title, status, dates, and recorded stage outcomes.
27. The dashboard must distinguish in-progress, completed, and not-yet-attempted examples and provide a path back to an in-progress attempt.
28. Progress views may group recorded attempt activity by thinking stage and by theorem linked to an attempted example.
29. Progress summaries must be traceable to stored attempts and stage results; they must not present percentages, mastery levels, recommendations based on mastery, or claims that a learner has mastered a theorem or stage.
30. Learners must not read or modify other learners' accounts, class information, attempts, or progress.

## Acceptance Criteria

- AC1: An unauthenticated visitor can load the landing page, preview, and published content.
- AC2: A new learner can register, sign in, reload the app, and sign out; privileged roles cannot be selected through public registration.
- AC3: A signed-in learner can join an existing class using a valid code; invalid and repeated joins do not create duplicate active enrollments or reveal private data.
- AC4: A learner can browse published examples and open their associated theorem and definition references.
- AC5: A learner can start an example attempt and work through Abstraction, Decomposition, Pattern recognition, and Solving in order.
- AC6: A learner's response and outcome for each stage can be saved, retrieved, and updated on their own attempt.
- AC7: The proof builder provides feedback that matches the authored expected proof steps and does not claim general proof validation.
- AC8: Opening and closing a theorem or definition reference returns the learner to the same stage and preserves the learner's work.
- AC9: A learner can resume an unfinished attempt and review completed attempts after a new session.
- AC10: Attempt, stage, class-enrollment, and progress APIs enforce authentication and ownership; changing an identifier cannot expose another learner's data.
- AC11: Progress summaries reconcile with persisted attempts and stage results and do not display numeric or inferred mastery.
- AC12: Automated UI/API tests and an end-to-end QA checklist cover the primary visitor-to-learner journey, responsive layout, keyboard operation, and error states.

## Technical Decisions

### Architecture and integration

- Continue the Angular client and layered .NET API architecture established in Phase 1.
- Use the existing auth, published-content, attempt, and stage-result contracts where possible; extend APIs only for learner journeys not supported by the current contract.
- PostgreSQL remains the source of truth for accounts, class enrollments, content, attempts, and stage outcomes.
- Use typed API models and explicit validation; do not make display labels or user identity authoritative client-side data.
- Keep routing and feature code organized around learner entry, content discovery, solving, and progress, following existing repository conventions.

### Content and proof format

Each in-scope example must provide enough versioned authored data to render its problem and check its proof-builder choices. The versioned data contract must define, at minimum:

- problem diagram or illustration reference and accessible description
- prompt and stage-specific instructions
- available responses or learner input shape for each stage
- ordered expected proof statements and reasons for the Solving stage
- accepted choices and the feedback shown for correct or incorrect selections
- optional hints and guide content
- related theorem and definition identifiers

Content is authored and reviewed outside this phase's product UI. Where the current `GuidedFlowJson` payload is insufficient, the API and domain contract must be extended deliberately and versioned; the client must not infer correctness from presentation text.

### Attempt and stage persistence

- Starting an example creates an attempt for the authenticated learner and selected published example.
- Updates target an attempt owned by the authenticated learner.
- Stage responses and outcomes use the existing attempt and stage-result model where possible.
- The API must define whether stage updates replace a prior result or append a new event; retries and network resubmission must not silently duplicate state.
- A completed attempt is marked complete only after the learner reaches the completion action or the server's explicit completion rule is met.
- Hint/guide counts may be recorded as response metadata or a separately versioned event; they must not be represented as correctness outcomes.

### Progress interpretation

Phase 2 displays raw, explainable evidence: attempt status, dates, stage outcomes, and the examples/theorems associated with those attempts. It does not combine these values into a score, percentage, badge, mastery level, or predictive recommendation.

Stage outcome values remain Correct, Partial, Incorrect, and Skipped. Any aggregate must state whether it counts attempts or unique examples and must not imply mastery. The product owner must approve a separate formula and interpretation before a later phase introduces mastery claims.

## Frontend Design

### Learner routes

The initial route map should cover these learner views, using the repository's established route conventions:

- public landing/sample preview
- register and sign in
- learner dashboard
- class join
- example library and example detail
- theorem and definition library/detail or contextual panels
- guided example stages and completion
- learner progress and attempt history

Route names are implementation details; their access requirements and user journeys are not.

### User experience requirements

- Preserve the learner's current stage and response while a contextual reference panel is open.
- Show progress through all four stages and make the current stage clear.
- Provide loading, empty, error, validation, offline/retry, and session-expired states.
- Keep the primary solver and proof controls usable at mobile widths and with keyboard navigation.
- Provide accessible text alternatives for diagrams and visible focus, labels, and validation feedback.
- Do not use color alone to communicate stage outcome.
- Do not use pass/fail wording on the completion view.

## API Design

Reuse existing API operations where available:

### Existing operations

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`
- `GET /api/examples`
- `GET /api/examples/{id}`
- `GET /api/theorems`
- `GET /api/definitions`
- `POST /api/attempts`
- `GET /api/attempts`
- `GET /api/attempts/{id}`
- `PUT /api/attempts/{id}/stages/{stage}`

### Phase 2 contract gaps to resolve

- class join by code for the authenticated learner, returning only the joined class summary
- learner dashboard data, either composed from existing learner-owned attempt/content calls or exposed through a narrowly scoped endpoint
- explicit attempt completion semantics and idempotent stage updates
- content detail/filter behavior needed by the library and contextual reference views
- guided-flow response schema and any metadata needed for proof feedback and guide/hint counts

Any new learner endpoint must derive the learner identity from the authenticated principal, validate request data, and avoid trusting user IDs supplied by the client.

## Security and Access

- Published geometry content and the public sample preview are publicly readable.
- Registration assigns Learner by default and cannot assign privileged roles.
- Joining a class requires an authenticated Learner and a valid teacher-issued code.
- Attempt creation, retrieval, update, completion, and progress are scoped to the authenticated learner.
- Learners cannot enumerate class rosters or inspect other learners' records.
- Class join errors must not disclose private class or user information.
- Secrets, access tokens, and passwords must not be logged or committed.
- Authorization and ownership behavior must be tested at the API boundary, not only hidden in the UI.

## Testing Strategy

- Angular unit/component tests for authentication states, routes, library filters, solver stages, proof feedback, progress views, and accessible error messages.
- .NET unit tests for join-code validation, duplicate-enrollment behavior, proof-step comparison, stage updates, completion rules, and progress aggregation.
- API integration tests for registration role restrictions, class joining, published content, attempt ownership, stage persistence, and progress privacy.
- Database tests for enrollment uniqueness and persistence across API restarts.
- End-to-end checks for visitor preview, learner registration/sign-in, join, content discovery, solving, interruption/resume, and progress review.
- Responsive and keyboard-navigation checks for the landing, library, solver, and progress views.
- Verify that no Phase 2 UI or API returns or renders a numeric mastery score or inferred mastery label.

## Risks / Open Questions

- Phase 1's frontend/API integration and readiness criteria must be verified before Phase 2 implementation begins.
- The product design's detailed stage prompts, accepted answers, and proof format need a versioned, reviewable content contract.
- Teacher-created class provisioning and join-code lifecycle are Phase 3 responsibilities; Phase 2 requires seeded or otherwise pre-existing classes.
- The appropriate client-side token storage and session recovery behavior must be confirmed against the app's deployment model.
- An exact content versioning and attempt-to-content snapshot policy may be required if authored examples change after attempts begin.
- Numeric mastery and its meaning remain intentionally deferred; future adoption requires product approval and validation against learner outcomes.
- Detailed teacher analytics, class management, authoring/review workflows, and arbitrary proof validation are out of scope.

## Next Actions

1. Confirm Phase 1 readiness against the running Angular, API, authentication, and PostgreSQL integration.
2. Approve the versioned guided-flow and proof-step content contract and prepare sample content.
3. Define the learner join endpoint and verify that a teacher-issued or seeded class is available for testing.
4. Implement the Phase 2 sprints in order, adding targeted unit, API integration, and end-to-end coverage.
5. Revisit numeric mastery only through an explicit product decision; until then, ship transparent attempt and stage progress only.
