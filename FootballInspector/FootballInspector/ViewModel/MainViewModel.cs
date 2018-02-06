using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FootballInspector.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FootballInspector.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {

        readonly private List<Clubs> allClubs;

        private ObservableCollection<Clubs> clubs;
        public ObservableCollection<Clubs> Clubs
        {
            get
            {
                return clubs;
            }
            set
            {
                Set(() => Clubs, ref clubs, value);
                RaisePropertyChanged();
            }
        }

        private List<string> leagues;
        public List<string> Leagues
        {
            get
            {
                return leagues;
            }
            set
            {
                leagues = value;
                RaisePropertyChanged();
            }
        }

        private string selectedLeague;
        public string SelectedLeague
        {
            get
            {
                return selectedLeague;
            }
            set
            {
                selectedLeague = value;
                RaisePropertyChanged();
            }
        }
        readonly public string InitiatedLeague;

        private Clubs selectedClub;
        public Clubs SelectedClub
        {
            get
            {
                return selectedClub;
            }
            set
            {
                Set(() => SelectedClub, ref selectedClub, value);
            }
        }

    

        public ICommand OnSelectedLeagueChangedCmd { get; set; }

        public MainViewModel()
        {
            allClubs = new List<Clubs>();
            using (FootballEntities db = new FootballEntities())
            {
                foreach(var x in db.Clubs)
                {
                    x.GoalDifference = x.ScoredGoals - x.LostGoals;
                    allClubs.Add(x);
                }
            }
            Leagues = new List<string>();
            Leagues.Add("All");
            Leagues.Add("La Liga");
            Leagues.Add("Barclays Premier League");
            Leagues.Add("Bundesliga");
            Leagues.Add("Inna");
            InitiatedLeague = Leagues.First();
            SelectedLeague = InitiatedLeague;
            clubs = new ObservableCollection<Clubs>(allClubs);
            OnSelectedLeagueChangedCmd = new RelayCommand(OnSelectedLeagueChanged);
            SelectedClub = clubs.First();
        }

        private void OnSelectedLeagueChanged()
        {
            Clubs.Clear();
            if (selectedLeague=="All")
            {
                Clubs = new ObservableCollection<Clubs>(allClubs);
            }
            else
            {
                var ctemplist = from c in allClubs where c.League == SelectedLeague select c;
                Clubs = new ObservableCollection<Model.Clubs>(ctemplist.ToList());
            }

            if (Clubs.Any())
            {
                SelectedClub = clubs.First();
            }
            else
            {
                SelectedClub = null;
            }
     
        }



    }
}