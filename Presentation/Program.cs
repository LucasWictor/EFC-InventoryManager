using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices(services =>
{
services.AddDbContext<DataContext>(options =>
     options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\Inlamningsuppgift\Infrastructure\Data\MyLocalDb.mdf;Integrated Security=True;Connect Timeout=30"));

    //Repository registrations
    services.AddScoped<CustomerRepository>();
    services.AddScoped<OrderRepository>();
    services.AddScoped<OrderDetailRepository>();
    services.AddScoped<ProductRepository>();

    //Service registratrions
    services.AddScoped<CustomerService>();
    services.AddScoped<InventoryService>();
    services.AddScoped<OrderService>();

});

var host = builder.Build();

//application start logic
//....
host.Dispose();