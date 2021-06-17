# Blazor Inline Editor  - Custom ASP.NET Blazor InputBase for inline editing using EditForm

Microsofts recommended way of creating webforms with validation in Blazor is by using the feature-rich EditForm component bound to a model that uses data annotations. The EditForm component is part of the Blazor framework but does not provide the ability for inline editing out of the box. This project provides a solution to this missing feature.

This project builds upon the InputBase class, a generic class from the Blazor framework, which should normally be inherited by all your custom input components. This project provides the "InlineEditingInputBase" class, a custom class acting as an intermediator between the InputBase class and your input components. To use the "InlineEditingInputBase" class, all you need to do is have your custom input components inherit from it instead of the InputBase class.

## How it works

After initialization, the generic "InlineEditingInputBase" class internally stores the initial input value of the inheriting input component as a reset value. The class also provides a function to reset the input to this inital value. Furthermore a bunch of boilerplate parsing functions, which are normally needed to be included in each of your custom input components, is included in this custom class.

The class file can be found within this path:
```
blazor_inlineEditor/Components/InlineEditing/
```

## What's also included?

This project also includes a "InlineEditor" component, providing both a UI and logic for your inline editor. Once you provide this "InlineEditor" component with a input component as a RenderFragment and an EditContext containing the model to be validated against, you are able to activate the inline editor by clicking on the initially displayed placeholder text. The component will then replace the placeholder text with the RenderFragments' input field. It will also display two buttons right next to it. One button is for saving the input (submitting the form and executing validation) and one is for resetting the changes made to the input value. If valid, both buttons will close the inline editor and display the possibly changed placeholder text. Otherwise a validation error message is displayed beneath the input field.

Use the "InlineEditor" component like so:
```
<InlineEditor
    Context="context"
    TInputType="string"
    Placeholder="Fullname..."
    Text="@user.Name"
    OnValidInput="UpdateName"
>
    <TextInput
        Placeholder="Fullname..."
        Id="username"
        @bind-Value="@user.Name"
        ValidationFor="@(() => user.Name)"
    />
</InlineEditor>

@code {
    public User user { get; set; }
    public EditContext context { get; set; }

    protected override void OnInitialized()
    {
        //Object that might be fetched from an API
        user = new User
        {
            Name = "Max"
        };

        context = new EditContext(user);
    }

    //Updating the fetched Object
    private void UpdateName(string value)
         => user.Name = value;
}                  
```

## How the InlineEditor component works
Internally this "InlineEditor" component provides a DataAnnotationValidator component and wraps all of its' content inside a EditForm tag. If a RenderFragment is provided as a ChildComponent, it will exists within the EditForm tag and can hence perform as an input field within the webform.

```
...

<EditForm EditContext="@Context" OnValidSubmit="OnValidSubmit">
    <DataAnnotationsValidator />

    <div class="inline-editor">
        @if (!IsInEditingMode)
        {
            <div @onclick="@ShowInlineEditor" class="not-input-group">
                @(string.IsNullOrEmpty(Text) ? Placeholder : Text)
            </div>
        }
        else
        {
            <div class="input-group">
                <div class="d-flex flex-row w-100">
                    <CascadingValue Value="this">
                        @ChildContent
                    </CascadingValue>
                    <div class="d-flex flex-row">
                        <button type="submit" class="btn btn-outline-secondary mb-auto" @onkeypress:preventDefault>Save</button>
                        <button @onclick="@CancelInlineEditor" class="btn btn-outline-secondary mb-auto">Undo</button>
                    </div>
                </div>
            </div>
        }
    </div>

</EditForm>

@code {
    [Parameter] public EditContext Context { get; set; }

    [Parameter] public string Placeholder { get; set; }
    [Parameter] public string Text { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<TInputType> OnValidInput { get; set; }

    private bool ShowEditMenu { get; set; } = false;
    private bool IsInEditingMode { get; set; } = false;

    private InlineEditingInputBase<TInputType> InputComponent { get; set; }

    public void RegisterWithInlineEditingBase(
        InlineEditingInputBase<TInputType> inputComponent
    )
    {
        InputComponent = inputComponent;
    }

    private void OnMouseOver()
        => ShowEditMenu = true;

    private void OnMouseLeave()
        => ShowEditMenu = false;

    private void ShowInlineEditor()
        => IsInEditingMode = true;

    private void CancelInlineEditor()
    {
        InputComponent?.ResetInput();
        IsInEditingMode = false;
    }

    protected async Task OnValidSubmit()
    {
        await OnValidInput.InvokeAsync(InputComponent.Value);
        IsInEditingMode = false;
    }

    ...
}
```

## Limitations
The EditContext provided to a EditForm will itself provide the model to the DataAnnotationValidator. This means that the validation is always performed against a model and not just against single properties within this model. This in return mean that you may need to provide the EditContext of the "InlineEditor" component with a new model, containing only a single property with DataAnnotations for validation, if there are more than one properties being validated against. If the model contains other properties that are not valid in the given state, the validation of the model will fail and the inline editor will not close and save the input.

```
//DO - use model with single validatable property
public class User
{
    [Required(ErrorMessage = "Please enter Name")]
    public string Name { get; set; }
}

//DON'T - use a model with more than one validatable property that might even fail validation by default

public class User
{
    [Required(ErrorMessage = "Please enter Name")]
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Please enter valid email address")]
    public string Email { get; set; }
}

```

## Getting started

To get this project up and running as is, feel free to follow these steps:

### Prerequisites

- .NET 5.0+ SDK
- IDE (preferably Visual Studio or Visual Studio Code)

### Setup

1. Clone this repository
2. At the root directory, restore required packages by running:

```
dotnet restore
```
3. Next, build the solution by running:

```
dotnet build
```

4. Once done, launch the application by running:

```
dotnet run
```

5. Launch https://localhost:44310/ in your browser to view the demo page of the inline editor (the port may differ, depending on your IDE configuration)