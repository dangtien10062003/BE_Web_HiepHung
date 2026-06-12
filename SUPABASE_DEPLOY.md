# Supabase deploy notes

This API cannot be hosted directly on Supabase because it is an ASP.NET Core
service. Supabase is used as the PostgreSQL database.

## Database

Set the production connection string as an environment variable:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=aws-...pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.PROJECT_REF;Password=YOUR_DB_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
dotnet ef database update
```

The generated SQL script is also available at:

```text
supabase-schema.sql
```

It can be pasted into the Supabase SQL Editor if direct database access is not
available from the machine.

## API hosting

Host the ASP.NET Core API on a .NET host such as a VPS, Render, Railway, Azure,
or Fly.io. Configure the same `ConnectionStrings__DefaultConnection` variable
on that host.
