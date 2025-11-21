namespace SafeAreaDemos.Views;

public partial class Example5Container : ContentPage
{
    public Example5Container()
    {
        InitializeComponent();
    }
    async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
