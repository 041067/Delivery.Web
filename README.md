# 🍔 Delivery.Web — End-to-End System Testing with ASP.NET Core + Playwright

> **A portfolio project focused on Software Quality, Automated System Testing and real-world user journeys in ASP.NET Core MVC.**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![xUnit](https://img.shields.io/badge/xUnit-2.9.3-5F5F5F)](https://xunit.net/)
[![Playwright](https://img.shields.io/badge/Playwright-1.63.0-2EAD33?logo=playwright&logoColor=white)](https://playwright.dev/)
[![C#](https://img.shields.io/badge/C%23-12+-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)

---

## 🎯 About the Project

**Delivery.Web** is a lightweight food-delivery web application built to demonstrate how **system testing** validates an application from the perspective of a real user.

Instead of testing isolated methods, the test suite opens a real browser and validates complete journeys through the application:

**Login → Restaurant → Menu → Payment → Order Confirmation → Tracking**

The project is intentionally small, but the testing approach is designed around practices used in professional software delivery:

- End-to-end browser automation
- Functional system testing
- Usability validation
- Performance smoke testing
- Automated application startup and shutdown
- Reproducible test execution with xUnit + Playwright

---

## 💼 Why This Project Matters

This repository demonstrates more than CRUD development.

It shows the ability to think about software as a **complete system**, connecting:

**Application behavior + business rules + user experience + automated validation**

A recruiter or engineering manager can quickly see practical experience with:

> **C# • ASP.NET Core MVC • Test Automation • xUnit • Playwright • End-to-End Testing • Software Quality • CI/CD-ready testing mindset**

---

## 🧩 Application Scenario

The application simulates a simplified food-delivery platform.

### Main user journey

1. User accesses the login page
2. User authenticates
3. User selects a restaurant
4. User views the menu
5. User selects a payment method
6. User confirms the order
7. User is redirected to order tracking
8. System displays the order status

### Business rules covered

- Authentication is required before ordering
- An order cannot be created without a payment method
- An empty cart cannot be submitted
- Invalid products are rejected
- A confirmed order receives a tracking view

---

## 🧪 System Testing Strategy

The project contains a dedicated test project:

**Delivery.SystemTests**

The tests use **Playwright + xUnit** to interact with the application through a real browser.

### Test scenarios

| Scenario | Type | Validation |
|---|---|---|
| Successful login | Functional | User can authenticate |
| Complete order | Functional / E2E | Full journey works end-to-end |
| Order tracking | Functional | Confirmed order appears correctly |
| Confirm button on mobile viewport | Usability | Critical action remains visible |
| Menu response time | Performance smoke | Page loads within defined threshold |

### Current automated coverage

- ✅ Login flow
- ✅ End-to-end order flow
- ✅ Order confirmation
- ✅ Order tracking
- ✅ Mobile viewport usability
- ✅ Basic response-time check

> **Important:** the performance test in this repository is a **smoke check**, not a full load/stress test. Simulating thousands of concurrent users should be handled with dedicated tools such as k6, JMeter or NBomber.

---

## 🏗️ Architecture

The solution is organized into two main projects:

~~~text
Delivery.Web
│
├── Controllers
│   ├── AccountController.cs
│   ├── HomeController.cs
│   └── OrderController.cs
│
├── Models
│   ├── User.cs
│   ├── Restaurant.cs
│   ├── Product.cs
│   ├── CartItem.cs
│   └── Order.cs
│
├── Service
│   └── DeliveryService.cs
│
├── Views
│   ├── Account
│   ├── Home
│   ├── Order
│   └── Shared
│
└── Program.cs

Delivery.SystemTests
└── DeliveryTests.cs
~~~

### Application layer

**ASP.NET Core MVC** handles the web application and user interaction.

### Service layer

**DeliveryService** contains the main application operations:

- Authentication
- Restaurant retrieval
- Menu retrieval
- Order creation
- Order retrieval

For this educational/project showcase, application data is kept **in memory**.

### System-test layer

**DeliveryTests.cs**:

- Starts the ASP.NET Core application automatically
- Waits until the application is available
- Launches Chromium through Playwright
- Executes browser-based scenarios
- Validates real rendered pages
- Shuts the application down after the tests

This makes the test execution reproducible without requiring the application to be started manually.

---

## 🧠 Unit vs Integration vs System Testing

One of the main goals of this repository is to demonstrate the distinction between testing levels.

| Test level | Focus | Example |
|---|---|---|
| **Unit** | Isolated logic | Validate a calculation or method |
| **Integration** | Components working together | Service + database/API |
| **System** | Complete application behavior | Login → order → tracking |

This repository focuses on the **third level: System Testing**.

The test does not simply call a C# method. It behaves like a real user interacting with the web application.

---

## 🚀 Technologies

### Backend

- **C#**
- **.NET 10**
- **ASP.NET Core MVC**
- Dependency Injection
- HTTP/HTTPS
- Session state

### Testing

- **xUnit**
- **Microsoft Playwright**
- Chromium browser automation
- End-to-end assertions
- Viewport-based usability validation
- Response-time assertions

### Engineering Concepts

- System / End-to-End Testing
- Test automation
- Business-rule validation
- User journey validation
- Test lifecycle management
- Reproducible automated execution
- Software Quality mindset

---

## ▶️ Getting Started

### Prerequisites

Install:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PowerShell 7
- Chromium browser through Playwright

### 1. Clone the repository

~~~powershell
git clone https://github.com/041067/Delivery.Web.git
cd Delivery.Web
~~~

### 2. Restore and build

~~~powershell
dotnet restore
dotnet build
~~~

### 3. Install Playwright browsers

After the build:

~~~powershell
pwsh Delivery.SystemTests/bin/Debug/net10.0/playwright.ps1 install
~~~

### 4. Run the system tests

~~~powershell
dotnet test
~~~

The test suite starts the web application automatically, executes the browser scenarios and terminates the application when finished.

---

## 🔐 Test Environment Notes

The current automated tests execute the application at:

**https://localhost:7194**

The test project starts the web application automatically with:

~~~text
dotnet run --no-launch-profile --urls=https://localhost:7194
~~~

The Playwright browser context is configured to ignore the local development HTTPS certificate, which is appropriate for this local test environment.

The current test harness uses the application working directory:

**C:\Delivery.Web\Delivery.Web**

For a different local path, update the **WorkingDirectory** value in:

**Delivery.SystemTests/DeliveryTests.cs**

---

## 📊 What a Successful Test Run Proves

A successful execution validates that the application can:

~~~text
Application starts
       ↓
Browser opens
       ↓
User logs in
       ↓
Restaurant is displayed
       ↓
Menu is accessible
       ↓
Payment is selected
       ↓
Order is created
       ↓
Tracking page is displayed
       ↓
Order status = Confirmado
~~~

This is the key difference between checking code correctness and checking whether the **system actually works for the user**.

---

## 🛠️ Engineering Decisions

### Why Playwright?

Playwright is a strong fit for this project because it allows the tests to operate at the browser level, validating the same interface used by the final user.

### Why xUnit?

xUnit provides a clean test structure and integrates naturally with the modern .NET testing ecosystem.

### Why start the application inside the test?

It reduces manual setup and turns the system test suite into a more reproducible execution unit:

~~~text
dotnet test
   ↓
Start application
   ↓
Wait for availability
   ↓
Run browser tests
   ↓
Collect assertions
   ↓
Shutdown application
~~~

---

## 📌 Project Scope

This repository intentionally prioritizes **test strategy and system behavior** rather than building a production-scale delivery platform.

The current application uses:

- In-memory users
- In-memory restaurants/products
- In-memory orders
- Simplified payment selection
- Simplified tracking
- Development HTTPS

That keeps the project focused on demonstrating **how system tests are designed and automated**.

---

## 🔭 Evolution Roadmap

Potential next steps for a production-oriented version:

- [ ] PostgreSQL or SQL Server persistence
- [ ] ASP.NET Core Identity / JWT authentication
- [ ] Real payment gateway integration
- [ ] REST API layer
- [ ] Test data builders / fixtures
- [ ] Integration tests with WebApplicationFactory
- [ ] API contract testing
- [ ] API security testing
- [ ] k6 / NBomber load testing
- [ ] Docker / Docker Compose
- [ ] GitHub Actions CI pipeline
- [ ] Test reports and coverage
- [ ] Parallel browser execution
- [ ] Test environment configuration

---

## 👨‍💻 Portfolio Focus

This project is part of a broader portfolio focused on:

**Software Development • Quality Engineering • Test Automation • DevOps • Cloud**

The goal is not only to build applications, but to demonstrate the ability to **validate, automate and improve software delivery**.

---

## ⭐ Key Takeaways for Recruiters

**What this repository demonstrates:**

> **I don't only build features — I validate complete user journeys.**

> **I understand the difference between unit, integration and system testing.**

> **I can automate browser-based end-to-end scenarios in C#/.NET.**

> **I consider functional behavior, usability and performance during validation.**

> **I structure testing as part of the software delivery process.**

---

## 📄 License

This project is available for educational and portfolio purposes.

---

### 🔗 Repository

**GitHub:** https://github.com/041067/Delivery.Web
