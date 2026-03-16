using Database_Customer;
using System;
using System.Linq;
/*
===============================================================================
DATABASE ACCESS IN C# USING LINQ TO SQL
===============================================================================
To run this code, you need to set up a local database and generate LINQ to SQL classes as described in the comments below.
===============================================================================
1. Create a Local Database File (.mdf)
   - Open Visual Studio and your project.
   - Open Server Explorer (View > Server Explorer).
   - Right-click "Data Connections" → "Add Connection..."
   - Choose "Microsoft SQL Server Database File (SqlClient)"
   - Specify a file path and name, e.g., "DBCustomer.mdf"
   - Click OK → the database appears under Data Connections.

2. Create the "Customer" Table
   Option A – Using Table Designer:
   - Expand your database → Tables
   - Right-click → Add New Table…
   - Define columns:
     * Id → int → Primary Key → Identity = True (auto-increment)
     * Name → nvarchar(50) → NOT NULL
     * CreatedDate → datetime → default GETDATE()
   - Save table as "Customer"

   Option B – Using SQL Query:
   
   CREATE TABLE Customer
   (
       Id INT PRIMARY KEY IDENTITY(1,1),
       Name NVARCHAR(50) NOT NULL,
       CreatedDate DATETIME DEFAULT GETDATE()
   )

3. Generate LINQ to SQL Classes (DataContext)
   - In Solution Explorer → Right-click project → Add > New Item…
   - Select "LINQ to SQL Classes (.dbml)" → name it e.g., "DBCustomerDBCustomer.dbml"
   - Open the .dbml designer → drag your "Customers" table from Server Explorer onto it
     - After dropping, Visual Studio asks to copy database file to project → choose "Yes"
     - This adds the .mdf file to your project and creates the necessary classes to interact with the database.
     - In properties of mdf file, set "Copy to Output Directory" to "Copy if newer". 
       - The default "Copy always" would overwrite the database every time you run the program, losing all data changes.
       - The "Copy if newer" option ensures that the database file is only copied if it has been modified, preserving your data across runs.s
   - After this, Visual Studio generates:
     - DBCustomer.dbml (XML definition of tables)
     - DBCustomer.designer.cs (C# classes mapping tables and columns)
     - DBCustomer.mdf (the pattern database file)s

4.Use the generated DataContext in your C# program
   - Create an instance: DBCustomerDataContext db = new DBCustomerDataContext();
   - Access the Customers table: db.Customers
   - Use InsertOnSubmit, DeleteOnSubmit, and SubmitChanges() for CRUD operations
   - Use LINQ queries like FirstOrDefault() and ToList() to fetch data
*/
namespace ConsoleCrudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the LINQ to SQL DataContext
            // This represents the database connection
            DBCustomerDataContext db = new DBCustomerDataContext();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMERS MENU ===");
                Console.WriteLine("1. List all customers");
                Console.WriteLine("2. Add a new customer");
                Console.WriteLine("3. Update a customer");
                Console.WriteLine("4. Delete a customer");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListCustomers(db);
                        break;
                    case "2":
                        AddCustomer(db);
                        break;
                    case "3":
                        UpdateCustomer(db);
                        break;
                    case "4":
                        DeleteCustomer(db);
                        break;
                    case "5":
                        return;  // Exit the program
                    default:
                        Console.WriteLine("Invalid option. Press Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ListCustomers(DBCustomerDataContext db)
        {
            var customers = db.Customers.ToList();

            Console.WriteLine("\nCustomer list:");
            foreach (var c in customers)
            {
                Console.WriteLine($"ID: {c.Id}, Name: {c.Name}, Created: {c.CreatedDate}");
            }

            Console.WriteLine("\nPress Enter to return to menu...");
            Console.ReadLine();
        }

        static void AddCustomer(DBCustomerDataContext db)
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine();

            var newCustomer = new Customer
            {
                Name = name,
                CreatedDate = DateTime.Now
            };

            db.Customers.InsertOnSubmit(newCustomer);
            db.SubmitChanges();

            Console.WriteLine("Customer added successfully. Press Enter to return to menu...");
            Console.ReadLine();
        }

        static void UpdateCustomer(DBCustomerDataContext db)
        {
            Console.Write("Enter the ID of the customer to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = db.Customers.FirstOrDefault(c => c.Id == id);
                if (customer != null)
                {
                    Console.Write($"New name ({customer.Name}): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                        customer.Name = newName;

                    db.SubmitChanges();
                    Console.WriteLine("Customer updated. Press Enter...");
                }
                else
                {
                    Console.WriteLine("Customer not found. Press Enter...");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Press Enter...");
            }
            Console.ReadLine();
        }

        static void DeleteCustomer(DBCustomerDataContext db)
        {
            Console.Write("Enter the ID of the customer to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = db.Customers.FirstOrDefault(c => c.Id == id);
                if (customer != null)
                {
                    db.Customers.DeleteOnSubmit(customer);
                    db.SubmitChanges();
                    Console.WriteLine("Customer deleted. Press Enter...");
                }
                else
                {
                    Console.WriteLine("Customer not found. Press Enter...");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Press Enter...");
            }
            Console.ReadLine();
        }
    }
}
