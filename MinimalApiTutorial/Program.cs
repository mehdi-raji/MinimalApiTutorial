var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

//StudentEndPoints.Map(app);
app.MapGet("/hello", () => "hello with name")
    .WithName("hi");

app.MapGet("/url", (LinkGenerator link) =>
    $"{link.GetPathByName("hi", values: null)}");

app.MapGroup("/api/students")
    .MapStudentsApi()
    .MapGet("/id", async context=> await context.Response.WriteAsJsonAsync(new {Message = "One Student"}))
    .WithTags("Student Api");

app.Run();

public static class RouteBuilderExtensions
{
    public static RouteGroupBuilder MapStudentsApi(this RouteGroupBuilder group)
    {

        group.MapGet("/", async context =>
        {
            await context.Response.WriteAsJsonAsync(new { Message = "AllStudents" });
        });

        group.MapPost("/", async context =>
        {
            await context.Response.WriteAsJsonAsync(new { Message = "Insert" });

        });

        group.MapPut("/", async context =>
        {
            await context.Response.WriteAsJsonAsync("Edit");
        });

        group.MapDelete("/", async context =>
        {
            await context.Response.WriteAsJsonAsync("Delete");
        });

        return group;
    }
}
//public static class StudentEndPoints
//{
//    public static void Map(WebApplication app)
//    {
//        app.MapGet("/", async context =>
//        {
//            await context.Response.WriteAsJsonAsync(new { Message = "AllStudents" });
//        });

//        app.MapPost("/", async context =>
//        {
//            await context.Response.WriteAsJsonAsync(new { Message = "Insert" });

//        });

//        app.MapPut("/", async context =>
//        {
//            await context.Response.WriteAsJsonAsync("Edit");
//        });

//        app.MapDelete("/", async context =>
//        {
//            await context.Response.WriteAsJsonAsync("Delete");
//        });

//    }
//}

