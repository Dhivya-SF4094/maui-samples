using Microsoft.Maui.Platform;

namespace PickerDemo.Control;

public class CustomPicker : Picker, ICustomPicker
{
    public static readonly BindableProperty DialogBackgroundColorProperty =
  BindableProperty.Create(nameof(DialogBackgroundColor), typeof(Color), typeof(CustomPicker), Colors.White);

    public static readonly BindableProperty DialogTextColorProperty =
        BindableProperty.Create(nameof(DialogTextColor), typeof(Color), typeof(CustomPicker), Colors.Black);

    public static readonly BindableProperty SelectedItemTextColorProperty =
            BindableProperty.Create(nameof(SelectedItemTextColor), typeof(Color), typeof(CustomPicker), Colors.Black);

    public Color SelectedItemTextColor
    {
        get => (Color)GetValue(SelectedItemTextColorProperty);
        set => SetValue(SelectedItemTextColorProperty, value);
    }

    public Color DialogBackgroundColor
    {
        get => (Color)GetValue(DialogBackgroundColorProperty);
        set => SetValue(DialogBackgroundColorProperty, value);
    }

    public Color DialogTextColor
    {
        get => (Color)GetValue(DialogTextColorProperty);
        set => SetValue(DialogTextColorProperty, value);
    }
}