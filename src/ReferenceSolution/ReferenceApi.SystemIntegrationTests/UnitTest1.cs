namespace ReferenceApi.SystemIntegrationTests;

public class EmployeesTests
{
    [Fact(Skip = "demo")]
    public async Task AddingAnEmployee()
    {

        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5253");

        var body = """
            {
                "firstName": "Jim",
                "lastName": "Smith
            }
            """;
        var response = await client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(body),

        });

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);




    }
}