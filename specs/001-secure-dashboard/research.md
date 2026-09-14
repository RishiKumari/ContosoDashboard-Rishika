# Research: Secure Dashboard Access

## Decision

The dashboard will continue to use a Blazor Server application with cookie-based mock authentication and explicit service-level authorization checks. Access to project and task data will be scoped by user role and membership, while administrators retain broader visibility when explicitly allowed by the role policy.

## Rationale

The current architecture already contains the required pattern: authentication is configured in Program.cs, the custom auth provider is registered, and the service layer already performs user-scoped checks in ProjectService and related classes. This makes the secure dashboard implementation a fit for the existing structure without introducing new infrastructure or larger architecture changes.

## Alternatives considered

- Full identity provider integration with Azure Entra ID: rejected because the project is intentionally offline and training-focused and the mock auth system is already a documented learning mechanism.
- Client-side-only access filtering: rejected because it would not protect data if the API or service boundary were bypassed.
- Role-based access only in UI: rejected because UI checks are not enough; service-layer checks are required to prevent IDOR and unauthorized access.

## Findings

- Existing pages already use `AuthenticationStateProvider` and `[Authorize]` guards.
- Existing services perform authorization checks when retrieving projects and tasks.
- SQLite is the approved local persistence model for this repository and is already configured.
- The feature must enforce both role logic and data-scope logic to satisfy the secure dashboard requirement.
