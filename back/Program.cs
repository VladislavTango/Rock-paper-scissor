using back.Services;
using GameConnect;
using Microsoft.AspNetCore.Builder;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        builder.Services.AddCors();

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost";
            options.InstanceName = "SampleInstance";
        });

        builder.Services.AddGrpc();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors(opth =>
        {
            opth.AllowAnyHeader();
            opth.AllowAnyMethod();
            opth.AllowAnyOrigin();
        });

        app.MapControllers();

        app.MapGrpcService<ConnectService>();
        app.MapGrpcService<PlayingService>();
        app.MapGrpcService<RegameService>();

        app.Run();
    }
}