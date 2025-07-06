# ParseTheParcel

A modular shipping cost calculator built with **.NET 9**, following **Clean Architecture** principles.

## Project Structure

- **ParseTheParcel.Api** – ASP.NET Core Web API
- **ParseTheParcel.Application** – Business logic (parcel rules, services)
- **ParseTheParcel.Domain** – Core domain models and interfaces
- **ParseTheParcel.Infrastructure** – Infrastructure (e.g. cost rule provider)
- **ParseTheParcel.ConsoleApp** – CLI interface for manual inputs/testing
- **ParseTheParcel.Tests** – Unit tests for Application and Domain layers

## How to Run

### 1. Clone the repository
```bash
git clone https://github.com/Refactorahau/ParseTheParcel_Snehansu_Sahoo.git
cd ParseTheParcel_Snehansu_Sahoo
```

### 2. Build the solution
Open in **Visual Studio 2022+** or run:
```bash
dotnet build
```

### 3. Run the API
```bash
cd ParseTheParcel.Api
dotnet run
```

Swagger UI will be available at:
```
https://localhost:<port>/swagger
```

### 4. Run the Console App
```bash
cd ParseTheParcel.ConsoleApp
dotnet run
```

## Unit Testing

To run unit tests:
```bash
cd ParseTheParcel.Tests
dotnet test
```

## Technologies Used

- .NET 9
- ASP.NET Core Web API
- xUnit, FluentAssertions
- Clean Architecture

---

## Example Parcel Request

```json
POST /api/parcel/calculate

{
  "length": 100,
  "breadth": 30,
  "height": 20,
  "weight": 2.5
}
```

## License

This project is for demo purposes.
