using Microsoft.Extensions.DependencyInjection;

namespace LottieAnimation
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Set minimum window size
            window.MinimumWidth = 320;
            window.MinimumHeight = 568;

            // Optional: Set initial size
            window.Width = 1024;
            window.Height = 768;

            // Optional: Set maximum size
            window.MaximumWidth = 1920;
            window.MaximumHeight = 1080;

            return window;
        }
    }
}