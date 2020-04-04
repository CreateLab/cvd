using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Covid.ViewModels;
using ReactiveUI;

namespace Covid.Views
{
    public class Countries: ReactiveUserControl<CountriesVM>
    {
        public Countries()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}