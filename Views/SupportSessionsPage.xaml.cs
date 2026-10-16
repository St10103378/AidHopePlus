using AidHopePlus.Models;
using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class SupportSessionsPage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<SupportSession> _allSessions = new();

        public SupportSessionsPage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadData();
        }

        private async void LoadData()
        {
            _allSessions = await _db.GetAllSupportSessionsAsync();
            SessionsList.ItemsSource = _allSessions;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string search = e.NewTextValue?.ToLower() ?? "";
            if (string.IsNullOrWhiteSpace(search))
            {
                SessionsList.ItemsSource = _allSessions;
                return;
            }

            SessionsList.ItemsSource = _allSessions
                .Where(s => (s.Beneficiary?.FullName?.ToLower().Contains(search) ?? false) ||
                            s.SupportType.ToLower().Contains(search))
                .ToList();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var beneficiaries = await _db.GetAllBeneficiariesAsync();
            if (!beneficiaries.Any())
            {
                await DisplayAlert("Error", "No beneficiaries found. Add beneficiaries first.", "OK");
                return;
            }

            string[] names = beneficiaries.Select(b => b.FullName).ToArray();
            string selectedName = await DisplayActionSheet("Select Beneficiary", "Cancel", null, names);
            if (selectedName == "Cancel" || selectedName == null) return;

            var beneficiary = beneficiaries.First(b => b.FullName == selectedName);

            string[] types = { "Psychosocial Support", "Counselling", "Medical Referral", "Spiritual Care",
                               "Home Visit", "Treatment Adherence Support", "Food Parcel" };
            string supportType = await DisplayActionSheet("Support Type", "Cancel", null, types);
            if (supportType == "Cancel" || supportType == null) return;

            string notes = await DisplayPromptAsync("Follow-up Notes", "Enter notes:");

            var session = new SupportSession
            {
                BeneficiaryId = beneficiary.Id,
                SupportType = supportType,
                SessionDate = DateTime.Today,
                FollowUpNotes = notes
            };

            await _db.AddSupportSessionAsync(session);
            LoadData();
        }
    }
}
