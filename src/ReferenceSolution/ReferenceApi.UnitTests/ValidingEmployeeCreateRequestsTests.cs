using FluentValidation.TestHelper;
using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class ValidingEmployeeCreateRequestsTests
{
    [Theory]
    [MemberData(nameof(GetValidCreateRequests))]
    public void FirstNameHasToExistAndBeValidLength(EmployeeCreateRequest model)
    {
        var validator = new EmployeeCreateRequestValidator();

        var result = validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName); ;

    }
    [Theory]
    [MemberData(nameof(GetBadFirstNameSampleModels))]
    public void FirstNameCannotExceedMinimumOrMaximumLength(EmployeeCreateRequest model)
    {
        var validator = new EmployeeCreateRequestValidator();

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);

    }


    public static IEnumerable<object[]> GetValidCreateRequests()
    {
        yield return new object[] {
            new EmployeeCreateRequest {
                FirstName = "xxx"
             }
        };
        yield return new object[] {
            new EmployeeCreateRequest {
                FirstName = new string('x',255)
             }
        };
    }
    public static IEnumerable<object[]> GetBadFirstNameSampleModels()
    {
        yield return new object[] {
            new EmployeeCreateRequest {
                FirstName = "xx" // has to be at least 3 letters
             }
        };
        yield return new object[] {
            new EmployeeCreateRequest {
                FirstName = new string('x',257) // max of 256
             }
        };
    }
}
