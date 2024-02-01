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
                        await DeleteCustomerAsync();
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

   
        // UPDATE CUSTOMER
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
            // Might need more fields.
            selectedCustomer.FirstName = firstName;
            selectedCustomer.LastName = lastName;
            selectedCustomer.Email = email;
            // Might need more fields.
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

        // DELETE CUSTOMER
        private async Task DeleteCustomerAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            AnsiConsole.Clear();

            var CustomerDict = customers.ToDictionary(c => c.CustomerId.ToString(), c => $"{c.FirstName} {c.LastName}");
            var CustomerId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a customer to delete:")
                .PageSize(10)
                .AddChoices(CustomerDict.Keys));

            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete this customer? This action cannot be undone.");
            if (confirm)
            {
                var success = await _customerService.DeleteCustomerAsync(int.Parse(CustomerId));
                if (success)
                {
                    AnsiConsole.MarkupLine("[green]Customer deleted successfully![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Failed to delete customer.[/]");
                }
            }
            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }


        //IF SELECT INVENTORY 
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

        //MANAGE ORDERS

        private async Task ManageOrdersAsync()
        {
            var keepManagingOrders = true;
            while (keepManagingOrders)
            {
                var orderAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Order Management")
                        .PageSize(10)
                        .AddChoices(new[] {
                    "List Orders",
                    "Create Order",
                    "Update Order Status",
                    "Return to Main Menu" }));

                switch (orderAction)
                {
                    case "List Orders":
                        await ListOrdersAsync();
                        break;
                    case "Create Order":
                        //implementation needed
                        break;
                    case "Update Order Status":
                        //implementation needed
                        break;
                    case "Return to Main Menu":
                        keepManagingOrders = false;
                        break;
                }
            }
        }

        private async Task ListOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync(); 
            AnsiConsole.Clear();
            var table = new Table();

            table.AddColumn("Order ID");
            table.AddColumn("Customer Name");
            table.AddColumn("Order Date");
            table.AddColumn("Total Items");

            foreach (var order in orders)
            {
               
                var customerName = order.Customer != null ? $"{order.Customer.FirstName} {order.Customer.LastName}" : "N/A";
                // Summing the quantities of each OrderDetail to get Total Items
                var totalItems = order.OrderDetails.Sum(od => od.Quantity).ToString();

                table.AddRow(
                    order.OrderId.ToString(),
                    customerName,
                    order.OrderDate.ToString("d"),
                    totalItems
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }


}