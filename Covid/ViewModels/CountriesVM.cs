using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Covid.Models;
using Covid.Models.SupportModels;
using DynamicData;
using DynamicData.Binding;
using Flurl.Http;
using ReactiveUI;

namespace Covid.ViewModels
{
    public class CountriesVM : ReactiveObject, IRoutableViewModel
    {
        private List<Country> _allCountries = new List<Country>();
        private string _filterName;
        private Country _country;
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        private ReactiveCommand<Unit, Unit> GetAllCountriesCommand { get; }
        SourceList<Country> countries = new SourceList<Country>();
        private ReadOnlyObservableCollection<Country> _collection;
        public ReadOnlyObservableCollection<Country> Collection => _collection;
        
        public string FilterName
        {
            get => _filterName;
            set { this.RaiseAndSetIfChanged(ref _filterName, value); }
        }

        public Country SelectedCountry
        {
            get => _country;
            set
            {
                this.RaiseAndSetIfChanged(ref _country, value);
                A.Invoke(value);
            }
        }

        private Action<Country> A;
        public CountriesVM(IScreen screen, Action<Country> a)
        {
            HostScreen = screen;
            A = a;
            var filter = this.WhenValueChanged(x => x.FilterName).Select(Filter);
            GetAllCountriesCommand = ReactiveCommand.CreateFromTask(GetAllCountriesInfo);
            countries.Connect().Filter(filter).ObserveOn(RxApp.MainThreadScheduler).Bind(out _collection).Subscribe();
            GetAllCountriesCommand.Execute().Subscribe();
        }

       

        private async Task GetAllCountriesInfo()
        {
            var countrySupportModels =
                await "https://corona.lmao.ninja/v2/countries".GetJsonAsync<List<CountrySupportModel>>();
            var tasks = countrySupportModels.Where(c => c.CountryInfo.Iso3 != null).Select(async c => new Country()
            {
                Cases = c.Cases,
                Deaths = c.Deaths,
                FlagImage = await GetFlag(c.CountryInfo.Flag),
                Name = c.Country,
                ISO3 = c.CountryInfo.Iso3,
                DeathsPerOneMillion = c.DeathsPerOneMillion,
                Recovered = c.Recovered,
                TodayCases = c.TodayCases,
                TodayDeaths = c.TodayDeaths
            }).ToList();
            foreach (var t in tasks)
            {
                _allCountries.Add(await t);
            }
            _allCountries.Sort((country, country1) => String.CompareOrdinal(country.Name, country1.Name));
            countries.Clear();
            countries.AddRange(_allCountries);
        }

        private async Task<Bitmap> GetFlag(String flagUrl)
        {
            var flagStream = await flagUrl.GetStreamAsync();
            var memoryStream = new MemoryStream();
            flagStream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            try
            {
                var bitmap = new Bitmap(memoryStream);
                return bitmap;
            }
            catch (Exception e)
            {
            }

            return null;
        }

        private Func<Country, bool> Filter(string filterName)
        {
            if (string.IsNullOrEmpty(filterName)) return x => true;
            return x => x.Name.ToUpper().Contains(filterName.ToUpper()) || x.ISO3.Contains(filterName.ToUpper());
        }
    }
}