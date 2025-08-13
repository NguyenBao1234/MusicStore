using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Message;

namespace MusicStore.Views;

public partial class MusicStoreWindow : Window
{
    public MusicStoreWindow()
    {
        InitializeComponent();
        // Register a handler to listen for the message sent by the view model.
        WeakReferenceMessenger.Default.Register<MusicStoreWindow, MusicStoreCloseMessage>
        (this, static (window, message) =>// static lambda = no closure, cannot capture outer variables, better performance

            {
                //Definition of callback here
                // Close the dialog and return the selected album.
                window.Close(message.SelectedAlbum);
            });
    }
}