#  Bulletin Board App

Full‑stack bulletin‑board application built with SQL Server, ASP.NET Core Web API, and ASP.NET Core MVC. It lets users create, view, filter, update, and delete announcements under categories and subcategories.

---

##  Features

- **SQL Server database** with three tables: `Categories`, `SubCategories`, and `Announcements`.
- **Stored procedures** for all CRUD operations.
- **ASP.NET Core Web API** exposing endpoints to manage announcements and fetch category data.
- **Global error handling** via middleware producing clean `ProblemDetails` JSON.
- **ASP.NET Core MVC front‑end** with Razor views and Bootstrap for UI.
- Cascading dropdowns to pick a category → subcategory.
- Client‑side & server‑side validation in forms.
- Swagger UI enabled for easy API testing (in Development).

---

##  Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or later
- [Visual Studio](https://visualstudio.microsoft.com/) 2022+ (with **ASP.NET and web development** workload) or VS Code
- SQL Server (LocalDB or full edition)

---

##  Project Structure

```
BulletinBoardApp/               # Solution root
├─ Sql/                         # Database scripts & stored procs
│   ├─ SqlCommands.txt          # DDL + DML scripts
│   └─ v1.0 Rollback.txt        # Rollback scripts
├─ BulletinBoardApi/            # ASP.NET Core Web API (backend)
│   └─ BulletinBoardApi.csproj
└─ BulletinBoardWeb/            # ASP.NET Core MVC (frontend)
    └─ BulletinBoardWeb.csproj
```

---

##  Setup & Run

1. **Clone the repo**

   ```bash
   git clone https://github.com/BohdanZaloha/bulletin-board-app.git
   cd bulletin-board-app
   ```

2. **Initialize the database**

   - Open SQL Server Management Studio (or `sqlcmd`).
   - Run the scripts in `Sql/SqlCommands.txt` to create tables & procs.
   - (Optional) Run `Sql/v1.0 Rollback.txt` to drop objects.

3. **Configure connection strings**

   - In the API project, edit `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=BulletinBoard;Trusted_Connection=True;"
     }
     ```

4. **Launch the API**

   ```bash
   cd BulletinBoardApi
   dotnet restore
   dotnet run
   ```

   - Swagger UI: `https://localhost:5001/swagger`

5. **Launch the Web UI**

   ```bash
   cd ../BulletinBoardWeb
   dotnet restore
   dotnet run
   ```

   - Browse: `https://localhost:5002`

---

##  Usage

- **View announcements** on the homepage.
- **Filter** by category/subcategory using the dropdowns.
- **Add** a new announcement via the **Create** button.
- **Edit/Delete** existing announcements using action buttons.

---

##  API Endpoints

| Method | Route                        | Description                       |
| ------ | ---------------------------- | --------------------------------- |
| GET    | `/api/categories`            | List all categories               |
| GET    | `/api/subcategories/{catId}` | List subcategories for a category |
| GET    | `/api/announcements`         | Get all announcements             |
| GET    | `/api/announcements/{id}`    | Get a single announcement         |
| POST   | `/api/announcements`         | Create a new announcement         |
| PUT    | `/api/announcements/{id}`    | Update an announcement            |
| DELETE | `/api/announcements/{id}`    | Delete an announcement            |

