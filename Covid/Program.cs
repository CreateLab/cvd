using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using Covid.ViewModels;
using Covid.Views;
using ReactiveUI;
using Splat;

namespace Covid
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            RegisterControls();
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
        }

        private static void RegisterControls()
        {
            Locator.CurrentMutable.Register(()=>new Countries(),typeof(IViewFor<CountriesVM>));
            Locator.CurrentMutable.Register(()=>new Global(),typeof(IViewFor<GlobalVM>));
            Locator.CurrentMutable.Register(()=>new CountryInfo(),typeof(IViewFor<CountryInfoVM>));
        }
    }
}