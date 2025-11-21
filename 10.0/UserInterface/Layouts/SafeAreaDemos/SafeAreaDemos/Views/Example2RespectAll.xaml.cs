using Microsoft.Maui.Platform;
using System.Collections.ObjectModel;

namespace SafeAreaDemos.Views;

public partial class Example2RespectAll : ContentPage
{
	public ObservableCollection<ChatMessage> Messages { get; set; }

	public Example2RespectAll()
	{
		InitializeComponent();

		// Initialize messages
		Messages = new ObservableCollection<ChatMessage>
		{
			new ChatMessage { Text = "Hello! How can I help you?", IsIncoming = true },
			new ChatMessage { Text = "I need some assistance", IsIncoming = false },
			new ChatMessage { Text = "Sure! What do you need help with?", IsIncoming = true },
			new ChatMessage { Text = "I'm here to answer any questions you have.", IsIncoming = true }
		};

		BindingContext = this;
	}

	async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	void OnSendButtonClicked(object sender, EventArgs e)
	{
		string message = MessageEntry.Text;

		if (!string.IsNullOrWhiteSpace(message))
		{
			// Add the message to the collection
			Messages.Add(new ChatMessage
			{
				Text = message,
				IsIncoming = false
			});

			// Clear the entry after sending
			MessageEntry.Text = string.Empty;

			// Scroll to the last message
			MessagesCollectionView.ScrollTo(Messages.Count - 1, position: ScrollToPosition.End, animate: true);
		}
	}
}

public class ChatMessage
{
	public string Text { get; set; } = string.Empty;
	public bool IsIncoming { get; set; }
	public Color BackgroundColor => IsIncoming ? Colors.LightBlue : Colors.LightGreen;
	public LayoutOptions HorizontalAlignment => IsIncoming ? LayoutOptions.Start : LayoutOptions.End;
	public CornerRadius CornerRadius => IsIncoming ? new CornerRadius(15, 15, 15, 0) : new CornerRadius(15, 15, 0, 15);
}