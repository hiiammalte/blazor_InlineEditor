# Blazor Inline Editor  - Custom ASP.NET Blazor InputBase for Inline Editing Forms using the EditForm component

Microsofts recommended way of creating webforms with validation in Blazor is by using the feature-rich EditForm component bound to a model that uses data annotations. The EditForm component is part of the Blazor framework but does not provide the ability for inline editing out of the box. This project provides a solution to this missing feature.

This project builds upon the InputBase class, a generic class from the Blazor framework, which should normally be inherited by all your custom input components. This project provides the "InlineEditingInputBase" class, a custom class acting as an intermediator between the InputBase class and your input components. To use the "InlineEditingInputBase" class, all you need to do is have your custom input components inherit from it instead of the InputBase class.

## How it works

After initialization, the generic "InlineEditingInputBase" class internally stores the initial input value of the inheriting input component as a reset value. The class also provides a function to reset the input to this inital value. Furthermore a bunch of boilerplate parsing functions, which are normally needed to be included in each of your custom input components, is included in this custom class.

## What's also included?

This project also includes a "InlineEditor" component, providing both a UI and logic for your inline editor. Once you provide this "InlineEditor" component with a input component as a RenderFragment and an EditContext containing the model to be validated against, you are able to activate the inline editor by clicking on the initially displayed placeholder text. The component will then replace the placeholder text with the RenderFragments' input field. It will also display two buttons right next to it. One button is for saving the input (submitting the form and executing validation) and one is for resetting the changes made to the input value. If valid, both buttons will close the inline editor and display the possibly changed placeholder text. Otherwise a validation error message is displayed beneath the input field.


## How the InlineEditor component works
Internally this "InlineEditor" component provides a DataAnnotationValidator component and wraps all of its' content inside a EditForm tag. If a RenderFragment is provided as a ChildComponent, it will exists within the EditForm tag and can hence perform as an input within the webform.

# Limitations
The EditContext provided to a EditForm will itself provide the model to the DataAnnotationValidator. This means that the validation is always performed against a model and not just against single properties within this model. This in return mean that you may need to provide the EditContext of the "InlineEditor" component with a new model, containing only a single property with DataAnnotations for validation, if there are more than one properties being validated against. If the model contains other properties that are not valid in the given state, the validation of the model will fail and the inline editor will not close and save the input.