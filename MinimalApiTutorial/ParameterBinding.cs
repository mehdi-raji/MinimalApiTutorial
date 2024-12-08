using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class ParameterBinding
{
	public static void Bind(this WebApplication app)
	{
		//1.Route Parameters
		app.MapGet("/developers/{id:int}", (int id) =>
		{
			return TypedResults.Ok($"Fetching developer with ID: {id}");
		}).WithTags("Parameter Binding");
           


		//2.Query String Parameters
		app.MapGet("/developers/search", ([FromQuery(Name = "developerName")] string? name) =>
		{
			if (string.IsNullOrWhiteSpace(name))
				return Results.NotFound();
			return TypedResults.Ok(new { message = "Searching developer with name: {name}" });
		}).WithTags("Parameter Binding");

		//3. Header Parameters
		app.MapGet("/developers/validate", ([FromHeader(Name = "X-API-Key")] string apiKey) =>
		{
			if (string.IsNullOrWhiteSpace(apiKey) || apiKey != "12345")
			{
				return Results.Unauthorized();
			}
			return Results.Ok(new { Authorization = apiKey });
		}).WithTags("Parameter Binding");

		//4.Body Parameters
		app.MapPost("/developers", (SoftwareDeveloper developer) =>
		{
			return TypedResults.Ok($"Received developer: {developer.Name}, Specialization: {developer.Specialization}");
		}).WithTags("Parameter Binding");


		//5. FromBody
		app.MapPost("/developer", ([FromBody] SoftwareDeveloper dev) =>
		{
			var name = dev.Name;
			var specialization = dev.Specialization;

			return TypedResults.Ok($"Received developer: {name}, Specialization: {specialization}");
		}).WithTags("Parameter Binding");


		//6.Form Parameters
		app.MapPost("/developers/upload", async (HttpContext context) =>
		{
			var form = await context.Request.ReadFormAsync();
			var name = form["name"].ToString();
			var specialization = form["specialization"].ToString();

			return TypedResults.Ok($"Received developer: {name}, Specialization: {specialization}");
		}).WithTags("Parameter Binding");


		//7.Services Injection
		app.MapGet("/developers/service", (ILogger<Program> logger) =>
		{
			logger.LogInformation("Service-based handler invoked.");
			return TypedResults.Ok("Service injection is working!");
		}).WithTags("Parameter Binding");

		//8.From Services
		app.MapGet("/developers/fromservices", ([FromServices] DeveloperService DevService) =>
		{
			var welcome = DevService.GiveWelcomePackage();
			return TypedResults.Ok($"welcome package means : {welcome}");
		}).WithTags("Parameter Binding");


		//9.Raw HttpContext
		app.MapGet("/developers/raw", (HttpContext context) =>
		{
			var requestMethod = context.Request.Method;
			return TypedResults.Ok($"Request Method: {requestMethod}");
		}).WithTags("Parameter Binding");





		app.MapGet("/{id}", ([FromRoute] int id,
							 [FromQuery(Name = "p")] int page,
							 [FromServices] DeveloperService service,
							 [FromHeader(Name = "Content-Type")] string contentType)
							 => {

							 }
		).WithTags("Parameter Binding");

	}
}
public class DeveloperService
{
    public string GiveWelcomePackage()
    {
        return "Laptop and so many other things...";
    }
}
public class SoftwareDeveloper
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Experience { get; set; }
    public string? Specialization { get; set; }



}