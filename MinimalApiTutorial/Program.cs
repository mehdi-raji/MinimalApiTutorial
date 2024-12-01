var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/hello-get", () => "[GET]hello world");
app.MapPost("/hello-post", () => "[POST]hello world");
app.MapPut("/hello-put", () => "[PUT]hello world");
app.MapDelete("/hello-delete", () => "[DELETE]hello world");

app.MapMethods("/new-hello-get", new[] { HttpMethods.Get }, () => "[GET] New Hello World!");
app.MapMethods("/hello-patch", new[] { HttpMethods.Patch }, () => "[PATCH] New Hello World!");

app.Run();
