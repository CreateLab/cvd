using System;
using System.ComponentModel;
using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;
using Covid.Models;
using Flurl.Http;
using ReactiveUI;

namespace Covid.ViewModels
{
    public class GlobalVM : ReactiveObject, IRoutableViewModel
    {
        private int _cases;
        private int _death;
        private int _recovered;
        private int _affectedCountries;
        public IScreen HostScreen { get; }

        public int Cases
        {
            get => _cases;
            set { this.RaiseAndSetIfChanged(ref _cases, value); }
        }

        public int Deaths
        {
            get => _death;
            set { this.RaiseAndSetIfChanged(ref _death, value); }
        }

        public int Recovered
        {
            get => _recovered;
            set { this.RaiseAndSetIfChanged(ref _recovered, value); }
        }

        public int AffectedCountries
        {
            get => _affectedCountries;
            set { this.RaiseAndSetIfChanged(ref _affectedCountries, value); }
        }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        private ReactiveCommand<Unit, Unit> GetAllDataCasesCommand { get; }

        public GlobalVM(IScreen screen)
        {
            HostScreen = screen;
            GetAllDataCasesCommand = ReactiveCommand.CreateFromTask(GetAllCases);
            GetAllDataCasesCommand.Execute().Subscribe();
        }

        private async Task GetAllCases()
        {
            var allCases = await "https://corona.lmao.ninja/v2/all".GetJsonAsync<AllCases>();
            Cases = allCases.Cases;
            AffectedCountries = allCases.AffectedCountries;
            Recovered = allCases.Recovered;
            Deaths = allCases.Deaths;
        }
    }
}