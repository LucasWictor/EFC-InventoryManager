using Spectre.Console;
using Infrastructure.Services;
using Infrastructure.Entities;
using System.Threading.Tasks;
using System.Diagnostics;

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
                        await ListProductsAsync();
                        break;
                    case "Add Product":
                        await AddProductAsync();
                        break;
      
                    case "Update Product":
                        await UpdateProductAsync();
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

        //LIST PRODUCTS
        private async Task ListProductsAsync()
        {
            var products = await _inventoryService.GetAllProductsAsync(); 
            AnsiConsole.Clear();
            var table = new Table();

            table.AddColumn("Product ID");
            table.AddColumn("Title");
            table.AddColumn("Price");
            table.AddColumn("Quantity In Stock");

            foreach (var product in products)
            {
                table.AddRow(
                    product.ProductId.ToString(),
                    product.Title,
                    $"${product.Price}",
                    product.QuantityInStock.ToString()
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }

        //ADD PRODUCT 
        private async Task AddProductAsync()
        {
            var title = AnsiConsole.Ask<string>("Enter the product title:");
            var price = AnsiConsole.Ask<decimal>("Enter the product price:");
            var quantityInStock = AnsiConsole.Ask<int>("Enter the quantity in stock:");
            //might want to list manufacturers for selection
            var manufacturerId = AnsiConsole.Ask<int>("Enter the Manufacturer ID:");

            var product = new ProductEntity
            {
                Title = title,
                Price = price,
                QuantityInStock = quantityInStock,
                ManufacturerId = manufacturerId // Remember to ensure logic to validate this ID
            };

            var result = await _inventoryService.AddProductAsync(product); 
            if (result != null)
            {
                AnsiConsole.MarkupLine("[green]Product added successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to add product.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }

        private async Task UpdateProductAsync()
        {
            var product = await _inventoryService.GetAllProductsAsync();
            AnsiConsole.Clear();

            var productDict = product.ToDictionary(p => p.ProductId.ToString(), p => $"{p.Title}");
            var productId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a product to update:")
                .PageSize(10)
                .AddChoices(productDict.Keys));

            var selectedProduct = product.First(p => p.ProductId.ToString() == productId);

            //Prompt new product details
            var title = AnsiConsole.Ask<string>("New title:", selectedProduct.Title);
            var price = AnsiConsole.Ask<decimal>("New price:", selectedProduct.Price);
            var quantityInStock = AnsiConsole.Ask<int>("New quantity in stock:", selectedProduct.QuantityInStock);

            // Update product details
            selectedProduct.Title = title;
            selectedProduct.Price = price;
            selectedProduct.QuantityInStock = quantityInStock;

            var success = await _inventoryService.UpdateProductAsync(selectedProduct);
            if (success)
            {
                AnsiConsole.MarkupLine("[green]Product updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to update product.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
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
                        await CreateOrderAsync();
                        break;
                    case "Update Order Status":
                        await UpdateOrderStatusAsync();
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
        private async Task CreateOrderAsync()
        {
            AnsiConsole.MarkupLine("[yellow]Let's create a new order![/]");

            // Step 1: Select a Customer
            var customers = await _customerService.GetAllCustomersAsync();
            var customerChoices = customers.Select(c => $"{c.FirstName} {c.LastName} (ID: {c.CustomerId})").ToList();
            var selectedCustomerInfo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a customer for the order:")
                    .PageSize(10)
                    .AddChoices(customerChoices));
            var selectedCustomerId = int.Parse(selectedCustomerInfo.Split(new[] { "(ID: ", ")" }, StringSplitOptions.RemoveEmptyEntries)[1]);

            // Step 2: Choose Products (simplified to choosing one product)
            var products = await _inventoryService.GetAllProductsAsync();
            var productChoices = products.Select(p => $"{p.Title} (ID: {p.ProductId})").ToList();
            var selectedProductInfo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a product to add to the order:")
                    .PageSize(10)
                    .AddChoices(productChoices));
            var selectedProductId = int.Parse(selectedProductInfo.Split(new[] { "(ID: ", ")" }, StringSplitOptions.RemoveEmptyEntries)[1]);

            // Step 3: Specify Quantities
            var quantity = AnsiConsole.Ask<int>("Enter the quantity for the selected product:");

           
            // Step 4: Create the Order
            OrderEntity newOrder = new OrderEntity
            {
                CustomerId = selectedCustomerId,
                OrderDate = DateTime.Now,
                Status = "New" // Default status
            };

            OrderDetailEntity newOrderDetail = new OrderDetailEntity
            {
                ProductId = selectedProductId,
                Quantity = quantity
            };
            //wrap newOrderDetail in a List
            List<OrderDetailEntity> orderDetails = new List<OrderDetailEntity> { newOrderDetail };
            //pass orderDetails 
            var success = await _orderService.CreateOrderAsync(newOrder, orderDetails);

            if (success)
            {
                AnsiConsole.MarkupLine("[green]Order created successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to create the order.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey(); 
        }

        private async Task UpdateOrderStatusAsync()
        {
            AnsiConsole.MarkupLine("[yellow]Let's update an order's status![/]");

            // Step 1: List all orders for selection
            var orders = await _orderService.GetAllOrdersAsync();
            if (!orders.Any())
            {
                AnsiConsole.MarkupLine("[red]No orders available to update.[/]");
                return;
            }

            var orderChoices = orders.Select(o => $"Order ID: {o.OrderId}, Customer ID: {o.CustomerId}, Status: {o.Status}").ToList();
            var selectedOrderInfo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an order to update its status:")
                    .PageSize(10)
                    .AddChoices(orderChoices));
            var selectedOrderId = int.Parse(selectedOrderInfo.Split(new[] { "Order ID: ", "," }, StringSplitOptions.RemoveEmptyEntries)[0]);

            // Step 2: Enter a new status
            var newStatus = AnsiConsole.Ask<string>("Enter the new status for the order:");

            // Step 3: Update the order status
            var success = await _orderService.UpdateOrderStatusAsync(selectedOrderId, newStatus);

            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Order ID: {selectedOrderId}'s status updated to {newStatus} successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to update the order status.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}