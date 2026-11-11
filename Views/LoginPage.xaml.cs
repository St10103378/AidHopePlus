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
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width <= 0 || height <= 0)
                return;

            if (width < 700)
            {
                // MOBILE
                MainLayout.ColumnDefinitions =
                    new ColumnDefinitionCollection
                    {
                new ColumnDefinition(GridLength.Star)
                    };

                MainLayout.RowDefinitions =
                    new RowDefinitionCollection
                    {
                new RowDefinition(new GridLength(220)),
                new RowDefinition(GridLength.Auto)
                    };

                Grid.SetColumn(BrandingPanel, 0);
                Grid.SetRow(BrandingPanel, 0);

                Grid.SetColumn(LoginPanel, 0);
                Grid.SetRow(LoginPanel, 1);
            }
            else
            {
                // DESKTOP
                MainLayout.ColumnDefinitions =
                    new ColumnDefinitionCollection
                    {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
                    };

                MainLayout.RowDefinitions =
                    new RowDefinitionCollection
                    {
                new RowDefinition(GridLength.Star)
                    };

                Grid.SetColumn(BrandingPanel, 0);
                Grid.SetRow(BrandingPanel, 0);

                Grid.SetColumn(LoginPanel, 1);
                Grid.SetRow(LoginPanel, 0);
            }
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
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("LoggedInUsername", username);
                Preferences.Set("UserRole", user.Role);

                Application.Current.MainPage = new AppShell(_db);
            }
            else
            {
                ErrorLabel.Text = "Invalid username or password";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}
