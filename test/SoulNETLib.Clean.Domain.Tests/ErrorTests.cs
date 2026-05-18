namespace SoulNETLib.Clean.Domain.Tests;

public sealed class ErrorTests
{
    #region NotFound

    [Fact]
    public void NotFound_DefaultMessage_CreatesErrorWithNotFoundCode()
    {
        var error = Error.NotFound();

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal("The requested resource was not found.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void NotFound_CustomMessage_CreatesErrorWithMessage()
    {
        var error = Error.NotFound("Item missing");

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal("Item missing", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void NotFound_TemplateWithArgs_FormatsMessage()
    {
        var error = Error.NotFound("{0} with ID {1} was not found.", "Recipe", 42);

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal("Recipe with ID 42 was not found.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void NotFound_TemplateWithMultipleArgs_FormatsAllArgs()
    {
        var id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var error = Error.NotFound("{0} #{1} in {2} not found.", "Item", id, "Warehouse");

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal($"Item #{id} in Warehouse not found.", error.Message);
    }

    [Fact]
    public void NotFound_TypeAndKey_CreatesFormattedMessage()
    {
        var id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var error = Error.NotFound("Recipe", id);

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal($"Recipe with ID {id} not found.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void NotFound_TypeAndIntKey_CreatesFormattedMessage()
    {
        var error = Error.NotFound("Category", 7);

        Assert.Equal(ErrorCodes.NotFound, error.Code);
        Assert.Equal("Category with ID 7 not found.", error.Message);
    }

    #endregion

    #region BusinessRule

    [Fact]
    public void BusinessRule_Message_CreatesError()
    {
        var error = Error.BusinessRule("Cannot close an already closed list.");

        Assert.Equal(ErrorCodes.BusinessRule, error.Code);
        Assert.Equal("Cannot close an already closed list.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void BusinessRule_TemplateWithArgs_FormatsMessage()
    {
        var error = Error.BusinessRule("{0} has already been closed.", "Shopping list");

        Assert.Equal(ErrorCodes.BusinessRule, error.Code);
        Assert.Equal("Shopping list has already been closed.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void BusinessRule_TemplateWithMultipleArgs_FormatsAllArgs()
    {
        var error = Error.BusinessRule("{0} cannot exceed {1} items.", "Cart", 100);

        Assert.Equal(ErrorCodes.BusinessRule, error.Code);
        Assert.Equal("Cart cannot exceed 100 items.", error.Message);
    }

    #endregion

    #region Validation

    [Fact]
    public void Validation_FieldAndMessage_CreatesErrorWithField()
    {
        var error = Error.Validation("Name", "'Name' must not be empty.");

        Assert.Equal(ErrorCodes.Validation, error.Code);
        Assert.Equal("'Name' must not be empty.", error.Message);
        Assert.Equal("Name", error.Field);
    }

    [Fact]
    public void Validation_FieldAndTemplateWithArgs_FormatsMessageAndSetsField()
    {
        var error = Error.Validation("Title", "'{0}' must not be empty.", "Title");

        Assert.Equal(ErrorCodes.Validation, error.Code);
        Assert.Equal("'Title' must not be empty.", error.Message);
        Assert.Equal("Title", error.Field);
    }

    [Fact]
    public void Validation_FieldAndTemplateWithMultipleArgs_FormatsAll()
    {
        var error = Error.Validation(
            "Name",
            "'{0}' must be between {1} and {2} characters.",
            "Name",
            1,
            200
        );

        Assert.Equal(ErrorCodes.Validation, error.Code);
        Assert.Equal("'Name' must be between 1 and 200 characters.", error.Message);
        Assert.Equal("Name", error.Field);
    }

    [Fact]
    public void Validation_MessageOnly_CreatesErrorWithoutField()
    {
        var error = Error.Validation("At least one item is required.");

        Assert.Equal(ErrorCodes.Validation, error.Code);
        Assert.Equal("At least one item is required.", error.Message);
        Assert.Null(error.Field);
    }

    #endregion

    #region InvalidData

    [Fact]
    public void InvalidData_Message_CreatesError()
    {
        var error = Error.InvalidData("Corrupt record found.");

        Assert.Equal(ErrorCodes.InvalidData, error.Code);
        Assert.Equal("Corrupt record found.", error.Message);
        Assert.Null(error.Field);
    }

    #endregion

    #region FromException

    [Fact]
    public void FromException_WithException_UsesExceptionMessage()
    {
        var ex = new InvalidOperationException("Something went wrong.");
        var error = Error.FromException(ex);

        Assert.Equal(ErrorCodes.General, error.Code);
        Assert.Equal("Something went wrong.", error.Message);
        Assert.Null(error.Field);
    }

    [Fact]
    public void FromException_WithCustomCode_UsesProvidedCode()
    {
        var ex = new ArgumentException("Bad arg.");
        var error = Error.FromException(ex, ErrorCodes.Validation);

        Assert.Equal(ErrorCodes.Validation, error.Code);
        Assert.Equal("Bad arg.", error.Message);
    }

    [Fact]
    public void FromException_NullException_ReturnsEmptyMessage()
    {
        var error = Error.FromException(null!, ErrorCodes.General);

        Assert.Equal(ErrorCodes.General, error.Code);
        Assert.Equal(string.Empty, error.Message);
    }

    #endregion

    #region ToResult

    [Fact]
    public void ToResult_ReturnsFailedResult()
    {
        var error = Error.NotFound("Not here.");

        var result = error.ToResult();

        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ToResultT_ReturnsFailedGenericResult()
    {
        var error = Error.BusinessRule("Nope.");

        var result = error.ToResult<int>();

        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    #endregion

    #region Record equality

    [Fact]
    public void Error_RecordEquality_SameValues_AreEqual()
    {
        var a = Error.Validation("Name", "Required.");
        var b = Error.Validation("Name", "Required.");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Error_RecordEquality_DifferentField_AreNotEqual()
    {
        var a = Error.Validation("Name", "Required.");
        var b = Error.Validation("Email", "Required.");

        Assert.NotEqual(a, b);
    }

    #endregion
}
