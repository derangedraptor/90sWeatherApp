using Microsoft.Maui.Controls;

namespace _90sWeatherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}