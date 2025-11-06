# Picker Dialog Customization Sample

This .NET MAUI sample demonstrates how to create a custom Picker control with full dialog customization across all platforms (Android, iOS, macOS, Windows). It showcases platform-specific handler implementations to customize the native picker dialog's background color, text colors, and selected item styling.

## 🎨 Features Demonstrated

### Custom Picker Control with Dialog Styling

This sample features a `CustomPicker` control that extends the standard MAUI `Picker` with three additional customizable properties:

- **DialogBackgroundColor**: Customizes the background color of the picker dialog/dropdown
- **DialogTextColor**: Customizes the text color of unselected items in the dialog
- **SelectedTextColor**: Customizes the text color of the currently selected item in the dialog

### Platform-Specific Implementations

The sample includes custom handlers for each platform:

- **Android**: Custom `AlertDialog` styling with `ColorDrawable` backgrounds and `CheckedTextView` text color customization
- **iOS/macOS**: Custom `UIPickerView` delegate with `NSAttributedString` for text styling and background color customization
- **Windows**: Custom `ComboBox` styling with `ComboBoxItem` style setters for background and foreground colors

## 🔧 Technical Implementation

### Custom Control Definition

The `CustomPicker` class extends the standard `Picker` control with additional bindable properties:

```csharp
public class CustomPicker : Picker, ICustomPicker
{
    public static readonly BindableProperty DialogBackgroundColorProperty = ...
    public static readonly BindableProperty DialogTextColorProperty = ...
    public static readonly BindableProperty SelectedTextColorProperty = ...

    public Color DialogBackgroundColor { get; set; }
    public Color DialogTextColor { get; set; }
    public Color SelectedTextColor { get; set; }
}
```

### XAML Usage Example

```xml
<controls:CustomPicker
    x:Name="picker"
    Title="Choose Country"
    BackgroundColor="Red"
    SelectedTextColor="DarkOrange"
    DialogBackgroundColor="Pink"
    DialogTextColor="DarkGreen"
    FontSize="16">
    <Picker.Items>
        <x:String>United States</x:String>
        <x:String>Canada</x:String>
        <x:String>United Kingdom</x:String>
        <!-- More items... -->
    </Picker.Items>
</controls:CustomPicker>
```

## 📱 Platform-Specific Handler Implementations

### Android Handler

Uses reflection to access the internal `AlertDialog` and customizes:

- Dialog window background color using `SetBackgroundDrawable`
- List item text colors via `ViewTreeObserver` and `GlobalLayoutListener`
- Selected vs. unselected item styling with `CheckedTextView`

```csharp
public partial class CustomPickerHandler : PickerHandler
{
    void OnCustomizeDialog(object? sender, EventArgs e)
    {
        // Access AlertDialog via reflection
        // Apply background color to dialog window
        // Customize CheckedTextView colors for each item
    }
}
```

### iOS/macOS Handler

**iOS**: Customizes `UIPickerView` directly during platform view creation

- Sets `BackgroundColor` on the picker view
- Uses custom `UIPickerViewDelegate` with `GetAttributedTitle` for text styling

**macOS (Catalyst)**: Hooks into the `Started` event to customize the `UIAlertController`

- Finds and customizes the presented alert controller
- Applies colors to the alert view and embedded picker view

```csharp
public class CustomPickerViewDelegate : UIPickerViewDelegate
{
    public override NSAttributedString GetAttributedTitle(
        UIPickerView pickerView, nint row, nint component)
    {
        // Apply different colors for selected vs. unselected items
        // Return NSAttributedString with custom text color
    }
}
```

### Windows Handler

Customizes the `ComboBox` control by:

- Creating custom `Style` for `ComboBoxItem` elements
- Setting `Background` and `Foreground` properties on dropdown items
- Adding the style to the ComboBox's `Resources` collection

```csharp
public partial class CustomPickerHandler : PickerHandler
{
    protected override ComboBox CreatePlatformView()
    {
        var comboBox = base.CreatePlatformView();
        // Apply custom item styles
        // Set background and foreground colors
    }
}
```

## 🔑 Key Concepts

### Custom Handler Registration

Handlers are registered in `MauiProgram.cs` using platform-specific conditional compilation:

```csharp
builder.ConfigureMauiHandlers(handlers =>
{
#if ANDROID
    handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#elif WINDOWS
    handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#elif IOS || MACCATALYST
    handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#endif
});
```

### Handler Architecture

- **Partial Classes**: Each platform has its own partial implementation of `CustomPickerHandler`
- **Platform View Access**: Handlers override methods like `CreatePlatformView()` and `ConnectHandler()` to access native controls
- **Event Hooks**: Connect to platform-specific events (e.g., `Click` on Android, `Started` on macOS)
- **Reflection (Android)**: Uses reflection to access internal dialog field when necessary

### Color Conversion

Each platform requires color conversion from MAUI `Color` to platform-specific color types:

- **Android**: `.ToPlatform()` → `Android.Graphics.Color`
- **iOS/macOS**: `.ToPlatform()` → `UIKit.UIColor`
- **Windows**: Custom conversion to `Windows.UI.Color` (ARGB bytes)

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK
- Visual Studio 2022 (17.13 or later) or Visual Studio Code with .NET MAUI extension
- Platform-specific workloads:
  - Android: Android SDK (API 21+)
  - iOS/macOS: Xcode 15.0+ (macOS only)
  - Windows: Windows 10.0.17763.0+

### Running the Sample

1. Clone the repository
2. Open `PickerColorCustomization.sln` in Visual Studio or open the folder in VS Code
3. Select your target platform (Android, iOS, macOS, or Windows)
4. Build and run the application
5. Click on the picker to see the customized dialog with your specified colors

## 💡 Key Takeaways

### When to Use Custom Handlers

Custom handlers are necessary when you need to:

- Customize native platform controls beyond standard MAUI properties
- Access platform-specific dialog or popup styling
- Implement features that aren't exposed through the standard control API

### Limitations

- **Android Reflection**: The Android implementation uses reflection to access the internal `_dialog` field. This is suppressed with `UnconditionalSuppressMessage` for AOT compilation.
- **macOS Timing**: The macOS implementation requires a small delay to ensure the alert controller is fully presented before customization.
- **Platform Differences**: Each platform has different native controls (AlertDialog, UIPickerView, ComboBox), requiring unique implementation approaches.

### Best Practices

1. **Use Partial Classes**: Keep platform-specific code separated using partial classes with platform-specific file names (`.Android.cs`, `.iOS.cs`, `.Windows.cs`)
2. **Conditional Compilation**: Register handlers using `#if` directives in `MauiProgram.cs`
3. **Memory Management**: Properly disconnect event handlers in `DisconnectHandler()` to prevent memory leaks
4. **Color Conversion**: Use `.ToPlatform()` extension methods when available, or create custom conversion methods
5. **AOT Considerations**: Use appropriate attributes (`UnconditionalSuppressMessage`) when reflection is unavoidable

## 📚 Related Documentation

- [.NET MAUI Picker Control](https://learn.microsoft.com/dotnet/maui/user-interface/controls/picker)
- [Customizing Controls with Handlers](https://learn.microsoft.com/dotnet/maui/user-interface/handlers/customize)

## 🎯 What You'll Learn

This sample demonstrates:

- Creating custom controls by extending existing MAUI controls
- Implementing platform-specific handlers using partial classes
- Accessing and customizing native platform controls
- Managing platform-specific styling and theming
- Properly registering and configuring custom handlers
- Color conversion between MAUI and platform-specific types
