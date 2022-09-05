### Pizza Management System

This small toy program implements a Pizza Management System, by which a user can request a new order, get the next order in line, or close the next order.

The system was implemented using .NET 6 and PostgreSQL.

You can find the requirements and a small usage guide in the following sections.

---

#### Requirements

The only requirements are **.NET 6** and **Docker** and **Docker Compose**.

Docker is only needed in case you don't have a local PostgreSQL daemon. In that case, just create a database and change the **PostgreConnectionString** in the appsettings.Development file accordingly.

The program bind on **port 4242** by default. You can change it in the appsettings.

---

#### Usage Guide 

1. `cd` into the Solution folder
2. `dotnet restore` to restore all NuGet dependencies
3. check appsettings.Development to see if the **PostgreConnectionString** and the **AspNetUrls** settings are fine for you
4. `dotnet run watch --project .\PizzaManagementSystem\` to run the program
5. go to **localhost:4242/swagger/index.html** to see Swagger's documentation page and make some requests

---

#### Endpoints Documentation

- **[POST] Order** 
  This endpoint lets you place a new order. You must provide an array of valid order items.
  Pizza types available at launch are as follows: 
  Margherita = 0
  Ortolana = 5
  Diavolina = 10
  Bufalina = 15
  Of course, you must provide a quantity greater than 0 for each order item.
  The endpoint returns a summary of the order, containing the computed Total for the order as well as the Order Id and the number of orders pending before this one.
- **[GET] Order/Next**
  This endpoint lets you retrieve the next order in line, along with its specific information
- **[DELETE] Order/CloseNext**
  This endpoint lets you close the next order. The order gets physically removed from the database for the sake of simplicity.