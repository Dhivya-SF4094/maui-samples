using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using PickerDemo.Control;
using UIKit;
using Foundation;

namespace PickerDemo.Handlers;

public partial class CustomPickerHandler : PickerHandler
{
#if MACCATALYST
    CustomPicker? _customPicker;
#endif

    protected override MauiPicker CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();
        if (VirtualView is CustomPicker customPicker)
        {
#if IOS
            if (platformView.InputView is UIPickerView pickerView)
            {
                pickerView.BackgroundColor = customPicker.DialogBackgroundColor.ToPlatform();
                pickerView.Delegate = new CustomPickerViewDelegate(
                    customPicker.DialogTextColor.ToPlatform(),
                    pickerView,
                    customPicker);
                pickerView.ReloadAllComponents();
            }
#elif MACCATALYST
            _customPicker = customPicker;
#endif
        }

        return platformView;
    }

#if MACCATALYST
    protected override void ConnectHandler(MauiPicker platformView)
    {
        base.ConnectHandler(platformView);

        // Hook into the Started event only once
        platformView.Started += OnPickerStarted;
    }

    protected override void DisconnectHandler(MauiPicker platformView)
    {
        platformView.Started -= OnPickerStarted;
        base.DisconnectHandler(platformView);
    }

    void OnPickerStarted(object? sender, EventArgs e)
    {
        if (_customPicker != null)
        {
            // Add a small delay to ensure the UIAlertController is fully presented
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(100);
                FindAndCustomizeAlertController(_customPicker);
            });
        }
    }

    void FindAndCustomizeAlertController(CustomPicker customPicker)
    {
        var connectedScenes = UIApplication.SharedApplication.ConnectedScenes;
        foreach (var scene in connectedScenes.OfType<UIWindowScene>())
        {
            var alertController = scene.Windows
                .Select(w => w.RootViewController?.PresentedViewController)
                .OfType<UIAlertController>()
                .FirstOrDefault();

            if (alertController?.View != null)
            {

                alertController.View.BackgroundColor = customPicker.DialogBackgroundColor.ToPlatform();

                var pickerView = alertController.View.Subviews.OfType<UIPickerView>().LastOrDefault();
                //  pickerView.selected
                if (pickerView != null)
                {
                    pickerView.Delegate = new CustomPickerViewDelegate(
                        customPicker.DialogTextColor.ToPlatform(),
                        pickerView,
                        customPicker);
                }
                break;
            }
        }
    }
#endif
}

public class CustomPickerViewDelegate : UIPickerViewDelegate
{
    private readonly UIColor _textColor;
    private readonly UIPickerView _pickerView;
    private readonly CustomPicker _customPicker;
    private readonly IUIPickerViewDelegate? _originalDelegate;

    public CustomPickerViewDelegate(UIColor textColor, UIPickerView pickerView, CustomPicker customPicker)
    {
        _textColor = textColor;
        _pickerView = pickerView;
        _customPicker = customPicker;
        _originalDelegate = pickerView.Delegate;
    }

    public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
    {
        var title = _pickerView.Model?.GetTitle(pickerView, row, component) ?? string.Empty;
        var selectedRow = pickerView.SelectedRowInComponent(component);
        var SelectedTextColor = (row == selectedRow) ? _customPicker.SelectedItemTextColor.ToPlatform() : _customPicker.DialogTextColor.ToPlatform();
        return new NSAttributedString(title, new UIStringAttributes
        {
            ForegroundColor = SelectedTextColor,
        });
    }

    public override void Selected(UIPickerView pickerView, nint row, nint component)
    {
        // Call the original delegate's Selected method to ensure proper state management
        _originalDelegate?.Selected(pickerView, row, component);

        // Reload all components to update the background and text colors for all rows
        pickerView.ReloadAllComponents();
    }
}