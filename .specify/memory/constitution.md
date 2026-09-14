<!--
Sync Impact Report
- Version change: 0.0.0 -> 1.0.0
- Modified principles: new constitution scaffold established for ContosoDashboard
- Added sections: Security and Data Handling; Development Workflow
- Removed sections: none
- Deferred items: TODO(RATIFICATION_DATE): original adoption date not recorded in project history.
-->

# ContosoDashboard Constitution

## Core Principles

### I. Learning-First and Safe-by-Default
This project exists to teach secure application design, Spec-Driven Development, and clear engineering habits. Every feature must be understandable to a learner, explainable in plain language, and safe to demonstrate in a training environment. Production assumptions must be explicitly labeled as training-only when they are intentionally simplified.

### II. Security and Authorization Must Be Enforced
Authentication, authorization, and data access must be enforced at both the UI and service boundaries. Users must never be able to access data or actions outside their authorized scope through URL tampering, role confusion, or direct object references. Mock authentication is acceptable only for training and must remain clearly separated from production identity systems.

### III. Spec-Driven Delivery Is Mandatory
Any user-visible change or new capability must begin with a clear specification, acceptance criteria, and traceable implementation tasks. The team must not bypass planning with undocumented or ad hoc changes. This is required to keep the project teachable, measurable, and consistent with the Spec Kit workflow.

### IV. Test-First and Verifiable Change
Behavioral changes must be verified with a failing test or equivalent reproducible check before implementation, followed by a passing validation after the fix. The project must not merge code that cannot be demonstrated to work through the relevant build or test command. This principle protects reliability and keeps the learning model grounded in evidence.

### V. Simplicity, Separation of Concerns, and Maintainability
The architecture must favor small, understandable components, explicit services, and clear boundaries between UI, business logic, and data access. Unnecessary abstractions, hidden side effects, and duplicated logic are prohibited unless they are justified by a documented requirement. The system must remain maintainable for training and review.

## Security and Data Handling

The application must treat security as a product requirement, not as a later enhancement. Sensitive operations must use explicit authorization checks, role-based policies, and user-scoped data access. The system must avoid insecure defaults and must not assume that UI visibility alone provides protection.

Data storage must be appropriate to the training context and platform constraints. When local persistence is used, it must be clearly documented and remain portable across developer environments. For this project, local SQLite is the approved default because it avoids dependency on SQL Server LocalDB and works reliably on ARM64 systems.

## Development Workflow

All work must follow a repeatable process: define the problem, capture the requirement, plan the change, implement the change, validate the result, and document any assumptions. Pull requests and code review must confirm that the change still aligns with the constitution, the relevant spec, and the expected user value. Any temporary exceptions must be documented and reviewed rather than silently accepted.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): original adoption date not recorded in project history | **Last Amended**: 2026-09-14

## Governance

This Constitution governs all repository work for ContosoDashboard. It supersedes contradictory local practices, informal conventions, or undocumented assumptions when a conflict exists. Compliance is reviewed during planning, code review, and validation before a change is considered complete.

Amendments require a written change to this constitution, a clear rationale for the change, and a review of the impact on the project principles and workflow. The amendment must identify the affected principle or section and explain whether the change adds, removes, or redefines governance requirements.

Versioning follows semantic versioning rules:

- MAJOR: backward-incompatible principle removals or redefinitions, or any governance rule that significantly changes required behavior
- MINOR: addition of a new principle or section, or material expansion of guidance that changes expected practice
- PATCH: clarification, wording, typo corrections, and non-semantic refinements

All amended versions must record the change in the constitution and update the version metadata. If the project changes materially, the governance team must review whether new principles, workflow controls, or security requirements are required before the next implementation cycle begins.

