# **COOKBOOK API**

This repository contains a .NET 8 Web API project that uses a SQLite file-based database stored locally inside the repository.

---

## 1. GENERAL INFO
- **Framework:** .NET 9.0
- **ORM:** EF Core 9.0.9
- **Database:** SQLite (file-based, committed once and then ignored)
- **Architecture:** Clean architecture-style layering (API + Application + Infrastructure)

---

## 2. DATABASE FILE

The SQLite database file is located here:

```
sqlite/CookBook.sqlite
```

This file is **versioned once** (so the repo remains runnable immediately after cloning),  
but **ignored for future commits** so local changes do not pollute the repo.

After cloning, if you don’t want Git to treat changes to this file as modifications, run:

```bash
git update-index --assume-unchanged sqlite/CookBook.sqlite
```

If you ever need to start tracking it again:

```bash
git update-index --no-assume-unchanged sqlite/CookBook.sqlite
```

---

## 3. HOW THE DB IS TIED TO THE CODE

The connection string is stored in `appsettings.json`:

```json
"ConnectionStrings": {
  "DbConnectionString": "sqlite/CookBook.sqlite"
}
```

And configured in `Program.cs` like this:

```csharp
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var relativePath = builder.Configuration.GetConnectionString("DbConnectionString")
                       ?? throw new InvalidOperationException("DbConnectionString is missing");

    var absolutePath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, relativePath));

    options.UseSqlite($"Data Source={absolutePath}");

    options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
});
```

This makes the DB path **relative to the project root**.  
As long as `sqlite/CookBook.sqlite` exists, EF will connect successfully.

---

## 4. ACCESSING THE DB

### ✅ Via Visual Studio

1. Open **View → SQL Server Object Explorer**
2. Right-click **SQLite** (or "Add connection")
3. Browse to `sqlite/CookBook.sqlite`
4. Open tables, run queries, etc.

### ✅ Via JetBrains Rider

1. `View → Tool Windows → Database`
2. Click ➕ → SQLite
3. Select: `sqlite/CookBook.sqlite`
4. Rider will introspect tables and allow browsing & editing.

---

## 5. REPO CLONE INSTRUCTIONS

```bash
git clone https://github.com/drago1979/CookBook.git
cd CookBook
```

Run this to avoid accidentally committing local DB modifications:

```bash
git update-index --assume-unchanged sqlite/CookBook.sqlite
```

Run migrations and seeders (seeders contain Categories) from project directory, eg:

```bash
C:\Projects\CookBook\Khaoticen.CookBook.Api>dotnet ef database update
```

Then just run the API:

```bash
dotnet run --project Khaoticen.CookBook.Api
```

---

## 6. NOTES

- The committed DB is **minimal/empty**, only schema + seed if applicable.
- Devs can modify their local version without affecting repo history.
- If schema updates happen, a *migration script or empty template DB* should be provided separately in future.

---
