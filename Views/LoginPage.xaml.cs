using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseService _db;

        public LoginPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim() ?? "";
            string password = PasswordEntry.Text ?? "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "Please enter username and password";
                ErrorLabel.IsVisible = true;
                return;
            }

            var user = await _db.LoginAsync(username, password);

            if (user != null)
            {
                // Successful login → go to main app with side menu
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorLabel.Text = "Invalid username or password";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}
