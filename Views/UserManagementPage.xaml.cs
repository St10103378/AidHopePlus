using AidHopePlus.Data;          // ← Change this if your DbContext is in a different namespace
using AidHopePlus.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using AidHopePlus.Services;
using Microsoft.EntityFrameworkCore;

namespace AidHopePlus.Views
{
    public partial class UserManagementPage : ContentPage
    {
        private readonly AppDbContext _db;
        private readonly EmailService _emailService;

        public UserManagementPage()
        {
            InitializeComponent();

            // Temporary way until we set up Dependency Injection properly
            _db = new AppDbContext();
            _emailService = new EmailService();
            // We will fix this in Step 2
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                var users = _db.Users.OrderByDescending(u => u.FullName).ToList();
                UsersCollection.ItemsSource = users;
                EmptyLabel.IsVisible = users.Count == 0;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not load users:\n{ex.Message}", "OK");
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            string fullName = await DisplayPromptAsync("New User", "Full Name:");
            if (string.IsNullOrWhiteSpace(fullName)) return;

            string username = await DisplayPromptAsync("New User", "Username:");
            if (string.IsNullOrWhiteSpace(username)) return;

            string email = await DisplayPromptAsync("New User", "Email:");
            if (string.IsNullOrWhiteSpace(email)) return;

            // Validate email
            email = email.Trim();

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Invalid Email",
                    "Please enter a valid email address.", "OK");
                return;
            }

            string password = await DisplayPromptAsync(
                "New User",
                "Password (minimum 5 characters):",
                keyboard: Keyboard.Default);

            if (string.IsNullOrWhiteSpace(password)) return;

            // Validate password
            if (!IsValidPassword(password))
            {
                await DisplayAlert(
                    "Weak Password",
                    "Password must contain:\n\n" +
                    "• At least 5 characters\n" +
                    "• At least one uppercase letter (A-Z)\n" +
                    "• At least one lowercase letter (a-z)\n" +
                    "• At least one number (0-9)\n" +
                    "• At least one special character (!@#$%)",
                    "OK");

                return;
            }

            string role = await DisplayActionSheet(
                "Select Role", "Cancel", null, "Staff", "Admin", "Manager");

            if (role == "Cancel" || role == null) return;

            try
            {
                if (_db.Users.Any(u => u.Username == username.Trim()))
                {
                    await DisplayAlert("Error", "Username already exists.", "OK");
                    return;
                }

                if (_db.Users.Any(u => u.Email == email))
                {
                    await DisplayAlert("Error", "Email already exists.", "OK");
                    return;
                }

                var newUser = new User
                {
                    FullName = fullName.Trim(),
                    Username = username.Trim(),
                    Email = email,
                    PasswordHash = password,
                    Role = role
                };

                _db.Users.Add(newUser);
                await _db.SaveChangesAsync();



                LoadUsers();

                // Send welcome email
                try
                {
                    await _emailService.SendWelcomeEmailAsync(
                        newUser.Email!,
                        newUser.FullName,
                        newUser.Username,
                        password
                    );

                    await DisplayAlert(
                        "Success",
                        $"User created successfully!\n\n" +
                        $"Welcome email sent to {newUser.Email}.",
                        "OK");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);

                    await DisplayAlert(
                        "User Created",
                        $"User created successfully, but the welcome email failed.\n\n{ex.Message}",
                        "OK");
                }
              
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    $"Failed to create user:\n{ex.Message}",
                    "OK");
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
           

            return password.Length >= 5
                && Regex.IsMatch(password, @"[A-Z]")
                && Regex.IsMatch(password, @"[a-z]")
                && Regex.IsMatch(password, @"[0-9]")
                && Regex.IsMatch(password, @"[^a-zA-Z0-9]");
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadUsers();
        }
        private async void OnConfigureEmailClicked(
    object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(
                new NavigationPage(new EmailSettingsPage()));
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            if (sender is not Button btn || btn.CommandParameter is not User user)
                return;

            string action = await DisplayActionSheet(
                $"Update {user.FullName}",
                "Cancel",
                null,
                "Update Email",
                "Update Password",
                "Update Email and Password");

            if (string.IsNullOrWhiteSpace(action) || action == "Cancel")
                return;

            string newEmail = user.Email ?? "";
            string newPassword = "";

            // UPDATE EMAIL
            if (action == "Update Email" ||
                action == "Update Email and Password")
            {
                newEmail = await DisplayPromptAsync(
                    "Update Email",
                    "Enter the new email address:",
                    initialValue: user.Email ?? "",
                    keyboard: Keyboard.Email);

                if (string.IsNullOrWhiteSpace(newEmail))
                    return;

                newEmail = newEmail.Trim();

                if (!IsValidEmail(newEmail))
                {
                    await DisplayAlert(
                        "Invalid Email",
                        "Please enter a valid email address.",
                        "OK");

                    return;
                }

                bool emailExists = await _db.Users.AnyAsync(u =>
                    u.Email == newEmail &&
                    u.Username != user.Username);

                if (emailExists)
                {
                    await DisplayAlert(
                        "Duplicate Email",
                        "This email address is already registered.",
                        "OK");

                    return;
                }
            }

            // UPDATE PASSWORD
            if (action == "Update Password" ||
                action == "Update Email and Password")
            {
                newPassword = await DisplayPromptAsync(
                    "Update Password",
                    "Enter the new password:");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return;

                if (!IsValidPassword(newPassword))
                {
                    await DisplayAlert(
                        "Weak Password",
                        "Password must contain:\n\n" +
                        "• At least 5 characters\n" +
                        "• At least one uppercase letter\n" +
                        "• At least one lowercase letter\n" +
                        "• At least one number\n" +
                        "• At least one special character",
                        "OK");

                    return;
                }
            }

            bool confirm = await DisplayAlert(
                "Confirm Update",
                $"Are you sure you want to update {user.FullName}?",
                "Yes",
                "No");

            if (!confirm)
                return;

            try
            {
                // Update email if selected
                if (action == "Update Email" ||
                    action == "Update Email and Password")
                {
                    user.Email = newEmail;
                }

                // Update password if selected
                if (action == "Update Password" ||
                    action == "Update Email and Password")
                {
                    user.PasswordHash = newPassword;
                }

                await _db.SaveChangesAsync();

                LoadUsers();

                await DisplayAlert(
                    "Success",
                    "User details updated successfully!",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Update Error",
                    $"Could not update user:\n{ex.Message}",
                    "OK");
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is User user)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Delete user {user.FullName}?", "Yes", "No");

                if (!confirm) return;

                try
                {
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete user:\n{ex.Message}", "OK");
                }
            }
        }
    }
}