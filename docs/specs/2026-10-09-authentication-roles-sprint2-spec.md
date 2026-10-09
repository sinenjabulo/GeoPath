# Feature Specification

- Date: 2026-10-09
- Feature: Sprint 2 Authentication and Role Foundation
- Related Plan: `docs/planning/2026-10-09-authentication-roles-sprint2-plan.md`
- Status: Complete (persistence superseded by Sprint 3)

## Summary

Sprint 2 creates the identity and authorization foundation for GeoPath. The project begins by defining user roles, protecting endpoints, and introducing a secure user-sign-in flow. The implementation is intentionally layered so it can later be replaced with PostgreSQL-backed persistence and Angular UI integration without breaking the API contract.

## User Stories

- As a learner, I want to create an account so I can access lesson content and track my progress.
- As a teacher, I want my role to be recognized so I can access class screens and reports.
- As an author, I want my content workflows to be restricted to the correct role.
- As an admin, I want a clear platform-level security model so sensitive actions are reserved for authorized users.
- As a visitor, I want public pages to remain visible without requiring login.

## Functional Requirements

1. The API must support registration with a name, email, password, and role.
2. User passwords must be hashed before storage.
3. The API must validate credentials during login and return an access token.
4. Roles must be explicit and represented as a controlled enum or equivalent model.
5. Protected endpoints must require an authenticated principal.
6. Protected endpoints may also require a specified role or set of roles.
7. The authenticated user context must expose identity claims such as user id, name, email, and role.
8. The backend must provide a clear extension point for future PostgreSQL persistence.

## Acceptance Criteria

- AC1: A user can register with a supported role.
- AC2: A user can log in with valid credentials and receive a token.
- AC3: The token grants access to an authenticated endpoint.
- AC4: A role-restricted endpoint rejects users lacking the required role.
- AC5: Passwords are not stored as plaintext values.
- AC6: The solution is structured so persistence can later move from in-memory storage to PostgreSQL without altering the API contract.

## Technical Decisions

### Architecture

The backend will use a layered design:

- Domain project contains the canonical user and role model.
- Application project contains authentication logic, DTOs, and services.
- Infrastructure project initially contained in-memory persistence and token generation; Sprint 3 replaced user persistence with PostgreSQL.
- API project wires up dependency injection, JWT authentication, and route authorization.

### Authentication strategy

JWT-based access tokens will be used for Sprint 2. They provide a standard, testable way to express authenticated identity and role claims while allowing the product to mature toward more advanced token handling later.

### Persistence strategy

Sprint 2 initially used an in-memory repository for development speed. Sprint 3 replaced it with EF Core and PostgreSQL while keeping the authentication API contract stable. Public registration creates Learner accounts; privileged roles must be assigned by an administrative process.

## Backend Design

### User and role model

The system defines the following default roles:

- Visitor
- Learner
- Teacher
- Author
- Admin

The role model is explicit and centralized in the domain project so authorization checks remain predictable.

### Auth service responsibilities

The authentication service will:

- validate registration data
- check for duplicate emails
- hash passwords
- create user entities
- verify login credentials
- issue tokens for authenticated users

### API contract

The API endpoints are:

- POST `/api/auth/register`
- POST `/api/auth/login`
- GET `/api/auth/me` (requires authentication)
- GET `/api/roles/teacher-only` (requires Teacher or Admin)
- GET `/api/roles/admin-only` (requires Admin)

## Database Design

### Sprint 2 database intent

This sprint does not require a production PostgreSQL schema yet. However, the domain and interface design anticipates a future table structure with:

- `users`
- `roles`
- `user_roles` or role assignments
- `user_sessions` or token metadata as needed

The in-memory implementation was development-only and has been removed in Sprint 3 in favor of PostgreSQL persistence.

## Frontend Design

The Angular client is currently not the main focus of Sprint 2, but the backend contract is structured so a future client can easily consume it. The expected flows include:

- sign-in screen
- account creation screen
- route guards for protected pages
- auth state service for token persistence

## API Design

The following DTOs are used:

- `RegisterRequest` with `name`, `email`, and `password`; new accounts receive the Learner role
- `LoginRequest` with `email` and `password`
- `AuthResponse` with `user`, `token`, and `expiresAtUtc`
- `UserSummary` with `id`, `name`, `email`, and `role`

Validation rules:

- names must be non-empty
- email addresses must contain `@` and a domain
- passwords must have at least 8 characters
- public registration cannot assign privileged roles
- duplicate email registration is rejected

## Security and Access

- Passwords are hashed using a one-way key-derivation process before storage.
- JWT signing keys must be configured through app settings and not hardcoded in production.
- Authorization checks use role claims and `[Authorize]` policies.
- Public endpoints remain accessible without authentication.
- Protected endpoints are explicit and clearly named to keep access logic reviewable.

## Testing Strategy

- build the server project to validate compile-time correctness
- execute auth flows through minimal HTTP endpoint calls
- validate role enforcement by requesting restricted endpoints with and without tokens
- confirm that duplicate registration and invalid credentials are rejected

## Risks / Open Questions

- The Sprint 3 schema uses EF Core and PostgreSQL; later changes should use new versioned migrations.
- Refresh tokens and logout semantics are intentionally deferred because Sprint 2 focuses on secure baseline identity.
- Angular route guards and a client-side auth module are recommended as a subsequent integration step.
