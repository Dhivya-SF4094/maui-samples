namespace PickerDemo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        BasicCountryPicker.DialogBackgroundColor = Colors.LightBlue;
        BasicCountryPicker.DialogTextColor = Colors.Blue;
        BasicCountryPicker.SelectedItemTextColor = Colors.YellowGreen;
    }
}