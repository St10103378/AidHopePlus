using AidHopePlus.Views;
using AidHopePlus.Services;

namespace AidHopePlus
{
    public partial class AppShell : Shell
    {
        private readonly DatabaseService _db;

        public AppShell(DatabaseService db)
        {
            InitializeComponent();
            _db = db;

            string userRole = Preferences.Get("UserRole", "");

            // Only Admin can see User Management
            UserManagementItem.IsVisible =
                userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }



        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Logout",
                "Are you sure you want to logout?",
                "Yes", "No");

            if (confirm)
            {
                Preferences.Remove("IsLoggedIn");
                Preferences.Remove("LoggedInUsername");
                Preferences.Remove("UserRole");

                Application.Current.MainPage =
                    new NavigationPage(new LoginPage(_db));
            }
        }
    }
}