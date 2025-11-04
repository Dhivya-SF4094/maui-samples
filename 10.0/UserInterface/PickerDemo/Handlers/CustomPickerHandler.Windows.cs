using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using MauiColor = Microsoft.Maui.Graphics.Color;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinColor = Windows.UI.Color;

namespace PickerDemo.Handlers;

public partial class CustomPickerHandler : PickerHandler
{
    protected override ComboBox CreatePlatformView()
    {
        var comboBox = base.CreatePlatformView();

        if (VirtualView is Control.CustomPicker customPicker)
        {
            var dialogBackgroundColor = ConvertToWinColor(customPicker.DialogBackgroundColor);
            var textColor = ConvertToWinColor(customPicker.DialogTextColor);
            var selectedItemTextColor = ConvertToWinColor(customPicker.SelectedItemTextColor);

            // Apply same colors to dropdown items
            var itemStyle = new Microsoft.UI.Xaml.Style(typeof(ComboBoxItem));

            itemStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(ComboBoxItem.BackgroundProperty, new SolidColorBrush(dialogBackgroundColor)));
            itemStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(ComboBoxItem.ForegroundProperty, new SolidColorBrush(textColor)));

            // Create resource dictionary for selected item colors
            var selectedForegroundBrush = new SolidColorBrush(selectedItemTextColor);

            // Add custom resources that will be used for selected state
            comboBox.Resources["ComboBoxItemForegroundSelected"] = selectedForegroundBrush;

            comboBox.Resources[typeof(ComboBoxItem)] = itemStyle;

        }
        return comboBox;
    }

    static WinColor ConvertToWinColor(MauiColor color)
    {
        return WinColor.FromArgb(
            (byte)(color.Alpha * 255),
            (byte)(color.Red * 255),
            (byte)(color.Green * 255),
            (byte)(color.Blue * 255)
        );
    }
}