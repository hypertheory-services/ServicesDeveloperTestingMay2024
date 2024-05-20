using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace ReferenceApi.Employees;

[FeatureGate("Employees")]
public class Api : ControllerBase
{
    [HttpPost("employees")]

    public async Task<ActionResult> AddEmployeeAsync(
        [FromBody] EmployeeCreateRequest request)
    {
        // Gary Bernhardt - "Sliming"
        var response = new EmployeeResponseItem
        {
            Id = $"{request.LastName.ToLower()}-{request.FirstName.ToLower()}",
            FirstName = request.FirstName,
            LastName = request.LastName,
        };
        return StatusCode(201, response);
    }
}

public record EmployeeCreateRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}

public record EmployeeResponseItem
{
    public required string Id { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}