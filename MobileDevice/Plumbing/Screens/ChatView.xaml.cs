using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : BaseContentView
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private Contact _toContact;
        private ObservableCollection<UserMessage> _collection;
        public Contact ToContact
        {
            get => _toContact;
            set
            {
                _toContact = value;
                var prevCol = _collection;
                if (prevCol != null)
                    prevCol.CollectionChanged -= CollectionChanged;
                _collection = _toContact != null && Singleton<Context>.Instance.ChatMessages.TryGetValue(_toContact.Id, out var c) ? c : null;
                if (_collection != null)
                    _collection.CollectionChanged += CollectionChanged;
                MessagesListView.ItemsSource = _collection;
                if (_collection != null)
                    MessagesListView.ScrollTo(_collection.LastOrDefault(), ScrollToPosition.End, false);
            }
        }

        public override Task OnApearing()
        {
            MessageTextBox.Text = null;
            MessageTextBox.Focus();
            return base.OnApearing();
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_collection != null)
                MessagesListView.ScrollTo(_collection.LastOrDefault(), ScrollToPosition.End, true);
        }

        public override async void OnBackButtonPressed()
        {
            await Main.NavigateToView<MenuView>();
        }

        private void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }

        private void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }

        public Func<string, Task> OnSendMessage { get; set; }
        private async void SendMessage(object sender, EventArgs e)
        {
            if (OnSendMessage != null)
                await OnSendMessage(MessageTextBox.Text);
            MessageTextBox.Text = null;
            MessageTextBox.Focus();
        }

        public override void OnClosing()
        {
            Controller?.OnClosing();
            ToContact = null;
            base.OnClosing();
        }
    }
}