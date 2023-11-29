using AddressProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressProvider
{
    public class Startup
    {
        public static WebApplication WebApp(params string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<AddressDatabase>(opt => opt.UseInMemoryDatabase("Addresses"));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapGet("/address/{id}", async (Guid id, AddressDatabase addressDatabase) =>
            {
                if (id.Equals(new Guid("93edc1a1-5093-4d30-a9c1-da04765553b7")))
                {
                    var address = new Address
                    {
                        Id = id,
                        AddressType = "delivery",
                        Street = "Main Street",
                        Number = 16,
                        City = "Beverly Hills",
                        ZipCode = 90210,
                        State = "California",
                        Country = "Canada"
                    };

                    return Results.Ok(address);
                }

                return Results.NotFound();
            });

            app.MapPost("/address", async (Address address, AddressDatabase addressDatabase) =>
            {
                addressDatabase.Addresses.Add(address);
                await addressDatabase.SaveChangesAsync();

                return Results.Created($"/address/{address.Id}", address);
            });

            app.MapDelete("/address/{id}", async (Guid id, AddressDatabase addressDatabase) =>
            {
                if (await addressDatabase.Addresses.FindAsync(id) is Address address)
                {
                    addressDatabase.Addresses.Remove(address);
                    await addressDatabase.SaveChangesAsync();
                }

                return Results.NoContent();
            });

            return app;
        }
    }
}
