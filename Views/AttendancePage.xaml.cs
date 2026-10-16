using AidHopePlus.Models;
using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class AttendancePage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<AttendanceRecord> _allRecords = new();

        public AttendancePage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadData();
        }

        private async void LoadData(string search = "")
        {
            try
            {
                _allRecords = string.IsNullOrWhiteSpace(search)
                    ? await _db.GetAllAttendanceAsync()
                    : await _db.SearchAttendanceAsync(search);

                AttendanceList.ItemsSource = _allRecords;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(e.NewTextValue);
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var events = await _db.GetAllEventsAsync();
            var beneficiaries = await _db.GetAllBeneficiariesAsync();

            if (!events.Any())
            {
                await DisplayAlert("Error", "No awareness events found. Create an event first.", "OK");
                return;
            }

            // Select Event
            string[] eventNames = events.Select(ev => $"{ev.EventName} ({ev.EventDate:yyyy-MM-dd})").ToArray();
            string selectedEvent = await DisplayActionSheet("Select Event", "Cancel", null, eventNames);
            if (selectedEvent == "Cancel" || selectedEvent == null) return;

            var evt = events.First(ev => selectedEvent.StartsWith(ev.EventName));

            // Select Beneficiary
            string[] names = beneficiaries.Select(b => b.FullName).ToArray();
            string selectedName = await DisplayActionSheet("Select Attendee", "Cancel", null, names);
            if (selectedName == "Cancel" || selectedName == null) return;

            var beneficiary = beneficiaries.First(b => b.FullName == selectedName);

            // Status
            string status = await DisplayActionSheet("Status", "Cancel", null, "Present", "Absent", "Late");
            if (status == "Cancel" || status == null) return;

            string notes = await DisplayPromptAsync("Notes (optional)", "Enter notes:");

            var record = new AttendanceRecord
            {
                AwarenessEventId = evt.Id,
                BeneficiaryId = beneficiary.Id,
                AttendeeName = beneficiary.FullName,
                AttendanceDate = evt.EventDate,
                Status = status,
                Notes = notes
            };

            await _db.AddAttendanceAsync(record);
            LoadData();
        }

        private async void OnViewClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is AttendanceRecord r)
            {
                await DisplayAlert("Attendance Details",
                    $"Attendee: {r.AttendeeName}\n" +
                    $"Event: {r.AwarenessEvent?.EventName}\n" +
                    $"Date: {r.AttendanceDate:yyyy-MM-dd}\n" +
                    $"Status: {r.Status}\n" +
                    $"Notes: {r.Notes ?? "-"}",
                    "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is AttendanceRecord r)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Delete attendance for {r.AttendeeName}?", "Yes", "No");

                if (confirm)
                {
                    await _db.DeleteAttendanceAsync(r.Id);
                    LoadData();
                }
            }
        }
    }
}
