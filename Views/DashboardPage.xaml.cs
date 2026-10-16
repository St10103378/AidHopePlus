using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class DashboardPage : ContentPage
    {
        private readonly DatabaseService _db;

        public DashboardPage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var stats = await _db.GetDashboardStatsAsync();
                BeneficiariesCount.Text = stats.Beneficiaries.ToString();
                SessionsCount.Text = stats.Sessions.ToString();
                EventsCount.Text = stats.Events.ToString();
                AttendanceCount.Text = stats.Attendance.ToString();
                PendingCount.Text = stats.Pending.ToString();

                var sessions = await _db.GetRecentSupportSessionsAsync(8);
                RecentSessionsList.ItemsSource = sessions;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not load data. Check your Azure SQL connection.\n\n" + ex.Message, "OK");
            }
        }

        private async void OnBeneficiariesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Beneficiaries");
        }

        private async void OnEventsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Awareness");
        }

        private async void OnSupportClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Support");
        }
    }
}
