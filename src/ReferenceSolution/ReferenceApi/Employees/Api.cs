using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace ReferenceApi.Employees;

[FeatureGate("Employees")]
public class Api(IValidator<EmployeeCreateRequest> validator, EmployeeSlugGenerator slugGenerator) : ControllerBase
{


    [HttpPost("employees")]

    public async Task<ActionResult> AddEmployeeAsync(
        [FromBody] EmployeeCreateRequest request)
    {

        var validations = validator.Validate(request);
        if (!validations.IsValid)
        {
            return BadRequest(validations.ToDictionary());
        }
        /// save it to the database, or whatever.
        // see if any employees already use that slug, if not, cool. save it.
        // if so, generate a new one?
        var response = new EmployeeResponseItem
        {
            Id = slugGenerator.Generate(request.FirstName, request.LastName),
            FirstName = request.FirstName,
            LastName = request.LastName,
        };
        return StatusCode(201, response);
    }
}

public record EmployeeCreateRequest
{
    public required string FirstName { get; init; }
    public string? LastName { get; init; }


}

public record EmployeeResponseItem
{
    public required string Id { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
