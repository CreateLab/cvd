using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Covid.Models;
using ReactiveUI;

namespace Covid.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        // The command that navigates a user to first view model.
        public ReactiveCommand<Unit, IRoutableViewModel> CountriesCommand { get; }

        // The command that navigates a user back.
        public ReactiveCommand<Unit, IRoutableViewModel> GlobalCommand { get; }
        public ReactiveCommand<Country, IRoutableViewModel> CurrentCountyCommand { get; }
        public MainWindowViewModel()
        {
            GlobalCommand = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new GlobalVM(this))
                );
            CurrentCountyCommand = ReactiveCommand.CreateFromObservable<Country,IRoutableViewModel>(
                c => Router.Navigate.Execute(new CountryInfoVM(this, c))
                );
            CountriesCommand = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new CountriesVM(this,GoToCurrentCountry))
                );
          
            GlobalCommand.Execute().Subscribe();
        }

        public void GoToCurrentCountry(Country c)
        {
            CurrentCountyCommand.Execute(c);
        }
    }
}