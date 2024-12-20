using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using MinimalApiTutorial.MinimalApi;
using MinimalApiTutorial.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "New Api", Version = "v1" });
});
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddSingleton<ISoftwareDeveloperRepository, SoftwareDeveloperRepository>();
builder.Services.AddScoped<ISoftwareDeveloperService, SoftwareDeveloperService>();
builder.Services.AddValidatorsFromAssemblyContaining<SoftwareDeveloperValidator>();

var app = builder.Build();

// Enable Swagger in development mode
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

var people = new List<Person>
{
	new() { Id = 1, Name = "Reza" },
	new() { Id = 2, Name = "Ali" },
};

app.MapDiEndPoints();

app.MapGet("results/person/{id:int}", (int id) =>
{
	var person = people.FirstOrDefault(x => x.Id == id);

	return person is null ? Results.NotFound() : Results.Ok(person);
});


app.MapGet("typedresult/person/{id:int}", Results<Ok<Person>,NotFound>(int id) =>
{
	var person = people.FirstOrDefault(x => x.Id == id);

	return person is null ? TypedResults.NotFound() : TypedResults.Ok(person);
});
ParameterBinding.Bind(app);
app.MapGet("/student/{name}/{id:int}", (string name, int id) => $"Hello {name}");
app.Run();

public class Person
{
	public int Id { get; set; }
	public string Name { get; set; }
}