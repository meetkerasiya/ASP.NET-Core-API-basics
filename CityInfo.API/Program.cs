var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;

//services.AddMvc();
services.AddMvc(option=>option.EnableEndpointRouting=false);


var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}

app.UseStaticFiles();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers("default", "{controller=Home}/{action=Index}");
//});

app.UseMvc();


app.Run();

