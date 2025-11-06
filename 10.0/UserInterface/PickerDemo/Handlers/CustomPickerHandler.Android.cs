using Microsoft.Maui.Handlers;
using Android.Widget;
using Android.Content;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
using MauiPicker = Microsoft.Maui.Platform.MauiPicker;
using PickerDemo.Control;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;

namespace PickerDemo.Handlers;

public partial class CustomPickerHandler : PickerHandler
{
    private System.Reflection.FieldInfo? _dialogFieldInfo;

    protected override void ConnectHandler(MauiPicker platformView)
    {
        base.ConnectHandler(platformView);

        // Cache the FieldInfo (only done once)
        _dialogFieldInfo ??= GetDialogField();

        platformView.Click += OnCustomizeDialog;
    }

    [UnconditionalSuppressMessage("AOT", "IL2070:UnrecognizedReflectionPattern", Justification = "The _dialog field is internal to PickerHandler and preserved by MAUI framework")]
    static System.Reflection.FieldInfo? GetDialogField()
    {
        return typeof(PickerHandler).GetField("_dialog",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
    }

    protected override void DisconnectHandler(MauiPicker platformView)
    {
        platformView.Click -= OnCustomizeDialog;
        base.DisconnectHandler(platformView);
    }

    void OnCustomizeDialog(object? sender, EventArgs e)
    {
        if (VirtualView is not CustomPicker customPicker)
        {
            return;
        }

        // Get dialog via reflection (unavoidable without framework changes)
        var dialog = _dialogFieldInfo?.GetValue(this) as AppCompatAlertDialog;

        // Apply background color
        if (dialog?.Window != null && customPicker.DialogBackgroundColor != null)
        {
            dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(customPicker.DialogBackgroundColor.ToPlatform()));
        }

        var dialogListView = dialog?.ListView;

        // Set adapter to ensure proper item layout
        if (dialogListView != null && Context != null)
        {
            var alertDialogLayout = Context.Resources?.GetIdentifier("select_dialog_singlechoice_material", "layout", "android") ?? 0;
            dialogListView.ChoiceMode = Android.Widget.ChoiceMode.Single;

            // Apply text color customization
            dialogListView.ViewTreeObserver?.AddOnGlobalLayoutListener(
                new GlobalLayoutListener(() =>
                {
                    var selectedPosition = VirtualView?.SelectedIndex ?? -1;

                    for (int i = 0; i < dialogListView.ChildCount; i++)
                    {
                        if (dialogListView.GetChildAt(i) is Android.Widget.CheckedTextView textView)
                        {
                            // Determine if this is the selected item
                            var isSelected = (dialogListView.FirstVisiblePosition + i) == selectedPosition;

                            if (isSelected)
                            {
                                // Apply selected item colors
                                textView.SetTextColor(customPicker.SelectedItemTextColor.ToPlatform());
                            }
                            else
                            {
                                // Apply regular text color
                                textView.SetTextColor(customPicker.DialogTextColor.ToPlatform());
                            }
                        }
                    }
                }));
        }
    }
}

public class GlobalLayoutListener : Java.Lang.Object,
    Android.Views.ViewTreeObserver.IOnGlobalLayoutListener
{
    private readonly Action _action;
    public GlobalLayoutListener(Action action) => _action = action;
    public void OnGlobalLayout() => _action?.Invoke();
}