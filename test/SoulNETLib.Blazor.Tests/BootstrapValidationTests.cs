using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using SoulNETLib.Blazor.Bootstrap;

namespace SoulNETLib.Blazor.Tests;

public sealed class BootstrapValidationTests : Bunit.BunitContext
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    private (IRenderedComponent<BootstrapValidation> Component, EditContext EditContext) RenderValidation()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingValue<EditContext>>(0);
            builder.AddComponentParameter(1, "Value", editContext);
            builder.AddComponentParameter(2, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<BootstrapValidation>(0);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        return (cut.FindComponent<BootstrapValidation>(), editContext);
    }

    [Fact]
    public void OnInitialized_SetsBootstrapCssClassProvider()
    {
        var (_, editContext) = RenderValidation();

        // Verify the CSS provider is set by checking that modified valid field returns "is-valid"
        var field = editContext.Field(nameof(TestModel.Name));
        editContext.NotifyFieldChanged(field);

        var cssClass = editContext.FieldCssClass(field);
        Assert.Contains("is-valid", cssClass, StringComparison.Ordinal);
    }

    [Fact]
    public void DisplayErrors_AddsFieldValidationMessages()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
        };

        component.Instance.DisplayErrors(errors);

        var field = editContext.Field(nameof(TestModel.Name));
        var messages = editContext.GetValidationMessages(field).ToList();
        Assert.Single(messages);
        Assert.Equal("Name is required", messages[0]);
    }

    [Fact]
    public void DisplayErrors_MultipleFields_AddsAllMessages()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
            ["Description"] = ["Description too long", "Description invalid"],
        };

        component.Instance.DisplayErrors(errors);

        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        var descMessages = editContext.GetValidationMessages(editContext.Field("Description")).ToList();

        Assert.Single(nameMessages);
        Assert.Equal(2, descMessages.Count);
    }

    [Fact]
    public void ClearErrors_RemovesAllMessages()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
        };

        component.Instance.DisplayErrors(errors);
        component.Instance.ClearErrors();

        var field = editContext.Field(nameof(TestModel.Name));
        var messages = editContext.GetValidationMessages(field).ToList();
        Assert.Empty(messages);
    }

    [Fact]
    public void AddModelError_AddsModelLevelMessage()
    {
        var (component, editContext) = RenderValidation();

        component.Instance.AddModelError("Something went wrong");

        var messages = editContext.GetValidationMessages().ToList();
        Assert.Contains("Something went wrong", messages);
    }

    [Fact]
    public void OnValidationRequested_ClearsServerErrors()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
        };
        component.Instance.DisplayErrors(errors);

        // Trigger validation requested (simulates form submit)
        editContext.Validate();

        var field = editContext.Field(nameof(TestModel.Name));
        var messages = editContext.GetValidationMessages(field).ToList();
        Assert.Empty(messages);
    }

    [Fact]
    public void OnFieldChanged_ClearsFieldSpecificErrors()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
            ["Description"] = ["Description is required"],
        };
        component.Instance.DisplayErrors(errors);

        // Notify that Name field changed
        editContext.NotifyFieldChanged(editContext.Field("Name"));

        // Name errors should be cleared
        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Empty(nameMessages);

        // Description errors should remain
        var descMessages = editContext.GetValidationMessages(editContext.Field("Description")).ToList();
        Assert.Single(descMessages);
    }

    [Fact]
    public void DisplayErrors_NullErrors_ThrowsArgumentNullException()
    {
        var (component, _) = RenderValidation();

        Assert.Throws<ArgumentNullException>(() => component.Instance.DisplayErrors(null!));
    }

    [Fact]
    public void Dispose_UnsubscribesFromEvents()
    {
        var (component, editContext) = RenderValidation();
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Name"] = ["Name is required"],
        };

        // Dispose the component
        component.Instance.Dispose();

        // After dispose, DisplayErrors should not throw but also not work
        // (CurrentEditContext is still set, but events are unsubscribed)
        // This mainly verifies no ObjectDisposedException
        component.Instance.DisplayErrors(errors);
    }
}
