using Database_Customer;
using System;
using System.Linq;

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
