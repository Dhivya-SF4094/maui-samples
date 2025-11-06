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

    public static void MapDialogBackgroundColor(CustomPickerHandler handler, CustomPicker picker)
    {
#if IOS
        if (handler.PlatformView?.InputView is UIPickerView pickerView)
        {
            pickerView.BackgroundColor = picker.DialogBackgroundColor.ToPlatform();
        }
#endif
    }

    public static void MapDialogTextColor(CustomPickerHandler handler, CustomPicker picker)
    {
    }

    public static void MapSelectedTextColor(CustomPickerHandler handler, CustomPicker picker)
    {
#if IOS
        if (handler.PlatformView?.InputView is UIPickerView pickerView)
        {
            pickerView.ReloadAllComponents();
        }
#endif
    }

    protected override MauiPicker CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();
        if (VirtualView is not CustomPicker customPicker)
            return platformView;

#if IOS
        if (platformView.InputView is UIPickerView pickerView)
        {
            pickerView.BackgroundColor = customPicker.DialogBackgroundColor.ToPlatform();
            pickerView.Delegate = new CustomPickerViewDelegate(pickerView, customPicker);
            pickerView.ReloadAllComponents();
        }
#elif MACCATALYST
        _customPicker = customPicker;
#endif

        return platformView;
    }

#if MACCATALYST
    protected override void ConnectHandler(MauiPicker platformView)
    {
        base.ConnectHandler(platformView);
        platformView.Started += OnPickerStarted;
    }

    protected override void DisconnectHandler(MauiPicker platformView)
    {
        platformView.Started -= OnPickerStarted;
        base.DisconnectHandler(platformView);
    }

    void OnPickerStarted(object? sender, EventArgs e)
    {
        if (_customPicker is null)
            return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(100);
            FindAndCustomizeAlertController(_customPicker);
        });
    }

    void FindAndCustomizeAlertController(CustomPicker customPicker)
    {
        var alertController = UIApplication.SharedApplication.ConnectedScenes
            .OfType<UIWindowScene>()
            .SelectMany(scene => scene.Windows)
            .Select(w => w.RootViewController?.PresentedViewController)
            .OfType<UIAlertController>()
            .FirstOrDefault(ac => ac.View != null);

        if (alertController?.View is null)
            return;

        alertController.View.BackgroundColor = customPicker.DialogBackgroundColor.ToPlatform();

        var pickerView = alertController.View.Subviews.OfType<UIPickerView>().LastOrDefault();
        if (pickerView is not null)
        {
            pickerView.Delegate = new CustomPickerViewDelegate(pickerView, customPicker);
        }
    }
#endif
}

public class CustomPickerViewDelegate : UIPickerViewDelegate
{
    private readonly UIPickerView _pickerView;
    private readonly CustomPicker _customPicker;
    private readonly IUIPickerViewDelegate? _originalDelegate;

    public CustomPickerViewDelegate(UIPickerView pickerView, CustomPicker customPicker)
    {
        _pickerView = pickerView;
        _customPicker = customPicker;
        _originalDelegate = pickerView.Delegate;
    }

    public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
    {
        var title = _pickerView.Model?.GetTitle(pickerView, row, component) ?? string.Empty;
        var selectedRow = pickerView.SelectedRowInComponent(component);
        var textColor = row == selectedRow
            ? _customPicker.SelectedTextColor.ToPlatform()
            : _customPicker.DialogTextColor.ToPlatform();

        return new NSAttributedString(title, new UIStringAttributes
        {
            ForegroundColor = textColor,
        });
    }

    public override void Selected(UIPickerView pickerView, nint row, nint component)
    {
        _originalDelegate?.Selected(pickerView, row, component);
        pickerView.ReloadAllComponents();
    }
}