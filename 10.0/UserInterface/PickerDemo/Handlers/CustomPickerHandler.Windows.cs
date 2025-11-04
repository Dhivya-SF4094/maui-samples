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

            // Apply same colors to dropdown items
            var itemStyle = new Microsoft.UI.Xaml.Style(typeof(ComboBoxItem));
            itemStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(ComboBoxItem.BackgroundProperty, new Microsoft.UI.Xaml.Media.SolidColorBrush(dialogBackgroundColor)));
            itemStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(ComboBoxItem.ForegroundProperty, new Microsoft.UI.Xaml.Media.SolidColorBrush(textColor)));

            comboBox.Resources.Add(typeof(ComboBoxItem), itemStyle);
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