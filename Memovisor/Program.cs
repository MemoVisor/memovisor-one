using Memovisor.Services;
using Memovisor.Services.MessageHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<InMemoryImageStorage>(provider =>
    {
        var baseUrl = builder.Configuration.GetValue<string>("Application:Host");
        return new InMemoryImageStorage(baseUrl);
    });
builder.Services.AddTransient<Handlers>();
builder.Services.AddSingleton<TelegramService>(provider =>
    {
        var handlers = provider.GetRequiredService<Handlers>();
        var token = builder.Configuration.GetValue<string>("TelegramBot:Token");
        return new TelegramService(token, handlers);
    });

builder.Services.AddTransient<HandlerFactory>();
builder.Services.AddTransient<HelpHandler>();
builder.Services.AddTransient<UrlHandler>();
builder.Services.AddTransient<PhotoHandler>();
builder.Services.AddTransient<DocumentHandler>();

var policyPreflightMaxAge = TimeSpan.FromDays(1);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetPreflightMaxAge(policyPreflightMaxAge);
    });
});

var app = builder.Build();

var telegramService = app.Services.GetRequiredService<TelegramService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();
