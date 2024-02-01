using Spectre.Console;
using Infrastructure.Services;
using Infrastructure.Entities;
using System.Threading.Tasks;

namespace Console.UI
{
    public class ConsoleUI
    {
        private readonly CustomerService _customerService;
        private readonly InventoryService _inventoryService;
        private readonly OrderService _orderService;

        public ConsoleUI(CustomerService customerService, InventoryService inventoryService, OrderService orderService)
        {
            _customerService = customerService;
            _inventoryService = inventoryService;
            _orderService = orderService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Welcome to the Inventory Management System")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Manage Customers",
                            "Manage Inventory",
                            "Manage Orders",
                            "Exit" }));

                switch (choice)
                {
                    case "Manage Customers":
                        await ManageCustomersAsync();
                        break;
                    case "Manage Inventory":
                        await ManageInventoryAsync();
                        break;
                    case "Manage Orders":
                        await ManageOrdersAsync();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        //IF SELECT MANAGE CUSTOMERS 
        private async Task ManageCustomersAsync()
        {
            var keepManagingCustomers = true;
            while (keepManagingCustomers)
            {
                var customerAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Customer Management")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "List Customers",
                            "Add Customer",
                            "Update Customer",
                            "Delete Customer",
                            "Return to Main Menu" }));

                switch (customerAction)
                {
                    case "List Customers":
                        await ListCustomersAsync();
                        break;
                    case "Add Customer":
                        await AddCustomerAsync();
                        break;
                    case "Update Customer":
                        await UpdateCustomerAsync();
                        break;
                    case "Delete Customer":
                        // Implementation needed
                        break;
                    case "Return to Main Menu":
                        keepManagingCustomers = false;
                        break;
                }
            }
        }
        //LIST ALL CUSTOMERS 
        private async Task ListCustomersAsync()
        {
            // Retrieve the list of customers using the CustomerService
            var customers = await _customerService.GetAllCustomersAsync(); 

            // Clear the console and create a new table
            AnsiConsole.Clear();
            var table = new Table();

            table.AddColumn("ID");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");
            table.AddColumn("Email");
            table.AddColumn("Address");
            table.AddColumn("Phone");

            foreach (var customer in customers)
            {
                table.AddRow(
                    customer.CustomerId.ToString(),
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    $"{customer.StreetName}, {customer.City}, {customer.PostalCode}, {customer.Country}",
                    customer.Phone ?? "N/A" // Use "N/A" if the phone number is null
                );
            }
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to return to the menu...");
            System.Console.ReadKey();
        }
        //ADD A CUSTOMER
        private async Task AddCustomerAsync()
        {
            var firstName = AnsiConsole.Ask<string>("Enter the customer's first name:");
            var lastName = AnsiConsole.Ask<string>("Enter the customer's last name:");
            var email = AnsiConsole.Ask<string>("Enter the customer's email:");

            var customer = new CustomerEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,

            };

            var result = await _customerService.CreateCustomerAsync(customer);
            if (result != null)
            {
                AnsiConsole.MarkupLine("[green]Customer created successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to create customer.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }

        // have to implement UpdateCustomerAsync, DeleteCustomerAsync etc.

        private async Task UpdateCustomerAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            AnsiConsole.Clear();

            var customerDict = customers.ToDictionary(c => c.CustomerId.ToString(), c => $"{c.FirstName} {c.LastName}");
            var customerId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a customer to update:")
                .PageSize(10)
                .AddChoices(customerDict.Keys));

            var selectedCustomer = customers.First(c => c.CustomerId.ToString() == customerId);

            //prompt for new details 
            var firstName = AnsiConsole.Ask<string>($"Enter new first name (Current: {selectedCustomer.FirstName}):", selectedCustomer.FirstName);
            var lastName = AnsiConsole.Ask<string>($"Enter new last name (Current: {selectedCustomer.LastName}):", selectedCustomer.LastName);
            var email = AnsiConsole.Ask<string>($"Enter new email (Current: {selectedCustomer.Email}):", selectedCustomer.Email);

            selectedCustomer.FirstName = firstName;
            selectedCustomer.LastName = lastName;
            selectedCustomer.Email = email;

            var success = await _customerService.UpdateCustomerAsync(selectedCustomer);
            if (success)
            {
                AnsiConsole.MarkupLine("[green]Customer updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to update customer.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

        private async Task ManageInventoryAsync()
        {
            var keepManagingInventory = true;
            while (keepManagingInventory)
            {
                var inventoryAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Inventory Management:")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "List Products",
                            "Add Product",
                            "Update Product",
                            "Delete Product",
                            "Return to Main Menu" }));

                switch (inventoryAction)
                {
                    case "List Products":
                        // Implementation needed
                        break;
                    case "Add Product":
                        // Implementation needed
                        break;
                    case "Update Product":
                        // Implementation needed
                        break;
                    case "Delete Product":
                        // Implementation needed
                        break;
                    case "Return to Main Menu":
                        keepManagingInventory = false;
                        break;
                }
            }
        }

        private async Task ManageOrdersAsync()
        {
            // Implementation needed
        }
    }
}