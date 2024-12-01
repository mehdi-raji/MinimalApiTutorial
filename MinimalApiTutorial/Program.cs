var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/inline", () => "Inline Lambda");

var handler = () => "Lambda Variable";
app.MapGet("/var-lambda", handler);

string Hello() => "Local Function";
app.MapGet("/local-function", Hello);

var myHandler = new HelloHandler();
app.MapGet("/class", myHandler.Hello);

app.MapGet("/static-class", StaticHelloHandler.Hello);

app.Run();

public class HelloHandler
{
    public string Hello()
    {
        return "Class";
    }
}
public class StaticHelloHandler
{
    public static string Hello()
    {
        return "Class";
    }
}