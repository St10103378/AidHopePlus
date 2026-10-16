using AidHopePlus.Services;
using AidHopePlus.Views;

namespace AidHopePlus
{
    public partial class App : Application
    {
        public App(DatabaseService db)
        {
            InitializeComponent();

            // Start on Login. After successful login we switch to AppShell
            MainPage = new NavigationPage(new LoginPage(db));
        }
    }
}
