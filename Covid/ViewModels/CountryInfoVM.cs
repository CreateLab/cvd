using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Covid.Models;
using Covid.Models.SupportModels;
using DynamicData;
using Flurl.Http;
using OxyPlot;
using OxyPlot.Axes;
using ReactiveUI;

namespace Covid.ViewModels
{
    public class CountryInfoVM : ReactiveObject, IRoutableViewModel
    {
        private class Case
        {
            public DateTime DateTime { get; set; }
            public long Count { get; set; }
        }
        public Bitmap FlagImage { get; }
        public string Name { get; }
        public string ISO3 { get; }
        public IScreen HostScreen { get; }
        public int Cases { get;  }
        public int TodayCases { get;  }
        public int Deaths { get;  }
        public int TodayDeaths { get;  }
        public int Recovered { get;  }
        public double? DeathsPerOneMillion { get;  }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        public SourceList<DataPoint> casesList = new SourceList<DataPoint>();
        private ReadOnlyObservableCollection<DataPoint> _collectionCases;
        public ReadOnlyObservableCollection<DataPoint> CollectionCases => _collectionCases;
        public SourceList<DataPoint> recoverList = new SourceList<DataPoint>();
        private ReadOnlyObservableCollection<DataPoint> _collectionRecover;
        public ReadOnlyObservableCollection<DataPoint> RecoverCases => _collectionRecover;
        public SourceList<DataPoint> deathList = new SourceList<DataPoint>();
        private ReadOnlyObservableCollection<DataPoint> _collectionDeath;
        public ReadOnlyObservableCollection<DataPoint> DeathCases => _collectionDeath;
        public CountryInfoVM(IScreen screen, Country country)
        {
            HostScreen = screen;
            FlagImage = country.FlagImage;
            Name = country.Name;
            ISO3 = country.ISO3;
            Cases = country.Cases;
            TodayCases = country.TodayCases;
            Deaths = country.Deaths;
            TodayDeaths = country.TodayDeaths;
            DeathsPerOneMillion = country.DeathsPerOneMillion;
            Recovered = country.Recovered;
            casesList.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out _collectionCases).Subscribe();
            recoverList.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out _collectionRecover).Subscribe();
            deathList.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out _collectionDeath).Subscribe();
            ReactiveCommand.CreateFromTask(SetListDate).Execute().Subscribe();

        }

        private async Task SetListDate()
        {
            var countryTimeLine = await $"https://corona.lmao.ninja/v2/historical/{ISO3.ToLower()}".GetJsonAsync<CountryTimeLine>();
            var listCase = new List<Case>();
            var listDeath = new List<Case>();
            var listRecover = new List<Case>();
            SetAndSortList(listCase,countryTimeLine.Timeline.Cases);
            AddData(casesList,listCase);
            SetAndSortList(listDeath,countryTimeLine.Timeline.Deaths);
            AddData(deathList,listDeath);
            SetAndSortList(listRecover,countryTimeLine.Timeline.Recovered);
            AddData(recoverList,listRecover);
            
        }

        private void AddData(SourceList<DataPoint> list, List<Case> cases)
        {
            list.Clear();
            foreach (var c in cases)
            {
                list.Add(new DataPoint(DateTimeAxis.ToDouble(c.DateTime),c.Count));
            }
        }

        private void SetAndSortList(List<Case> cases, Dictionary<string, long> dictionary)
        {
            foreach (var c in dictionary)
            {
                cases.Add(new Case()
                {
                    DateTime = Convert.ToDateTime(c.Key,new CultureInfo("en-US")),
                    Count = c.Value
                });
            }
            cases.Sort(delegate(Case case1, Case case2) { return DateTime.Compare(case1.DateTime, case2.DateTime); });
        }
    }
}