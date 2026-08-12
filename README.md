# TradeSphere B2B

### Enterprise dealer commerce & order management

TradeSphere is a B2B portal for wholesale brands and their dealers. Dealers browse the catalog, build a session cart, complete checkout through a simulated POS, and download PDF invoices. Admins manage products, receive live order alerts, and monitor a corporate dashboard.

Built to demonstrate **production-grade ASP.NET Core MVC**: layered architecture, cookie auth, real-time messaging, and clean service boundaries — not a tutorial dump.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/mvc)
[![EF Core](https://img.shields.io/badge/EF%20Core-SQLite-2C8EBB?logo=sqlite&logoColor=white)](https://learn.microsoft.com/ef/core)
[![SignalR](https://img.shields.io/badge/SignalR-Realtime-8A2BE2)](https://learn.microsoft.com/aspnet/core/signalr)
[![QuestPDF](https://img.shields.io/badge/QuestPDF-Invoices-1E3A5F)](https://www.questpdf.com/)
[![Serilog](https://img.shields.io/badge/Serilog-Logging-FF4F00)](https://serilog.net/)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## Architecture

The app is **ASP.NET Core MVC** with an **N-Tier** split that stays close to **Clean Architecture**: controllers stay thin, business rules live in services, persistence is behind repositories, and all of it is wired with **Dependency Injection**.

```
Views (Razor + Bootstrap 5)
        │
Controllers  —  [Authorize]  ·  ViewModels  ·  no business logic
        │  constructor injection
Application services
  IAuthService · IProductService · ICartService
  IOrderService · IPaymentService · IInvoiceService
        │
IGenericRepository<T>  →  GenericRepository<T>
        │
AppDbContext  (EF Core + SQLite)
```

| Principle | How it shows up in the codebase |
|---|---|
| **Repository Pattern** | `IGenericRepository<T>` for CRUD and `FindAsync`; `DbContext` never leaks into controllers |
| **Dependency Injection** | Registrations live in `Extensions/ServiceRegistration.cs` — skinny `Program.cs` |
| **Loose coupling** | Controllers depend on interfaces (`IOrderService`, `IPaymentService`, …), not concrete types |
| **ViewModels** | Domain entities stay out of views; server-side validation via Data Annotations |
| **Single responsibility** | Payment, stock deduction, PDF generation, and logging each have a dedicated service |

---

## Tech Stack

| Layer | Stack |
|---|---|
| Runtime | ASP.NET Core MVC on **.NET 10** |
| Data | Entity Framework Core + **SQLite** (Mac-friendly; swap the provider for SQL Server later) |
| Auth | Cookie authentication, Claims, `[Authorize(Roles = "Admin")]` |
| Realtime | **SignalR** hub at `/orderHub` |
| Documents | **QuestPDF** (Community license) |
| Logging | **Serilog** rolling files → `Logs/log-.txt` |
| Frontend | **Bootstrap 5**, Bootstrap Icons, **SweetAlert2**, **Chart.js** |

---

## Key Features

- **Role-based access** — Admins own product CRUD; dealers see catalog, cart, and their own orders.
- **Dealer login & self-registration** — Email or tax number; live password rules (length, case, digit, special char) aligned with server validation.
- **Session shopping cart** — Complex objects stored as JSON in session; navbar badge via a ViewComponent.
- **Dummy payment (virtual POS)** — Checkout card form; numbers starting with `0000` are declined, others succeed.
- **Order pipeline** — Transactional create (header + lines), stock decrement, order history.
- **PDF invoices** — QuestPDF layout; download is restricted to the order owner.
- **Live admin toasts** — New orders broadcast over SignalR and shown with SweetAlert2 (3s, top-right).
- **Dashboard** — KPI cards and a Chart.js monthly sales chart.
- **Global exception handling** — `UseExceptionHandler` + Serilog `LogError` with request path.

---

## Getting Started

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) on macOS, Linux, or Windows.

```bash
git clone https://github.com/pinareray/DotNet-B2B-tradesphere.git
cd DotNet-B2B-tradesphere

dotnet restore
dotnet ef database update
dotnet run
```

The app listens on `http://localhost:5026` by default (see `Properties/launchSettings.json`).

If `dotnet ef` is missing:

```bash
dotnet tool install --global dotnet-ef
```

### Demo accounts

| Role | Login | Password |
|---|---|---|
| Admin | `admin@tradesphere.com` | `Admin123!` |
| Dealer | `abc@tradesphere.com` or tax no `1234567890` | `Dealer123!` |

**Payment sandbox:** success → `4111111111111111` · decline → any card starting with `0000`.

---

## Repository layout

```
Controllers/     Auth, Product, Cart, Order, Home
Services/        Business logic (interfaces + implementations)
Repositories/    Generic EF Core repository
Models/          Domain (BaseEntity, Dealer, Product, Order, OrderItem)
ViewModels/      UI contracts + validation
Views/           Razor
Data/            AppDbContext, migrations
Hubs/            OrderHub (SignalR)
Extensions/      DI, session JSON helpers, auth seeder
Logs/            Serilog output (gitignored)
```

---

## License

MIT — see [LICENSE](LICENSE).
