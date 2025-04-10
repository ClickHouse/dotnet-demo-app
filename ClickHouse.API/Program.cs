using ClickHouse.Core.Interfaces.Repositories;
using ClickHouse.Core.Models;
using ClickHouse.Data.Extentions;
using ClickHouse.Data.Migrations;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services
    .AddDemoServices(builder.Configuration)
    .AddOpenApi();

var app = builder.Build();

// var migration = app.Services.GetRequiredService<ClickHouseMigration>();
// await migration.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/stats", async () =>
    {
        var sensorRepo = app.Services.GetRequiredService<ISensorRepository>();

        var latestSensorTimeTask = sensorRepo.GetLatestSensorTimeAsync();
        var totalSensorsTask = sensorRepo.GetTotalSensorsCountAsync();
        var totalSamplesTask = sensorRepo.GetTotalSamplesCountAsync();

        await Task.WhenAll(
            latestSensorTimeTask,
            totalSensorsTask,
            totalSamplesTask
        );

        var stats = new DemoStats
        {
            LatestSensorTime = await latestSensorTimeTask,
            TotalSensors = await totalSensorsTask,
            TotalSamples = await totalSamplesTask
        };

        return stats;
    })
    .WithName("GetStats");

app.MapGet("/api/list-stats", async () =>
    {
        var sensorRepo = app.Services.GetRequiredService<ISensorRepository>();

        var samplesPerDateTask = sensorRepo.GetSampleCountsPerDateAsync();
        var samplesPerSensorTask = sensorRepo.GetSampleCountsPerSensorAsync();

        await Task.WhenAll(
            samplesPerDateTask,
            samplesPerSensorTask
        );

        var listStats = new DemoListStats
        {
            SamplesPerDate = (await samplesPerDateTask).ToArray(),
            SamplesPerSensor = (await samplesPerSensorTask).ToArray()
        };

        return listStats;
    })
    .WithName("GetListStats");

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
