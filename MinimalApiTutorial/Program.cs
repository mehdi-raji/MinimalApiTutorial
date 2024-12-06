using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "New Api", Version = "v1" });
});

var app = builder.Build();

// Enable Swagger in development mode
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//
// app.MapGet("/hello", () => "hello with name")
// 	.WithName("hi");
//
// app.MapGet("/url", (LinkGenerator link) =>
// 	$"{link.GetPathByName("hi", values: null)}");
RouteBuilderExtensions.Map(app);

app.Run();

public static class RouteBuilderExtensions
{
	public static void Map(WebApplication app)
	{
		// Map endpoints with IResult handlers
		app.MapGet("/students", GetAllStudents)
			.WithName("GetAllStudents") 
			.WithTags("Student API");   

		app.MapPost("/students", AddStudent)
			.WithName("AddStudent")     
			.WithTags("Student API");

		app.MapPut("/students", EditStudent)
			.WithName("EditStudent")    
			.WithTags("Student API");

		app.MapDelete("/students", DeleteStudent)
			.WithName("DeleteStudent")  
			.WithTags("Student API");
	}

	private static IResult GetAllStudents()
	{
		return Results.Ok(new { Message = "All Students" });
	}

	private static IResult AddStudent()
	{
		return Results.Ok(new { Message = "Insert Student" });
	}

	private static IResult EditStudent()
	{
		return Results.Ok(new { Message = "Edit Student" });
	}

	private static IResult DeleteStudent()
	{
		return Results.Ok(new { Message = "Delete Student" });
	}
}
