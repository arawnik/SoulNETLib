using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Blazor.Bootstrap;

namespace SoulNETLib.Blazor.Tests;

public sealed class BootstrapFieldCssClassProviderTests
{
    private readonly BootstrapFieldCssClassProvider sut = new();

    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void GetFieldCssClass_UnmodifiedValidField_ReturnsEmpty()
    {
        var editContext = new EditContext(new TestModel());
        var field = editContext.Field(nameof(TestModel.Name));

        var result = sut.GetFieldCssClass(editContext, field);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetFieldCssClass_ModifiedValidField_ReturnsIsValid()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var field = editContext.Field(nameof(TestModel.Name));
        editContext.NotifyFieldChanged(field);

        var result = sut.GetFieldCssClass(editContext, field);

        Assert.Equal("is-valid", result);
    }

    [Fact]
    public void GetFieldCssClass_UnmodifiedInvalidField_ReturnsIsInvalid()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var field = editContext.Field(nameof(TestModel.Name));

        var store = new ValidationMessageStore(editContext);
        store.Add(field, "Required");

        var result = sut.GetFieldCssClass(editContext, field);

        Assert.Equal("is-invalid", result);
    }

    [Fact]
    public void GetFieldCssClass_ModifiedInvalidField_ReturnsIsInvalid()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var field = editContext.Field(nameof(TestModel.Name));

        var store = new ValidationMessageStore(editContext);
        store.Add(field, "Required");
        editContext.NotifyFieldChanged(field);

        var result = sut.GetFieldCssClass(editContext, field);

        Assert.Equal("is-invalid", result);
    }

    [Fact]
    public void GetFieldCssClass_NullEditContext_ThrowsArgumentNullException()
    {
        var field = new FieldIdentifier(new TestModel(), nameof(TestModel.Name));

        Assert.Throws<ArgumentNullException>(() => sut.GetFieldCssClass(null!, field));
    }
}
