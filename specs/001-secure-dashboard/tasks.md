# Tasks: Secure Dashboard Access

**Input**: Design documents from `/specs/001-secure-dashboard/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Confirm the project and environment are ready for secure dashboard work.

- [ ] T001 Confirm the .NET 10 and SQLite baseline is applied in ContosoDashboard/ContosoDashboard.csproj and ContosoDashboard/appsettings.json
- [ ] T002 [P] Verify the SQLite database configuration and startup wiring in ContosoDashboard/Program.cs
- [ ] T003 [P] Validate the current model and service structure in ContosoDashboard/Data/ApplicationDbContext.cs, ContosoDashboard/Models/, and ContosoDashboard/Services/

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the core authorization and data-access foundation that all stories depend on.

- [ ] T004 Ensure the database context includes user, project, task, notification, and membership relationships in ContosoDashboard/Data/ApplicationDbContext.cs
- [ ] T005 [P] Configure authentication and role policies in ContosoDashboard/Program.cs for employee, team lead, project manager, and administrator access
- [ ] T006 [P] Register scoped application services and dependency wiring in ContosoDashboard/Program.cs
- [ ] T007 Implement shared authorization checks for project and task access in ContosoDashboard/Services/ProjectService.cs and ContosoDashboard/Services/TaskService.cs
- [ ] T008 [P] Harden unauthenticated navigation flow and redirect handling in ContosoDashboard/Shared/RedirectToLogin.razor and ContosoDashboard/Pages/Login.cshtml
- [ ] T009 Run the baseline build and confirm the project compiles before user story work begins

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel.

---

## Phase 3: User Story 1 - Sign in to the dashboard and view personalized work (Priority: P1) 🎯 MVP

**Goal**: Deliver the secure landing experience for an authenticated user.

**Independent Test**: Sign in with a seeded user and confirm the dashboard shows only that user’s tasks, notifications, and project summary.

### Implementation for User Story 1

- [ ] T010 [US1] Update the dashboard summary and current-user lookup in ContosoDashboard/Pages/Index.razor
- [ ] T011 [US1] Validate the dashboard summary data source and notification data in ContosoDashboard/Services/DashboardService.cs and ContosoDashboard/Services/NotificationService.cs
- [ ] T012 [P] [US1] Confirm the login page and authentication state provider support the seeded user model in ContosoDashboard/Pages/Login.cshtml and ContosoDashboard/Services/CustomAuthenticationStateProvider.cs
- [ ] T013 [US1] Add access checks and redirect behavior for unauthenticated users in ContosoDashboard/Program.cs and ContosoDashboard/Shared/RedirectToLogin.razor
- [ ] T014 [US1] Verify the dashboard loads only the current user’s data and blocks attempts to view unrelated content

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Review project work and team status (Priority: P1)

**Goal**: Restrict project visibility to authorized managers, team leads, and members.

**Independent Test**: Sign in as a project manager or team member and confirm only authorized project data appears, while unauthorized project access is denied.

### Implementation for User Story 2

- [ ] T015 [US2] Harden project retrieval and authorization in ContosoDashboard/Services/ProjectService.cs for manager, member, and unauthorized access cases
- [ ] T016 [P] [US2] Update ContosoDashboard/Pages/Projects.razor to display only authorized project data and role-aware metadata
- [ ] T017 [US2] Update ContosoDashboard/Pages/ProjectDetails.razor to deny direct access to unauthorized project routes and show only allowed project details
- [ ] T018 [US2] Review ContosoDashboard/Models/Project.cs and ContosoDashboard/Models/ProjectMember.cs to confirm the membership rules support role-based visibility
- [ ] T019 [US2] Validate manager/team access boundaries and unauthorized route protection with a smoke test

**Checkpoint**: At this point, User Stories 1 and 2 should both work independently.

---

## Phase 5: User Story 3 - Manage and update work items safely (Priority: P2)

**Goal**: Enforce task-level scoping so users can only manage their assigned or authorized work.

**Independent Test**: Sign in as an employee and update task status, then verify other users’ tasks remain inaccessible even through direct URL or manual manipulation.

### Implementation for User Story 3

- [ ] T020 [US3] Restrict task retrieval and updates to the current user’s scope in ContosoDashboard/Services/TaskService.cs
- [ ] T021 [P] [US3] Update ContosoDashboard/Pages/Tasks.razor to filter and update only authorized tasks and statuses
- [ ] T022 [US3] Review ContosoDashboard/Models/TaskItem.cs and related task service logic to confirm due date, status, and project associations remain security-safe
- [ ] T023 [US3] Add safe task access checks for direct access and unauthorized updates before saving task changes
- [ ] T024 [US3] Validate a user cannot modify or view another user’s task data through direct action or route tampering

**Checkpoint**: User Story 3 should be independently functional and safe.

---

## Phase 6: User Story 4 - Administer the system with elevated permissions (Priority: P2)

**Goal**: Provide the correct administrator and manager controls without weakening the access model.

**Independent Test**: Sign in as an administrator and confirm elevated access works while non-admin users are blocked from administrative routes.

### Implementation for User Story 4

- [ ] T025 [US4] Apply admin and elevated-role checks to the user management and access model in ContosoDashboard/Services/UserService.cs and ContosoDashboard/Program.cs
- [ ] T026 [P] [US4] Update team and profile surface area in ContosoDashboard/Pages/Team.razor and ContosoDashboard/Pages/Profile.razor to respect access roles and display only authorized data
- [ ] T027 [US4] Confirm administrator-only routes and service calls fail cleanly for non-privileged users
- [ ] T028 [US4] Run a manager/admin access smoke test to confirm the security boundary remains consistent across screens

**Checkpoint**: All user stories should now be independently functional.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Finish the hardening and validation work that affects the whole feature.

- [ ] T029 [P] Review and tighten logging, failure messages, and error handling across ContosoDashboard/Program.cs, ContosoDashboard/Services/, and ContosoDashboard/Pages/
- [ ] T030 [P] Validate the secure dashboard quickstart scenarios from specs/001-secure-dashboard/quickstart.md
- [ ] T031 Review the authorization contract in specs/001-secure-dashboard/contracts/authorization-contract.md and confirm all major access paths align with it
- [ ] T032 Security hardening pass across service-layer enforcement and route-level access checks

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Story phases (Phases 3-6)**: All depend on Foundational completion
- **Polish (Phase 7)**: Depends on all desired stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational; independent MVP
- **User Story 2 (P1)**: Can start after Foundational; builds on the same access model
- **User Story 3 (P2)**: Can start after Foundational and may reuse the same service checks
- **User Story 4 (P2)**: Can start after Foundational; depends on the security model and role policies

### Parallel Opportunities

- Setup tasks T001-T003 can be completed in parallel
- Foundational tasks T005-T008 can be completed in parallel after T004 is in place
- US1 tasks T010-T014 can proceed in parallel once the foundation is ready
- US2 tasks T015-T019 can proceed in parallel once the project authorization model is established
- US3 tasks T020-T024 can proceed in parallel once task access rules are in place
- US4 tasks T025-T028 can proceed in parallel once the admin role model is in place

---

## Parallel Example: User Story 1

```bash
# Example parallel work stream after foundation is ready
Task: "Update dashboard summary and current-user lookup in ContosoDashboard/Pages/Index.razor"
Task: "Validate dashboard summary data source in ContosoDashboard/Services/DashboardService.cs"
Task: "Confirm login and auth-provider behavior in ContosoDashboard/Pages/Login.cshtml"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate the login and dashboard flow
5. Deploy or demo once the access model is confirmed

### Incremental Delivery

1. Setup + Foundations → secure baseline ready
2. User Story 1 → personalized dashboard and login restrictions
3. User Story 2 → project access controls
4. User Story 3 → task-level data scoping
5. User Story 4 → admin and elevated-role checks
6. Polish → final security review and validation

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Developer A: User Story 1
3. Developer B: User Story 2
4. Developer C: User Story 3
5. Developer D: User Story 4
6. Final hardening pass completed by the team

---

## Notes

- [P] tasks are for distinct files or workstreams with no dependency on incomplete tasks
- Each story remains independently testable and can be validated in isolation
- The secure dashboard work should prioritize access control before feature polish
- Each task includes a file path and a clear action to limit ambiguity during implementation
