using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CandidateApi.Models;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Set up in-memory database for candidates
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("CandidateDb"));

        services.AddControllers(); // Enable MVC controllers
        services.AddMemoryCache(); // Add memory caching service
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map API controllers
        });
    }
}
