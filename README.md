# Online Catalog
ASP.NET Core Razor Pages application for university course management

## Features
- Role-based access (Student, Professor, Secretary, Moderator)
- Course management
- Grade tracking
- PDF certificate generation
- Real-time notifications

## Setup
1. Create MySQL database using schema.sql
2. Update connection string in appsettings.json
3. Run application

## Structure:

OnlineCatalog/

├── Areas/

│   └── Identity/

│       └── Pages/

│           └── Account/

│               ├── Login.cshtml

│               ├── Login.cshtml.cs

│               ├── Register.cshtml

│               └── Register.cshtml.cs

├── Data/

│   ├── AppDbContext.cs

│   └── RoleTypes.cs

├── Hubs/

│   └── MessageHub.cs

├── Models/

│   ├── ApplicationUser.cs

│   ├── Course.cs

│   ├── Enrollment.cs

│   └── Notification.cs

├── Pages/

│   ├── Courses/

│   │   └── Search.cshtml (+.cs)

│   ├── Moderator/

│   │   └── Courses/

│   │       └── Edit.cshtml (+.cs)

│   ├── Professor/

│   │   └── GradeBook.cshtml (+.cs)

│   ├── Secretary/

│   │   └── Export.cshtml (+.cs)

│   └── Student/

│       ├── Certificate.cshtml (+.cs)

│       └── Courses.cshtml (+.cs)

├── Services/

│   └── NotificationService.cs

├── wwwroot/

│   └── css/

│       └── site.css

├── Pages/

│   └── Shared/

│       ├── _Layout.cshtml

│       ├── _LoginPartial.cshtml

│       └── _ValidationScriptsPartial.cshtml

├── .gitignore

├── appsettings.json

├── Program.cs

└── README.md (optional)

## Libraries:

- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

- Microsoft.VisualStudio.Web.CodeGeneration.Design

- Microsoft.AspNetCore.Identity.EntityFrameworkCore

- Microsoft.AspNetCore.Identity.UI

- Microsoft.EntityFrameworkCore.SqlServer

- Microsoft.EntityFrameworkCore.Tools

- Microsoft.AspNetCore.SignalR.Client or Microsoft.AspNetCore.SignalR

- QuestPDF

- iText
