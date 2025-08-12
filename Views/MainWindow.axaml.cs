using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Message;
using MusicStore.ViewModels;

namespace MusicStore.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        WeakReferenceMessenger.Default.Register<MainWindow, PurchaseAlbumMessage>(this, static (w, m) =>
        {
            // Create an instance of MusicStoreWindow and set MusicStoreViewModel as its DataContext.
            MusicStoreWindow dialog = new MusicStoreWindow
            {
                DataContext = new MusicStoreViewModel()//Set Model as Data
            };
            // Show dialog window and reply with returned AlbumViewModel or null when the dialog is closed.
            m.Reply(dialog.ShowDialog<AlbumViewModel?>(w));
        });
    }
}