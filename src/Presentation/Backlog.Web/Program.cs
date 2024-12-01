using System.Net;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Common;
using Backlog.Data;
using Backlog.Data.Repository;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Common;
using Backlog.Web.Helpers.ModelBinding;
using Backlog.Web.Helpers.Routing;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Scrutor;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMvcCore();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = builder.Configuration.GetConnectionString("AppContext");

builder.Services.AddDbContext<ApplicationContext>(options =>
     options.UseLazyLoadingProxies()
     .UseSqlServer(connectionString)
     .EnableDetailedErrors()
     .EnableSensitiveDataLogging(true)
     .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug())));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ICacheManager, MemoryCacheManager>();
builder.Services.AddScoped<IWorkContext, WorkContext>();

builder.Services.AddEasyCaching(option => { option.UseInMemory(builder.Configuration, "default", "easycahing:inmemory"); });

builder.Services.Scan(scan => scan
                .FromApplicationDependencies(a => a.FullName.StartsWith("Backlog"))
                .AddClasses(true)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface((service, filter) => filter.Where(implementation => implementation.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)))
                .WithScopedLifetime());

var settings = new List<Type>();
var assemblies = AppDomain.CurrentDomain.GetAssemblies();
foreach (var assembly in assemblies)
{
    try
    {
        var types = assembly.GetTypes()
       .Where(x => typeof(ISettings).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
       .ToList();
        settings.AddRange(types);
    }
    catch { }
}

foreach (var setting in settings)
{
    builder.Services.AddScoped(setting, serviceProvider =>
    {
        return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
    });
}

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMvcCore(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.ModelMetadataDetailsProviders.Add(new MetadataProvider());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.AccessDeniedPath = "/account/forbidden/";
    options.LoginPath = "/Login";
    options.SlidingExpiration = true;
});

#region Mini Profile

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    options.TrackConnectionOpenClose = true;
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
    options.PopupDecimalPlaces = 1;
    options.EnableMvcFilterProfiling = true;
    options.EnableMvcViewProfiling = true;
});

#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/PageNotFound");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseExceptionHandler();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoint => RouteProvider.Configure(endpoint));

await app.RunAsync();
