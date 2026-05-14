# Welcome to My Basecamp 1
***

## Task
This project is a follow-up to MyBaseCamp1. 
The goal was to extend the existing
project management application with new collaboration features:
 file attachments,
discussion threads, and messages. 
The challenge was integrating these features
cleanly into the existing structure while keeping role-based access control intact.
## Description
Built on top of MyBaseCamp1 using ASP.NET Core MVC, Entity Framework Core, and SQL Server.

New features added in MyBaseCamp2:
- **Attachment** - Any project member can upload and delete attachments (png, jpg, pdf, txt)
- **Thread** - Only the project admin can create, edit, and delete discussion threads
- **Message** - Any project member can post, edit, and delete messages inside a thread

Technologies used:
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- C#
## Installation
Clone the repository:
```bash
git clone https://git.us.qwasar.io/my_basecamp_2/my_basecamp_2.git
```
Make sure the following are installed:
- .NET 8 SDK
- SQL Server
- Entity Framework Core Tools

Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=Basecamp2;Trusted_Connection=True;"
}
```

Apply migrations and create the database:
```bash
dotnet ef database update
```

Run the project:
```bash
dotnet run
```
## Usage
Then open `https://localhost:xxxx` in your browser.

**Important:** The first user to register automatically becomes admin.
Register your account first, then log in with admin privileges.

**Step by step:**

1. **Register & Login**
   - Go to `/Account/Register` and create your account
   - The first registered user automatically becomes admin
   - Log in at `/Account/Login`

2. **Projects**
   - After login you will see the Dashboard
   - Click `Projects` from the navigation menu
   - Click `New Project` to create a project
   - Click `View` on any project to open the project details page

3. **Attachments** (inside project details)
   - Any project member can upload files (png, jpg, pdf, txt)
   - Click `Choose File`, select your file and click `Upload`
   - Uploaded files are listed with their format badge
   - Click `Download` to view the file or `Delete` to remove it

4. **Discussion Threads** (inside project details)
   - Only the project admin can create threads
   - Click `+ New Thread`, enter a title and click `Create`
   - Click `Open` to enter a thread
   - Edit or delete a thread using the `Edit` / `Delete` buttons

5. **Messages** (inside a thread)
   - Any project member can post a message
   - Type your message in the text area and click `Send`
   - You can edit or delete your own messages

6. **Admin Panel**
   - Admin users can access `/Admin/Users`
   - From there you can promote users to admin or remove admin rights

### The Core Team


<span><i>Made at <a href='https://qwasar.io'>Qwasar SV -- Software Engineering School</a></i></span>
<span><img alt='Qwasar SV -- Software Engineering School's Logo' src='https://storage.googleapis.com/qwasar-public/qwasar-logo_50x50.png' width='20px' /></span>
