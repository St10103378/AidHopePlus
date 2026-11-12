
using System.Net.Mail;

namespace AidHopePlus.Views
{
    public partial class EmailSettingsPage : ContentPage
    {
        public EmailSettingsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            GmailAddressEntry.Text =
                await SecureStorage.Default.GetAsync("GmailSmtpEmail");
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string email = GmailAddressEntry.Text?.Trim() ?? "";
            string password = GmailPasswordEntry.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert(
                    "Validation",
                    "Please enter your Gmail address and App Password.",
                    "OK");

                return;
            }

            try
            {
                var address = new MailAddress(email);

                if (address.Address != email)
                {
                    await DisplayAlert(
                        "Invalid Email",
                        "Please enter a valid Gmail address.",
                        "OK");

                    return;
                }

                await SecureStorage.Default.SetAsync(
                    "GmailSmtpEmail", email);

                await SecureStorage.Default.SetAsync(
                    "GmailSmtpPassword", password.Replace(" ", ""));

                await DisplayAlert(
                    "Success",
                    "Gmail settings saved successfully.",
                    "OK");

                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    $"Could not save email settings: {ex.Message}",
                    "OK");
            }
        }
    }
}
