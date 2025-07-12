using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactView : BaseContentView
    {
        public ContactView()
        {
            InitializeComponent();
            ContactList.ItemsSource = Singleton<Context>.Instance.Contacts;
        }

        public override async void OnBackButtonPressed()
        {
            await Main.NavigateToView<MenuView>();
        }

        private void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ContactList.SelectedItem = null;
        }

        public Func<Contact, Task> OnTapped { get; set; }
        private async void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ContactList.SelectedItem = null;
            if (OnTapped != null)
                await OnTapped(e.Item as Contact);
        }
    }
}