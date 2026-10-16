# AID HOPE + Management System
### .NET MAUI (Windows Desktop + optional Mobile) + Azure SQL

---

## IMPORTANT – Fix the errors you saw

The previous version targeted Android / iOS / MacCatalyst which require extra workloads.
This updated version is **Windows-first** so it builds cleanly on most machines.

### 1. Open the solution
Double-click **AidHopePlus.sln**

### 2. Restore & Build
In Visual Studio:
- Right-click the solution → **Restore NuGet Packages**
- Then **Build → Rebuild Solution**

### 3. Set your Azure SQL connection string
Open `MauiProgram.cs` and replace the connection string.

### 4. Create the database
Open **Package Manager Console** and run:

```powershell
Add-Migration InitialCreate
Update-Database
```

(If you already ran migrations before, use:)
```powershell
Add-Migration AddAttendance
Update-Database
```

### 5. Run
Select **Windows Machine** at the top → Press **F5**

Login:
- Username: `staff`
- Password: `staff123`

---

## If you still get workload errors

Open **Visual Studio Installer** → Modify → make sure these are installed:

- .NET Multi-platform App UI development
- Windows App SDK C# Templates
- .NET 8 / .NET 9 SDK

Then restart Visual Studio.

---

## Optional: Enable Android later

If you want Android support later:

1. Install the Android workload:
   ```
   dotnet workload install maui-android
   ```
2. In `AidHopePlus.csproj`, uncomment the Android target line.
3. Rebuild.

---

## Features

- Side menu (Dashboard, About, Support, Beneficiaries, Awareness, Attendance, Reports)
- Full CRUD for Beneficiaries, Support Sessions, Awareness Events, Attendance
- Live Dashboard + Reports
- Azure SQL shared database
