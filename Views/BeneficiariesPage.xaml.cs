using AidHopePlus.Models;
using AidHopePlus.Services;

namespace AidHopePlus.Views
{
    public partial class BeneficiariesPage : ContentPage
    {
        private readonly DatabaseService _db;

        public BeneficiariesPage()
        {
            InitializeComponent();
            _db = Application.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            LoadData();
        }
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadData();
        }
        private async void LoadData(string search = "")
        {
            try
            {
                var list = string.IsNullOrWhiteSpace(search)
                    ? await _db.GetAllBeneficiariesAsync()
                    : await _db.SearchBeneficiariesAsync(search);

                BeneficiariesList.ItemsSource = list;
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
            string name = await DisplayPromptAsync("Add Beneficiary", "Full Name:");
            if (string.IsNullOrWhiteSpace(name)) return;

            string contact = await DisplayPromptAsync("Add Beneficiary", "Contact Number:");
            string status = await DisplayActionSheet("Status", "Cancel", null, "HIV Positive", "Affected");

            if (status == "Cancel" || status == null) return;

            var beneficiary = new Beneficiary
            {
                FullName = name,
                ContactNumber = contact,
                Status = status
            };

            await _db.AddBeneficiaryAsync(beneficiary);
            LoadData();
        }

        private async void OnViewClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Beneficiary b)
            {
                await DisplayAlert("Beneficiary Details",
                    $"Code: {b.BeneficiaryCode}\nName: {b.FullName}\nContact: {b.ContactNumber}\nStatus: {b.Status}",
                    "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Beneficiary b)
            {
                string newName = await DisplayPromptAsync("Edit", "Full Name:", initialValue: b.FullName);
                if (string.IsNullOrWhiteSpace(newName)) return;

                b.FullName = newName;
                await _db.UpdateBeneficiaryAsync(b);
                LoadData();
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Beneficiary b)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Delete {b.FullName}?", "Yes", "No");

                if (confirm)
                {
                    await _db.DeleteBeneficiaryAsync(b.Id);
                    LoadData();
                }
            }
        }
    }
}
