using Spectre.Console;
using Infrastructure.Services;
using Infrastructure.Entities;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

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
            DisplayFigletBanner("InventoryPro");

            //Welcome message styling
            AnsiConsole.Clear();
            DisplayWelcomeMessage();

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                 .Title("[bold underline orange1]Please select an option:[/]")
                 .PageSize(10)
                 
                 .AddChoices(new[] {
                 "Manage Customers",
                 "Manage Inventory",
                 "Manage Orders",
                 "Exit"}));



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
        private void DisplayWelcomeMessage()
        {
            // Display the welcome message
            DisplayFigletBanner("InventoryPro");

            var welcomePanel = new Panel("[bold]Welcome to the Inventory Management System[/]")
                .Border(BoxBorder.Double)
                .BorderStyle(new Style(Color.Orange1))
                .Padding(35, 50)
                .Expand();
            AnsiConsole.Render(welcomePanel);
        }
        private void DisplayFigletBanner(string text)
        {
            var banner = new FigletText(text)
               .Color(new Color(215, 95, 175));

            AnsiConsole.Render(banner.Centered());
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
            AnsiConsole.Clear();

           
            var table = new Table()
                .Border(TableBorder.Rounded) 
                .BorderColor(Color.Grey) // Set border color
                .AddColumn(new TableColumn("[u]ID[/]").Centered())
                .AddColumn(new TableColumn("[u]First Name[/]").Centered())
                .AddColumn(new TableColumn("[u]Last Name[/]").Centered())
                .AddColumn(new TableColumn("[u]Email[/]").Centered())
                .AddColumn(new TableColumn("[u]Address[/]").Centered())
                .AddColumn(new TableColumn("[u]Phone[/]").Centered())
                .Alignment(Justify.Center)
                .Title("[bold yellow]Customer List[/]")
                .Expand();

            foreach (var customer in customers)
            {
                table.AddRow(
                    customer.CustomerId.ToString(),
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    $"{customer.StreetName}, {customer.City}, {customer.PostalCode}, {customer.Country}",
                    customer.Phone ?? "N/A"
                );
            }

            // Render the table to the console
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to return to the menu...");
            System.Console.ReadKey(true);
            AnsiConsole.Clear();
            DisplayWelcomeMessage();
        }
        //ADD A CUSTOMER
        private async Task AddCustomerAsync()
        {
            var firstName = AnsiConsole.Ask<string>("Enter the customer's first name:");
            var lastName = AnsiConsole.Ask<string>("Enter the customer's last name:");
            var email = AnsiConsole.Ask<string>("Enter the customer's email:");
            var streetName = AnsiConsole.Ask<string>("Enter the customer's street name:");
            var postalCode = AnsiConsole.Ask<string>("Enter the customer's postal code:");
            var country = AnsiConsole.Ask<string>("Enter the customer's country:");
            var city = AnsiConsole.Ask<string>("Enter the customer's city:");
            // Assuming phone is optional, not asking for it here

            var customer = new CustomerEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                StreetName = streetName,
                PostalCode = postalCode,
                Country = country,
                City = city
                // Phone is not set here, assuming it's optional
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

            AnsiConsole.MarkupLine("Press any key to return to the menu...");
            System.Console.ReadKey(true);
            AnsiConsole.Clear();
            DisplayWelcomeMessage();
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

            // Create a dictionary to map the string representation to customer ID
            var customerDict = customers.ToDictionary(c => $"{c.CustomerId} - {c.FirstName} {c.LastName}", c => c.CustomerId);

            var customerChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a customer to delete:")
                    .PageSize(10)
                    .AddChoices(customerDict.Keys)); // Change to Keys for the selection

            // Change here to use the key to find the value
            var selectedCustomerId = customerDict[customerChoice];

            // Check if the selected customer has associated orders
            var hasOrders = await _orderService.CustomerHasOrdersAsync(selectedCustomerId);
            if (hasOrders)
            {
                var deleteWithOrders = AnsiConsole.Prompt(new ConfirmationPrompt("This customer has associated orders. Do you want to delete the orders as well? (Y/N)"));
                if (deleteWithOrders)
                {
                    // Delete the associated orders first
                    await _orderService.DeleteOrdersByCustomerIdAsync(selectedCustomerId);
                }
            }

            // Deletes the customer
            var success = await _customerService.DeleteCustomerAsync(selectedCustomerId);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Customer '{customers.First(c => c.CustomerId == selectedCustomerId).FirstName} {customers.First(c => c.CustomerId == selectedCustomerId).LastName}' deleted successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to delete customer.[/]");
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
                        await DeleteProductAsync();
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
            var description = AnsiConsole.Ask<string>("Enter the product description:");
            var price = AnsiConsole.Ask<decimal>("Enter the product price:");
            var quantityInStock = AnsiConsole.Ask<int>("Enter the quantity in stock:");

           
            var manufacturerName = AnsiConsole.Ask<string>("Enter the manufacturer's name (leave empty if unknown):");

            // Create a new ProductEntity object with the provided information
            var product = new ProductEntity
            {
                Title = title,
                Description = description,
                Price = price,
                QuantityInStock = quantityInStock,
                ManufacturerName = string.IsNullOrEmpty(manufacturerName) ? null : manufacturerName // Store null if the input is empty
            };

            // Attempt to add the new product using inventory service
            var result = await _inventoryService.AddProductAsync(product);
            if (result != null)
            {

                AnsiConsole.MarkupLine("[green]Product added successfully![/]");
                AnsiConsole.MarkupLine("[yellow]Returning to the main menu...[/]");
                Thread.Sleep(2000);
                AnsiConsole.Clear();

            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to add product.[/]");
            }


            AnsiConsole.Clear();
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

        //DELETE PRODUCT
        private async Task DeleteProductAsync()
        {
            AnsiConsole.MarkupLine("[yellow]Delete a product![/]");

            // Fetch and list all products
            var products = await _inventoryService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                AnsiConsole.MarkupLine("[red]No products available.[/]");
                return;
            }

            var productChoices = products.Select(p => $"{p.Title} (ID: {p.ProductId})").ToList();
            var selectedProductInfo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a product to delete:")
                    .PageSize(10)
                    .AddChoices(productChoices));

            var selectedProductId = int.Parse(selectedProductInfo.Split(new string[] { "(ID: " }, StringSplitOptions.RemoveEmptyEntries)[1].TrimEnd(')'));

            // Confirm before deletion
            var confirm = AnsiConsole.Confirm("Are you sure you want to delete this product?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[grey]Product deletion cancelled.[/]");
                return;
            }

            // Attempt to delete the product
            var success = await _inventoryService.DeleteProductAsync(selectedProductId);
            if (success)
            {

                AnsiConsole.MarkupLine("[green]Product successfully deleted.[/]");
                AnsiConsole.MarkupLine("[yellow]Returning to the main menu...[/]");
                Thread.Sleep(2000);
                AnsiConsole.Clear();

            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to delete the product.[/]");
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