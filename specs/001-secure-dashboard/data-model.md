# Data Model: Secure Dashboard Access

## Entities

### User

- UserId: integer, primary key
- Email: string, unique
- DisplayName: string
- Department: string, optional
- JobTitle: string, optional
- Role: enum (`Employee`, `TeamLead`, `ProjectManager`, `Administrator`)
- AvailabilityStatus: enum
- CreatedDate: datetime
- LastLoginDate: datetime, optional
- Notification preferences: booleans
- Relationships: assigned tasks, managed projects, project memberships, notifications

### Project

- ProjectId: integer, primary key
- Name: string
- Description: string, optional
- ProjectManagerId: integer, foreign key to User
- StartDate: datetime
- TargetCompletionDate: datetime, optional
- Status: enum (`Planning`, `Active`, `OnHold`, `Completed`)
- CreatedDate: datetime
- UpdatedDate: datetime
- Relationships: project manager, tasks, project members

### TaskItem

- TaskId: integer, primary key
- Title: string
- Description: string, optional
- Priority: enum (`Low`, `Medium`, `High`, `Critical`)
- Status: enum (`NotStarted`, `InProgress`, `Completed`)
- DueDate: datetime, optional
- AssignedUserId: integer, foreign key to User
- CreatedByUserId: integer, foreign key to User
- ProjectId: integer, optional, foreign key to Project
- CreatedDate: datetime
- UpdatedDate: datetime
- Relationships: assigned user, creator, project, comments

### ProjectMember

- ProjectMemberId: integer, primary key
- ProjectId: integer, foreign key to Project
- UserId: integer, foreign key to User
- Role: string
- AssignedDate: datetime

### Notification

- NotificationId: integer, primary key
- UserId: integer, foreign key to User
- Title: string
- Message: string
- CreatedDate: datetime
- IsRead: boolean
- RelatedProjectId: integer, optional

### Announcement

- AnnouncementId: integer, primary key
- Title: string
- Content: string
- CreatedByUserId: integer, foreign key to User
- PublishDate: datetime
- ExpiryDate: datetime, optional
- IsActive: boolean

## Validation Rules

- Users must have unique email addresses.
- Project names must be present and not empty.
- Task titles must be present and not empty.
- Authentication must establish the current user identity before loading restricted lists.
- Access rules must validate project membership or manager ownership before exposing project details.

## Security Model

- Employees can access their own tasks and assigned project views.
- Team leads and project managers can view their managed scope.
- Administrators can view broader system data when role permissions allow.
- Service methods must reject unauthorized access with null or empty results rather than exposing restricted data.
