using Microsoft.AspNetCore.Mvc;

namespace ReferenceApi.Employees;

public class Api : ControllerBase
{
    [HttpPost("employees")]

    public async Task<ActionResult> AddEmployeeAsync(
        [FromBody] EmployeeCreateRequest request)
    {
        return StatusCode(201);
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