using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using secret_santa_lottery_api.Configuration;
using secret_santa_lottery_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IParticipantRepository, ParticipantRepository>();

builder.Services.Configure<CosmosDbConfig>(
    builder.Configuration.GetSection(nameof(CosmosDbConfig))
);

builder.Configuration.AddAzureKeyVault(
        new Uri($"https://secret-santa-kv.vault.azure.net"),
        new DefaultAzureCredential());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
