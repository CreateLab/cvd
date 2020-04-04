using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Covid.ViewModels;
using ReactiveUI;

namespace Covid.Views
{
    public class Global: ReactiveUserControl<GlobalVM>
    {
        public Global()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}