using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.JsonPatch;
using NLog.Web;
using NLog;
using CityInfo.API.Services;
using CityInfo.API.Context;
using Microsoft.EntityFrameworkCore;

var logger = NLog.LogManager.Setup().LoadConfigurationFromXml("nlog.config").GetCurrentClassLogger();
logger.Info("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);


    var services = builder.Services;

    //services.AddNewtonsoftJson();
    //services.AddMvc();
    services.AddMvc(option => option.EnableEndpointRouting = false)
        .AddMvcOptions(o =>
        {
            // o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
        }).AddNewtonsoftJson();
    //.AddNewtonsoftJson(o=>
    //{
    //    if(o.SerializerSettings.ContractResolver != null)
    //    {
    //        var castedResolver = o.SerializerSettings.ContractResolver
    //                                as DefaultContractResolver;
    //        castedResolver.NamingStrategy = null;
    //    }
    //});

    //For NLog

    builder.Host.UseNLog();


    //For Mock Mail Services
#if DEBUG
        services.AddTransient<IMailService,LocalMailService>();
#else
        services.AddTransient<IMailService,CloudMailService>();
#endif

    //EF
    var connectionString = builder.Configuration["ConnectionStrings:cityInfoDbConnectionString"];
    services.AddDbContext<CityInfoContext>(o=>
    {
        o.UseSqlServer(connectionString);
    });

    services.AddScoped<ICityInfoRepository, CityInfoRepository>();



    var app = builder.Build();

   

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler();
    }

    app.UseStatusCodePages();
    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllers("default", "{controller=Home}/{action=Index}");
    //});

    app.UseMvc();


    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}


