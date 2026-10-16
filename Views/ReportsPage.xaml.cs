using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class ReportsPage : ContentPage
    {
        private readonly DatabaseService _db;

        public ReportsPage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadReport();
        }

        private async void LoadReport()
        {
            try
            {
                var stats = await _db.GetDashboardStatsAsync();
                TotalBeneficiaries.Text = stats.Beneficiaries.ToString();
                TotalSessions.Text = stats.Sessions.ToString();
                TotalEvents.Text = stats.Events.ToString();
                TotalAttendance.Text = stats.Attendance.ToString();

                var statusBreakdown = await _db.GetBeneficiaryStatusBreakdownAsync();
                StatusBreakdown.ItemsSource = statusBreakdown.ToList();

                var typeBreakdown = await _db.GetSupportTypeBreakdownAsync();
                SupportTypeBreakdown.ItemsSource = typeBreakdown.ToList();

                var recent = await _db.GetRecentSupportSessionsAsync(8);
                RecentSessionsList.ItemsSource = recent;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not load report data.\n\n" + ex.Message, "OK");
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadReport();
        }
    }
}
