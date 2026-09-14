# Quickstart: Secure Dashboard Access Validation

## Prerequisites

- .NET 10 SDK installed
- Project restored and built successfully
- SQLite database is available and the app can create/reset the local database file

## Setup

1. Open a terminal in the project root.
2. Run:
   ```powershell
   dotnet restore
   dotnet build
   dotnet run
   ```
3. Open the local app URL and log in using one of the seeded test accounts.

## Validation scenarios

### Scenario 1: Authenticated employee access

- Sign in as an employee account such as Ni Kang.
- Confirm the dashboard shows only that user’s data.
- Expected result: tasks and project information match the authenticated employee’s assigned work only.

### Scenario 2: Protected route blocking

- Open a protected page in an unauthenticated session or with an unknown user context.
- Expected result: the app redirects to the login experience and prevents the page from rendering restricted content.

### Scenario 3: Unauthorized project access attempt

- Sign in as a user who is not a member or manager of a project.
- Attempt to navigate directly to that project’s route.
- Expected result: access is denied and the user does not see the restricted project details.

### Scenario 4: Manager visibility and role separation

- Sign in as a project manager or team lead.
- Confirm they can access the relevant project scope but not unrelated users’ data.
- Expected result: project and team data are scoped correctly to the manager’s authorized area.

## Expected outcomes

- Users can authenticate and access only appropriate dashboard content.
- Unauthorized direct requests do not reveal hidden records.
- Security checks remain consistent across the main UI and service layer.
