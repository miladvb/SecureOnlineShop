# SecureOnlineShop

**SecureOnlineShop (SOSH)** is a full‑stack e‑commerce web application built with **ASP.NET Core 8 (MVC)** and **Entity Framework Core**.  
It demonstrates how to combine modern .NET features—such as minimal hosting, Identity, and EF Core Migrations—with clean, layered architecture to deliver a secure, maintainable online‑shop experience.

> **Why “Secure”?**  
> The project applies defence‑in‑depth patterns (ASP.NET Identity, `UserManager` / `RoleManager`, anti‑XSS helpers, global exception handling, HTTPS redirection, data‑annotation validation, etc.) so that you can use it as a starting point for production‑grade stores or as a learning resource for secure web development in .NET.

---

## Key Features

| Area | Highlights |
|------|------------|
| **Authentication & Authorisation** | ASP.NET Identity (register / login / reset‑password), role‑based access (Admin vs Customer), and optional 2‑factor providers. |
| **Product Catalogue** | CRUD for products, categories, gallery images, inventory status and prices. |
| **Shopping Cart & Checkout** | Session‑based cart, order summary, quantity editing, subtotal / tax calculation, and order persistence. |
| **Admin Dashboard** | Separate area with category, product and user‑manager controllers, data‑tables, search & pagination. |
| **Reviews & Comments** | Customers can leave comments / ratings; moderators approve or delete. |
| **Contact Us** | Contact form persists messages for later CRM processing. |
| **Responsive UI** | Razor Views + Bootstrap 5 with simple theming; works on mobile and desktop. |
| **Data Access** | EF Core 8 code‑first models, SQL Server provider by default (InMemory provider for tests). |
| **Dev Experience** | `dotnet watch`, seeded sample data, Docker Compose for MSSQL, ready‑to‑use `launchSettings.json`. |

---

## Tech Stack

- **Backend:** ASP.NET Core 8 MVC, C# 12  
- **Database:** SQL Server (LocalDB) · EF Core 8 (Code‑first)  
- **Security:** ASP.NET Identity, DataAnnotations, Anti‑Forgery tokens  
- **Front‑End:** Razor Views, Bootstrap 5, Vanilla JS  
- **Tooling:** .NET SDK 8, EF Core CLI, Docker Compose (optional), Visual Studio 2022 / VS Code  

---

## Getting Started

### Prerequisites
```bash
# .NET 8 SDK
winget install Microsoft.DotNet.SDK.8
# Or use Homebrew / apt:
brew install --cask dotnet-sdk
```

### Clone & Run

```bash
git clone https://github.com/<your‑user>/SecureOnlineShop.git
cd SecureOnlineShop/SOSH.Web

# create & migrate database
dotnet ef database update

# run the web app
dotnet run
```

Open <https://localhost:5001> in your browser. The first account you register is promoted to **Admin** automatically.

> **Using Docker**  
> ```
> docker compose up -d mssql
> dotnet ef database update
> dotnet run
> ```

---

## Project Structure

```
SecureOnlineShop/
├─ SOSH.sln               # Visual Studio solution
└─ SOSH.Web/              # ASP.NET Core MVC site
   ├─ Program.cs
   ├─ Controllers/
   ├─ Models/
   ├─ Views/
   ├─ Data/               # DbContext & seed
   ├─ Migrations/
   └─ wwwroot/            # Static assets (CSS, JS, images)
```

---

## Roadmap

- [ ] Stripe / PayPal payment integration  
- [ ] Email confirmation & password reset  
- [ ] Unit & Integration test suite (xUnit)  
- [ ] CI/CD‑ready GitHub Actions workflow  
- [ ] Container‑first deployment (Dockerfile, Kubernetes manifest)  

---

## Contributing

1. Fork the repo.
2. Create your feature branch (`git checkout -b feat/amazing‑feature`).
3. Commit your changes (`git commit -m 'feat: add amazing feature'`).
4. Push to the branch (`git push origin feat/amazing‑feature`).
5. Open a Pull Request.

All code should follow the [Microsoft C# style guide](https://learn.microsoft.com/dotnet/csharp) and pass `dotnet build` without warnings.

---

## License

Distributed under the **MIT License**. See `LICENSE` for more information.

---


