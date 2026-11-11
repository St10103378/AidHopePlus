using AidHopePlus.Models;
using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class AwarenessEventsPage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<AwarenessEvent> _allEvents = new();

        public AwarenessEventsPage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                _allEvents = await _db.GetAllEventsAsync();
                EventsList.ItemsSource = _allEvents;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadData();
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string search = e.NewTextValue?.ToLower() ?? "";
            if (string.IsNullOrWhiteSpace(search))
            {
                EventsList.ItemsSource = _allEvents;
                return;
            }

            EventsList.ItemsSource = _allEvents
                .Where(ev => ev.EventName.ToLower().Contains(search) ||
                             (ev.Location?.ToLower().Contains(search) ?? false))
                .ToList();
        }

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Create Event", "Event Name:");
            if (string.IsNullOrWhiteSpace(name)) return;

            string location = await DisplayPromptAsync("Create Event", "Location:");
            string dateStr = await DisplayPromptAsync("Create Event", "Date (yyyy-MM-dd):", initialValue: DateTime.Today.ToString("yyyy-MM-dd"));

            if (!DateTime.TryParse(dateStr, out DateTime date))
            {
                await DisplayAlert("Error", "Invalid date format", "OK");
                return;
            }

            var evt = new AwarenessEvent
            {
                EventName = name,
                Location = location,
                EventDate = date
            };

            await _db.AddEventAsync(evt);
            LoadData();
        }

        private async void OnViewClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is AwarenessEvent ev)
            {
                await DisplayAlert(ev.EventName,
                    $"Location: {ev.Location}\nDate: {ev.EventDate:yyyy-MM-dd}\n\n{ev.Description}",
                    "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Info", "Edit functionality can be expanded later", "OK");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is AwarenessEvent ev)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Delete event '{ev.EventName}'?", "Yes", "No");

                if (confirm)
                {
                    await _db.DeleteEventAsync(ev.Id);
                    LoadData();
                }
            }
        }
    }
}
