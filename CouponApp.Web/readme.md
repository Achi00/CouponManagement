# Coupon Marketplace System API

RESTful backend for a coupon marketplace platform where merchants publish discounts and customers reserve or purchase coupons.  
Built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server** following **Clean Architecture** principles.

---

# Features

- Role-based system (Administrator, Merchant, Customer)
- Coupon offer management and moderation workflow
- Reservation and purchase system
- Coupon availability and expiration handling
- Category-based browsing and search
- Background jobs for reservation cleanup and offer expiration
- Global system settings managed by admin
- Structured logging and error handling
- Swagger API documentation
- API versioning and health checks

---

# Tech Stack

- ASP.NET Core (MVC + Web API)
- Entity Framework Core
- SQL Server
- ASP.NET Identity / JWT Authentication
- Clean Architecture
- Repository Pattern
- Dependency Injection
- Mapster
- FluentValidation
- Swagger
- Background Worker Services

---

# System Roles

## Administrator
- Full system control
- Manage users and merchants
- Approve or reject merchant offers
- Configure global system settings
- Manage offer categories

## Merchant
- Create and manage discount offers
- View offer performance and sales history
- Edit offers within allowed time window
- Monitor active and expired offers

## Customer
- Browse approved offers
- Reserve coupons
- Purchase coupons
- Track purchased coupons and statuses

---

# Core Modules

## Merchant Panel
- Dashboard with statistics for active and expired offers
- Offer CRUD (title, description, category, image, price, discount price, quantity, expiration)
- Pending approval workflow
- Limited-time editing
- Sales history and coupon buyers

## Admin Panel
- User management (customers and merchants)
- Offer moderation (approve / reject with reason)
- Global system configuration
- Category management

## Customer Area
- Browse and search offers
- Coupon reservation system
- Coupon purchase simulation
- Personal coupon history and statuses

---

# Public Offer Details

Each offer includes:

- Full description and pricing
- Remaining coupon quantity
- Expiration information
- Reservation and purchase options for authenticated customers
- Automatic **"Sold Out"** state when coupons reach zero

---

# Background Services

## Reservation Cleanup
Automatically removes expired reservations if the purchase is not completed within the configured time.

## Offer Expiration
Updates offer status when the expiration date is reached so they no longer appear in active listings.

---