# Authorization Contract: Secure Dashboard Access

## Purpose

This contract defines the expected authorization boundaries for protected dashboard pages and service-level access checks.

## Rules

1. All protected pages require an authenticated user identity.
2. Project and task data must be filtered by the current user’s role and membership scope.
3. Managers must not see unrelated project data unless explicitly authorized.
4. Administrators may access elevated system views only when their role grants the required permission.
5. The service layer must deny unauthorized access even if a user manipulates the route or model state.

## Expected Result

If a user does not meet the required permission checks, the system returns no data or a redirect to the login/access-denied flow instead of exposing restricted project or task details.
