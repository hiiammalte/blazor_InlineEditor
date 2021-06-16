using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace blazor_inlineEditor.Components.InlineEditing
{
    public class InlineEditingInputBase<T> : InputBase<T>
    {
        [CascadingParameter] protected internal InlineEditor<T> InlineEditor { get; set; }
        [CascadingParameter] protected virtual string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm";
        
        private T _resetValue;


        protected override void OnInitialized()
        {
            if (InlineEditor is not null)
                InlineEditor.RegisterWithInlineEditingBase(this);

            _resetValue = Value;
        }

        public async Task ResetInput()
        {
            await ValueChanged.InvokeAsync(_resetValue);
            StateHasChanged();
        }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {

            Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            bool conversionSuccess = targetType switch
            {
                Type x when x == typeof(string) => ParseString(value, out result),
                Type x when x.IsEnum => TryParseEnum(value, out result),
                Type x when x == typeof(int) => TryParseInteger(value, out result),
                Type x when x == typeof(double) => TryParseDouble(value, out result),
                Type x when x == typeof(Guid) => TryParseGuid(value, out result),
                Type x when x == typeof(DateTime) => TryParseDateTime(value, DateTimeFormat, out result),
                Type x when x == typeof(DateTimeOffset) => TryParseDateTimeOffset(value, DateTimeFormat, out result),
                _ => throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.")
            };

            if (conversionSuccess)
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format($"The field '{FieldIdentifier.FieldName}' could not be parsed.");
                return false;
            }
        }


        protected override string FormatValueAsString(T value)
        {
            return value switch
            {
                string stringValue => stringValue,
                Guid guidValue => guidValue.ToString(),
                int integerValue => BindConverter.FormatValue(integerValue, CultureInfo.InvariantCulture),
                float floatValue => BindConverter.FormatValue(floatValue, CultureInfo.InvariantCulture),
                DateTime dateTimeValue => BindConverter.FormatValue(dateTimeValue, DateTimeFormat, CultureInfo.InvariantCulture),
                DateTimeOffset dateTimeOffsetValue => BindConverter.FormatValue(dateTimeOffsetValue, DateTimeFormat, CultureInfo.InvariantCulture),
                _ => string.Empty // Handling of nullable types with no value such as Nullable<DateTime>, etc.
            };
        }

        static bool ParseString(string value, out T result)
        {
            result = (T)(object)value;
            return true;
        }

        static bool TryParseEnum(string value, out T result)
        {
            var success = BindConverter.TryConvertTo<T>(value, CultureInfo.CurrentCulture, out var parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseGuid(string value, out T result)
        {
            var success = Guid.TryParse(value, out Guid parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseInteger(string value, out T result)
        {
            var success = int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDouble(string value, out T result)
        {
            var success = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDateTime(string value, string dateTimeFormat, out T result)
        {
            var success = BindConverter.TryConvertToDateTime(value, CultureInfo.InvariantCulture, dateTimeFormat, out DateTime parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDateTimeOffset(string value, string dateTimeFormat, out T result)
        {
            var success = BindConverter.TryConvertToDateTimeOffset(value, CultureInfo.InvariantCulture, dateTimeFormat, out DateTimeOffset parsedValue);
            if (success)
            {
                result = (T)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

    }
}
