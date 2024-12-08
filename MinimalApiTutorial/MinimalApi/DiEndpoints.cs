﻿using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalApiTutorial.MinimalApi
{
    public static class DiEndPoints
    {
        public static void MapDiEndPoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/di")
                .WithTags("Dependency Injection");

            #region EndPoints

            // Get all developers
            endpoints.MapGet("/developers", GetAllDevelopersAsync);

            // Get a developer by ID
            endpoints.MapGet("/developers/{id:int}", GetDeveloperByIdAsync);

            // Create a new developer
            endpoints.MapPost("/developers", AddDeveloperAsync);

            // Update an existing developer
            endpoints.MapPut("/developers/{id:int}", UpdateDeveloperAsync);

            // Delete a developer by ID
            endpoints.MapDelete("/developers/{id:int}", DeleteDeveloperAsync);

            #endregion
        }

        #region Endpoint Handlers

        private static async Task<Ok<IEnumerable<SoftwareDeveloper>>> GetAllDevelopersAsync(ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var developers = await service.GetAllAsync(cancellationToken);
            return TypedResults.Ok(developers);
        }

        private static async Task<Results<Ok<SoftwareDeveloper>, NotFound<string>>> GetDeveloperByIdAsync(int id, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var developer = await service.GetByIdAsync(id, cancellationToken);
            return developer is not null
                ? TypedResults.Ok(developer)
                : TypedResults.NotFound($"Developer with ID {id} not found.");
        }

        private static async Task<Created<SoftwareDeveloper>> AddDeveloperAsync(SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var addedDeveloper = await service.AddAsync(developer, cancellationToken);
            return TypedResults.Created($"/developers/{addedDeveloper.Id}", addedDeveloper);
        }

        private static async Task<Results<Ok<SoftwareDeveloper>, NotFound<string>, BadRequest<string>>> UpdateDeveloperAsync(int id, SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            if (id != developer.Id)
            {
                return TypedResults.BadRequest("ID in route does not match ID in body.");
            }

            var updatedDeveloper = await service.UpdateAsync(developer, cancellationToken);
            return updatedDeveloper is not null
                ? TypedResults.Ok(updatedDeveloper)
                : TypedResults.NotFound($"Developer with ID {id} not found.");
        }

        private static async Task<Ok<bool>> DeleteDeveloperAsync(int id, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var success = await service.DeleteAsync(id, cancellationToken);
            return TypedResults.Ok(success);
        }

        #endregion
    }
}