using System.Collections.ObjectModel;

namespace SafeAreaDemos.ViewModels;

public class Example4ViewModel
{
    public ObservableCollection<Article> Articles { get; set; }

    public Example4ViewModel()
    {
        Articles = new ObservableCollection<Article>
        {
            new Article
            {
                Title = "What is Default?",
                Content = "SafeAreaEdges.Default uses the platform's native safe area behavior. Content automatically respects system UI elements without requiring manual adjustments."
            },
            new Article
            {
                Title = "Platform-Specific Handling",
                Content = "iOS respects notches, status bars, and home indicators. Android handles system bars and display cutouts. Each platform applies its own safe area rules automatically."
            },
            new Article
            {
                Title = "Automatic Protection",
                Content = "All edges (top, bottom, left, right) are protected by default. Your content never gets obscured by system UI elements on any platform."
            },
            new Article
            {
                Title = "Best for Standard Apps",
                Content = "Default is ideal for most applications where you want standard, predictable behavior. Perfect for forms, lists, and general content pages."
            },
            new Article
            {
                Title = "Orientation Aware",
                Content = "Automatically adjusts when the device rotates. Safe areas update dynamically without any code changes required from your end."
            }
        };
    }
}

public class Article
{
    public string Title { get; set; }
    public string Content { get; set; }
}
