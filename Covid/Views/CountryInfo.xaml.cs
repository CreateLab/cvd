using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Covid.ViewModels;
using ReactiveUI;

namespace Covid.Views
{
    public class CountryInfo: ReactiveUserControl<CountryInfoVM>
    {
        public CountryInfo()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}