using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MinimalApiTutorial.MinimalApi
{
    public static class DiEndPoints
    {
        public static void MapDiEndPoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/di")
                .WithTags("Dependency Injection");

            #region EndPoints

            endpoints.MapGet("/developers", GetAllDevelopersAsync);

            endpoints.MapGet("/developers/{id:int}", GetDeveloperByIdAsync);

            endpoints.MapPost("/developers", AddDeveloperAsync);
            
            endpoints.MapPut("/developers/{id:int}", UpdateDeveloperAsync);

            endpoints.MapDelete("/developers/{id:int}", DeleteDeveloperAsync);

            #endregion
        }

        #region Endpoint Handlers

        private static async Task<IResult> GetAllDevelopersAsync(ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var developers = await service.GetAllAsync(cancellationToken);
            return TypedResults.Ok(developers);
        }

        private static async Task<IResult> GetDeveloperByIdAsync(int id, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var developer = await service.GetByIdAsync(id, cancellationToken);
            return developer is not null
                ? TypedResults.Ok(developer)
                : TypedResults.NotFound($"Developer with ID {id} not found.");
        }

        private static async Task<IResult> AddDeveloperAsync(
            SoftwareDeveloper developer,
            ISoftwareDeveloperService service,
            IValidator<SoftwareDeveloper> validator,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(developer, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }

            var addedDeveloper = await service.AddAsync(developer, cancellationToken);
            return TypedResults.Created($"/developers/{addedDeveloper.Id}", addedDeveloper);
        }


        private static async Task<IResult> UpdateDeveloperAsync(int id, SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken)
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

        private static async Task<IResult> DeleteDeveloperAsync(int id, ISoftwareDeveloperService service, CancellationToken cancellationToken)
        {
            var success = await service.DeleteAsync(id, cancellationToken);
            return TypedResults.Ok(success);
        }

        #endregion
    }
}
