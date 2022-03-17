using Hangfire;
using Hangfire.MemoryStorage;
using Orchestrator.Integrator.Application.Extensions;
using Orchestrator.Integrator.Application.Middlewares;
using Orchestrator.Integrator.Application.Rabbitmq;
using Orchestrator.Integrator.Application.RickAndMorty.Jobs;
using Orchestrator.Integrator.Application.RickAndMorty.Services;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    SerilogExtension.AddSerilogApi(builder.Configuration);
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddScoped<IRabbitmqConnectionFactory, RabbitmqConnectionFactory>();
    builder.Services.AddScoped<IProducingService, ProducingService>();
    builder.Services.AddTransient<IMessageFactory, MessageFactory>();
    builder.Services.AddHttpClient<IFindEpisodeService, FindEpisodeService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["RickAndMortyApi:BaseUrl"]);
    });
    builder.Services.AddScoped<IFindEpisodesJob, FindEpisodesJob>();
    builder.Services.AddScoped<ISendEpisodesJob, SendEpisodesJob>();
    builder.Services.AddScoped<IProcessEpisodeService, ProcessEpisodeService>();
    builder.Services.AddScoped<ISendEpisodeService, SendEpisodeService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "[Orchestrator] Integrator", Version = "v1" });
    });

    builder.Services.Configure<RabbitmqOptions>(builder.Configuration.GetSection(RabbitmqOptions.Position));
    builder.Services.Configure<ExchangeOptions>(builder.Configuration.GetSection(ExchangeOptions.Position));

    builder.Services.AddHangfire(op =>
    {
        op.UseMemoryStorage();
    });

    builder.Services.AddHangfireServer();

    using var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestSerilLogMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "[Orchestrator] Integrator v1");
            c.RoutePrefix = string.Empty;
        });
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();
    app.UseHangfireDashboard();

    var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = serviceScopeFactory.CreateScope();

    var findEpisodesJob = scope.ServiceProvider.GetRequiredService<IFindEpisodesJob>();
    var sendEpisodesJob = scope.ServiceProvider.GetRequiredService<ISendEpisodesJob>();
    //RecurringJob.AddOrUpdate(() => findEpisodesJob.RunAsync(null, CancellationToken.None), Cron.Minutely);
    BackgroundJob.Enqueue(() => findEpisodesJob.RunAsync(null, CancellationToken.None));
    BackgroundJob.Enqueue(() => sendEpisodesJob.RunAsync(null, CancellationToken.None));

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}