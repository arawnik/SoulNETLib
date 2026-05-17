using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Blazor;

namespace SoulNETLib.Blazor.Tests;

public sealed class FieldIdentifierHelperTests
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public AddressModel? Address { get; set; }
        public List<ItemModel> Items { get; set; } = [];
    }

    private sealed class AddressModel
    {
        public string City { get; set; } = string.Empty;
    }

    private sealed class ItemModel
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    [Fact]
    public void ToFieldIdentifier_SimpleProperty_ResolvesToModel()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "Name");

        Assert.Same(model, result.Model);
        Assert.Equal("Name", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_CaseInsensitive_ResolvesToCorrectPropertyName()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "name");

        Assert.Same(model, result.Model);
        Assert.Equal("Name", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_NestedProperty_ResolvesToChildObject()
    {
        var address = new AddressModel { City = "Helsinki" };
        var model = new TestModel { Address = address };
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "Address.City");

        Assert.Same(address, result.Model);
        Assert.Equal("City", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_IndexedProperty_ResolvesToListItem()
    {
        var item = new ItemModel { Title = "Flour", Amount = 2.5m };
        var model = new TestModel { Items = [item] };
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "Items[0].Title");

        Assert.Same(item, result.Model);
        Assert.Equal("Title", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_NullIntermediate_StopsAtLastNonNull()
    {
        var model = new TestModel { Address = null };
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "Address.City");

        Assert.Same(model, result.Model);
        Assert.Equal("Address", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_EmptyPath_ReturnsModelLevelIdentifier()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, string.Empty);

        Assert.Same(model, result.Model);
        Assert.Equal(string.Empty, result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_UnknownProperty_ReturnsRawFieldName()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var result = FieldIdentifierHelper.ToFieldIdentifier(editContext, "NonExistent");

        Assert.Same(model, result.Model);
        Assert.Equal("NonExistent", result.FieldName);
    }

    [Fact]
    public void ToFieldIdentifier_NullEditContext_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => FieldIdentifierHelper.ToFieldIdentifier(null!, "Name")
        );
    }
}
