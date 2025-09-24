# CRM System - .NET 6 Solution

This solution contains a complete Customer Relationship Management (CRM) system built with .NET 6, featuring a Web API backend and a Razor Pages frontend.

## Projects

### APIDotnet6 (Web API)
- **Purpose**: RESTful API for CRM data management
- **Port**: https://localhost:7145
- **Features**:
  - Customer management (CRUD operations)
  - Contact tracking
  - Opportunity management
  - Search and filtering capabilities
  - In-memory database with sample data
  - Swagger/OpenAPI documentation

### RazorDotnet6 (Razor Pages Web Application)
- **Purpose**: Frontend web interface for the CRM system
- **Port**: https://localhost:7001
- **Features**:
  - Dashboard with key metrics
  - Customer management interface
  - Contact history tracking
  - Opportunity pipeline management
  - Responsive Bootstrap UI

## Getting Started

### Prerequisites
- .NET 6 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Start the API Project**:
   ```bash
   cd APIDotnet6
   dotnet run
   ```
   The API will be available at https://localhost:7145
   Swagger UI: https://localhost:7145/swagger

2. **Start the Razor Pages Project** (in a new terminal):
   ```bash
   cd RazorDotnet6
   dotnet run
   ```
   The web application will be available at https://localhost:7001

### Features Overview

#### Dashboard
- Customer count summary
- Open opportunities counter
- Recent contacts tracking
- Pipeline value calculation
- Quick action buttons

#### Customer Management
- Add, edit, and delete customers
- View detailed customer profiles
- Search customers by name, email, or company
- Filter by status (Active/Inactive)
- Track contact history and opportunities per customer

#### Contact Tracking
- Record customer interactions (phone, email, meetings)
- Track contact dates and outcomes
- Associate contacts with specific customers
- View contact history timeline

#### Opportunity Management
- Manage sales opportunities
- Track deal stages and probabilities
- Monitor expected close dates
- Calculate pipeline values
- Associate opportunities with customers

## Data Models

### Customer
- Personal information (name, email, phone)
- Company details
- Address information
- Status tracking
- Creation and modification dates
- Notes

### Contact
- Contact type (Phone, Email, Meeting, etc.)
- Subject and description
- Contact date and status
- Contacted by information
- Customer association

### Opportunity
- Title and description
- Estimated value and probability
- Sales stage tracking
- Expected close date
- Status (Open, Won, Lost)
- Customer association
- Assigned salesperson

## API Endpoints

### Customers
- `GET /api/customers` - List customers with filtering
- `GET /api/customers/{id}` - Get customer details
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### Contacts
- `GET /api/contacts` - List contacts with filtering
- `GET /api/contacts/{id}` - Get contact details
- `GET /api/contacts/customer/{customerId}` - Get customer contacts
- `POST /api/contacts` - Create new contact
- `PUT /api/contacts/{id}` - Update contact
- `DELETE /api/contacts/{id}` - Delete contact

### Opportunities
- `GET /api/opportunities` - List opportunities with filtering
- `GET /api/opportunities/{id}` - Get opportunity details
- `GET /api/opportunities/customer/{customerId}` - Get customer opportunities
- `GET /api/opportunities/stats` - Get opportunity statistics
- `POST /api/opportunities` - Create new opportunity
- `PUT /api/opportunities/{id}` - Update opportunity
- `DELETE /api/opportunities/{id}` - Delete opportunity

## Technology Stack

- **.NET 6**: Runtime and framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: Data access
- **In-Memory Database**: For development and testing
- **Bootstrap 5**: CSS framework
- **Bootstrap Icons**: Icon library
- **Razor Pages**: Server-side rendering
- **HttpClient**: API communication
- **Swagger/OpenAPI**: API documentation

## Development Notes

- The application uses an in-memory database that resets on each restart
- CORS is configured to allow communication between the Razor Pages app and API
- Sample data is automatically seeded when the API starts
- The Razor Pages application communicates with the API via HttpClient
- Error handling is implemented with graceful fallbacks
- Responsive design works on desktop and mobile devices

## Future Enhancements

- Implement persistent database (SQL Server, PostgreSQL)
- Add user authentication and authorization
- Implement real-time notifications
- Add email integration
- Create reporting and analytics features
- Add file attachment capabilities
- Implement advanced search and filtering
- Add data export functionality