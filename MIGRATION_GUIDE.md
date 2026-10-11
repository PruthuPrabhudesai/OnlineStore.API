# Entity Framework Core Migration Guide

## Overview
This guide provides step-by-step instructions for replacing existing migration files with new changes in the OnlineStore.API project using Entity Framework Core (.NET 8).

You have two DbContexts:
- **StoreDbContext** - Main store database (Migrations/ folder)
- **StoreAuthDbContext** - Authentication database (Migrations/StoreAuthDb/ folder)

---

## Prerequisites
- Visual Studio 2026 (or later) with .NET 8 SDK installed
- Package Manager Console or .NET CLI available
- SSMS database already created

---

## Option 1: Using Package Manager Console (Recommended for Visual Studio)

### Step 1: Open Package Manager Console
1. In Visual Studio, go to **Tools → NuGet Package Manager → Package Manager Console**
2. Ensure the **Default project** is set to `OnlineStore.API`

### Step 2: Remove Existing Migrations

#### For StoreAuthDbContext (Authentication DB):
```powershell
Remove-Migration -Context StoreAuthDbContext
Remove-Migration -Context StoreAuthDbContext
```
> Run the command twice if there are multiple migrations. Check until you see: "No migrations have been applied to the database 'OnlineStore_Auth'."

#### For StoreDbContext (Main Store DB):
```powershell
Remove-Migration -Context StoreDbContext
Remove-Migration -Context StoreDbContext
```

### Step 3: Create New Migrations

#### For StoreAuthDbContext:
```powershell
Add-Migration -Context StoreAuthDbContext InitialCreate
```

#### For StoreDbContext:
```powershell
Add-Migration -Context StoreDbContext InitialCreate
```

### Step 4: Update the Database

#### For StoreAuthDbContext:
```powershell
Update-Database -Context StoreAuthDbContext
```

#### For StoreDbContext:
```powershell
Update-Database -Context StoreDbContext
```

---

## Option 2: Using .NET CLI (Command Line)

### Step 1: Open PowerShell/Terminal
Navigate to your project root:
```powershell
cd C:\Study\C#\My Projects\Main Projects\Online Store\OnlineStore.API\
```

### Step 2: Remove Existing Migrations

#### For StoreAuthDbContext:
```powershell
dotnet ef migrations remove --context StoreAuthDbContext
dotnet ef migrations remove --context StoreAuthDbContext
```

#### For StoreDbContext:
```powershell
dotnet ef migrations remove --context StoreDbContext
dotnet ef migrations remove --context StoreDbContext
```

### Step 3: Create New Migrations

#### For StoreAuthDbContext:
```powershell
dotnet ef migrations add InitialCreate --context StoreAuthDbContext --output-dir Migrations/StoreAuthDb
```

#### For StoreDbContext:
```powershell
dotnet ef migrations add InitialCreate --context StoreDbContext --output-dir Migrations
```

### Step 4: Update the Database

#### For StoreAuthDbContext:
```powershell
dotnet ef database update --context StoreAuthDbContext
```

#### For StoreDbContext:
```powershell
dotnet ef database update --context StoreDbContext
```

---

## Troubleshooting

### Issue: "No DbContext found"
**Solution:** Make sure you're specifying the correct context name with `-Context` parameter.

### Issue: "Migrations directory not found"
**Solution:** The directory is created automatically when you run `Add-Migration`, no action needed.

### Issue: "An entity of type 'IdentityRole' is being tracked"
**Solution:** This is normal. Clear the migration history and rebuild:
1. Delete all migrations except the snapshot
2. Run `Add-Migration` to create a fresh migration
3. Run `Update-Database`

### Issue: "The database already has tables"
**Solution:** Your SSMS database already has tables. Either:
- **Option A:** Drop the existing database in SSMS and let migrations recreate it
- **Option B:** Manually sync the database schema with the migrations

---

## What Gets Deleted & Recreated

When following this guide:
- ✅ All migration files in `Migrations/` and `Migrations/StoreAuthDb/` are removed
- ✅ ModelSnapshot.cs files are regenerated
- ✅ New migration files are created with your current DbContext configuration
- ✅ Database tables are recreated (if you run `Update-Database`)

---

## Database Seed Data

Your `StoreAuthDbContext` includes seed data for roles:
- **Customer** role (ID: a71a55d6-99d7-4123-b4e0-1218ecb90e3e)
- **Admin** role (ID: c309fa92-2123-47be-b397-a1c77adb502c)

This seed data will be automatically applied when you run `Update-Database`.

---

## Verify Migration Success

After running migrations, verify in SSMS:
1. Connect to your database
2. Check that tables exist:
   - `AspNetRoles`, `AspNetUsers`, `AspNetUserRoles`, etc. (from StoreAuthDbContext)
   - Your store-specific tables (from StoreDbContext)
3. Verify seed data in `AspNetRoles` table

---

## Quick Reference Commands

| Task | PMC Command | CLI Command |
|------|------------|------------|
| Remove last migration | `Remove-Migration -Context StoreAuthDbContext` | `dotnet ef migrations remove --context StoreAuthDbContext` |
| Create migration | `Add-Migration InitialCreate -Context StoreAuthDbContext` | `dotnet ef migrations add InitialCreate --context StoreAuthDbContext` |
| Update database | `Update-Database -Context StoreAuthDbContext` | `dotnet ef database update --context StoreAuthDbContext` |
| View migrations | `Get-Migration -Context StoreAuthDbContext` | `dotnet ef migrations list --context StoreAuthDbContext` |

---

## Next Steps (After Migrations Complete)
1. ✅ Verify database tables are created in SSMS
2. ✅ Confirm seed data is populated
3. ✅ Update connection strings in `appsettings.json` if needed
4. ✅ Test application functionality

---

## Additional Resources
- [EF Core Migrations Documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Identity in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity/)
