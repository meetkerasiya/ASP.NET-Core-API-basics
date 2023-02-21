using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;

//services.AddMvc();
services.AddMvc(option => option.EnableEndpointRouting = false)
    .AddMvcOptions(o =>
    {
        o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
    });
    //.AddNewtonsoftJson(o=>
    //{
    //    if(o.SerializerSettings.ContractResolver != null)
    //    {
    //        var castedResolver = o.SerializerSettings.ContractResolver
    //                                as DefaultContractResolver;
    //        castedResolver.NamingStrategy = null;
    //    }
    //});


var app = builder.Build();


if(app.Environment.IsDevelopment())
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

