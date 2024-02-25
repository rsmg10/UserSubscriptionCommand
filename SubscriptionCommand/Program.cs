using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using SubscriptionCommand.Abstraction;
using SubscriptionCommand.Infrastructure.MessageBus;
using SubscriptionCommand.Infrastructure.Presistance;
using SubscriptionCommand.Services;
using System.Net.WebSockets;
using Calzolari.Grpc.AspNetCore.Validation;
using MediatR;
using MediatR.Pipeline;
using SubscriptionCommand;
using SubscriptionCommand.Behaviours;

var builder = WebApplication.CreateBuilder(args);

  
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
builder.Services.AddDbContext<ApplicationDatabase>(o => o.UseSqlServer("Data Source=.;Database=ASHSubscriptionCommand;Trusted_Connection=true;TrustServerCertificate=True"));
builder.Services.AddScoped<IEventStore, EventStore>();

using (var scope= builder.Services.BuildServiceProvider().CreateScope())
{

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDatabase>();
    db.Database.Migrate();

}


var busConnection = builder.Configuration["ServiceBusClient"];
builder.Services.AddSingleton(new ServiceBusClient(busConnection));
builder.Services.AddSingleton<AzureMessageBus>();
builder.Services.AddGrpc(); 
builder.Services.AddGrpc(o => o.EnableMessageValidation());
builder.Services.AddValidators();
builder.Services.AddGrpcValidation();
var app = builder.Build(); 
// anis testing 
// create topics que and sub
// take  primary  connection string

// Configure the HTTP request pipeline.
app.MapGrpcService<InvitationService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseMiddleware<ExceptionMiddleware>();
app.Run();

namespace SubscriptionCommand
{
    public partial class Program { }
}