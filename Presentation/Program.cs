using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Console.UI;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
    
        
        services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\Inlamningsuppgift\Infrastructure\Data\MyLocalDb.mdf;Integrated Security=True;Connect Timeout=30"));

        //  repositories registration
        services.AddScoped<CustomerRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<OrderDetailRepository>();
        services.AddScoped<ProductRepository>();

        // Services registration
        services.AddScoped<CustomerService>();
        services.AddScoped<InventoryService>();
        services.AddScoped<OrderService>();
        services.AddSingleton<ConsoleUI>();
    });

var app = builder.Build();

// Application start logic


// Run the ConsoleUI async
await app.Services.GetRequiredService<ConsoleUI>().RunAsync();

// Dispose of the host after the ConsoleUI finishes running
app.Dispose();
