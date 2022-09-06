using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Awari.Model;

namespace Awari.ViewModel
{
    public class AwariViewModel : ViewModelBase
    {

        private AwariGameModel _model;
        
        private List<Button> mybuttons = new List<Button>();
        public Boolean second = false;
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public Boolean IsGameFour
        {
            get { return _model.GameDifficulty == GameDifficulty.Negyes; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Negyes)
                    return;

                _model.GameDifficulty = GameDifficulty.Negyes;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");
            }
        }

        public Boolean IsGameEigth
        {
            get { return _model.GameDifficulty == GameDifficulty.Nyolcas; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Nyolcas)
                    return;

                _model.GameDifficulty = GameDifficulty.Nyolcas;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");
            }
        }

        public Boolean IsGameTwelve
        {
            get { return _model.GameDifficulty == GameDifficulty.Tizenkettes; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Tizenkettes)
                    return;

                _model.GameDifficulty = GameDifficulty.Tizenkettes;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");
            }
        }

        public event EventHandler NewGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public event EventHandler ExitGame;

        public AwariViewModel(AwariGameModel model)
        {

            _model = model;
            _model.GameOver += new EventHandler<AwariEventArgs>(Model_GameOver);
            


            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
                 
        }

        public List<Button> myAwariTable { get { return TableSetting(); } }
       
        private void Model_GameOver(object sender, AwariEventArgs e)
        {
            
        }
        private void OnNewGame()
        {
            if (NewGame != null)
            {
                NewGame(this, EventArgs.Empty);
                OnPropertyChanged(nameof(myAwariTable));
            }
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
            {
                //OnPropertyChanged hamarabb fut le, mint a LoadGame.
                LoadGame(this, EventArgs.Empty);
                OnPropertyChanged(nameof(myAwariTable));
                              
                
            }
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
                OnPropertyChanged(nameof(myAwariTable));
            }
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
            {
                ExitGame(this, EventArgs.Empty);
            }
        }


        private void ButtonRestrict()
        {
            if (_model.CurrentPlayer == 0)
            {
                for (int i = _model.Table.NNumber / 2 + 1; i < _model.Table.TableSize - 1; i++)
                {
                    mybuttons[i].Background =Brushes.Blue;
                }
                for (int i = 0; i < _model.Table.NNumber / 2; i++)
                {
                    mybuttons[i].IsEnabled = true;
                    mybuttons[i].Background = Brushes.Green;
                }
                for (int i = 0; i < _model.Table.NNumber / 2; i++)
                {
                    if (_model.Table.GetValue(i) == 0)
                    {
                        mybuttons[i].IsEnabled = true;
                    }
                }
            }
            if (_model.CurrentPlayer == 1)
            {
                for (int i = 0; i < _model.Table.NNumber / 2; i++)
                {
                    mybuttons[i].Background = Brushes.Red;
                }
                for (int i = _model.Table.NNumber / 2 + 1; i < _model.Table.TableSize - 1; i++)
                {
                    mybuttons[i].IsEnabled = true;
                    mybuttons[i].Background = Brushes.Green;
                }
                for (int i = _model.Table.NNumber / 2 + 1; i < _model.Table.TableSize - 1; i++)
                {
                    if (_model.Table.GetValue(i) == 0)
                    {
                    }
                }
            }



        }

        private void myon_Click(object sender, RoutedEventArgs e)
        {
           
            var mybutton = sender as Button;
            Int32 x = (int)mybutton.Tag;
            if (_model.Table.NNumber+1==x || x == _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.CurrentPlayer == 0 && x > _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.CurrentPlayer == 1 && x < _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.Table.GetValue(x) == 0)
            {
                return;
            }
            
            if (second)
                second = _model.StonePacking(_model.CurrentPlayer, x, false);
            else
            {
                second = _model.StonePacking(_model.CurrentPlayer, x, true);
            }
            if (!second)
            {
                _model.ChangePlayer();
            }

            ButtonRestrict();

            for (int i = 0; i < _model.Table.TableSize; i++)
            {
                mybuttons[i].Content = _model.Table.GetValue(i).ToString();
            }
            Boolean over = false;
            over = _model.IsGameOver;
            if (over)
            {
                /// Red Player Cup Score compared to Blue Player's Cup
                if (_model.Table.GetValue(_model.Table.NNumber / 2) > _model.Table.GetValue(_model.Table.TableSize - 1))
                {
                    _model.OnGameOver(0);
                }
                else if (_model.Table.GetValue(_model.Table.NNumber / 2) < _model.Table.GetValue(_model.Table.TableSize - 1))
                {
                    _model.OnGameOver(1);
                }
                else
                {
                    _model.OnGameOver(2);
                }
            }

        }

        private List<Button> TableSetting()
        {
            if (!_model.gameLoaded)
            {
                return null;
            }
            mybuttons = new List<Button>();
            Button seged = new Button();
            //Generating Red cups
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                seged = new Button();
                seged.Tag=i;
                seged.Content = _model.Table.GetValue(i).ToString();
                seged.Height = 50;
                seged.Width = 50;
                seged.VerticalAlignment = VerticalAlignment.Top;
                seged.HorizontalAlignment = HorizontalAlignment.Left;
                seged.Background = Brushes.Red;
                seged.Margin = new Thickness(130 + 100 * i, 350, 40, 40);
                seged.Click += new RoutedEventHandler(myon_Click);              
                mybuttons.Add(seged);
            }
            //Red Player Score Cup
            seged = new Button();
            seged.Tag = _model.Table.NNumber / 2;
            seged.Content = _model.Table.GetValue(_model.Table.NNumber + 1).ToString();
            seged.Height = 50;
            seged.Width = 50;
            seged.VerticalAlignment = VerticalAlignment.Top;
            seged.HorizontalAlignment = HorizontalAlignment.Left;
            seged.Background = Brushes.Red;
            seged.Margin = new Thickness(130 + 100 * (_model.Table.NNumber / 2), 250, 0, 0);
            seged.Click += new RoutedEventHandler(myon_Click);
            mybuttons.Add(seged);
            //Generating Blue cups
            for (int i = (_model.Table.NNumber / 2 )+1; i < _model.Table.NNumber+1; i++)
            {
                seged = new Button();
                seged.Tag = i;
                seged.Content = _model.Table.GetValue(i).ToString();
                seged.Height = 50;
                seged.Width = 50;
                seged.VerticalAlignment = VerticalAlignment.Top;
                seged.HorizontalAlignment = HorizontalAlignment.Left;
                seged.Background = Brushes.Blue;
                seged.Margin = new Thickness(130 + 100 * (_model.Table.NNumber - i), 150, 40, 40);
                seged.Click += new RoutedEventHandler(myon_Click);
                mybuttons.Add(seged);
            }
            //Blue Player Score Cup
            seged = new Button();
            seged.Tag = _model.Table.NNumber + 1;
            seged.Content = _model.Table.GetValue(_model.Table.NNumber + 1).ToString();
            seged.Height = 50;
            seged.Width = 50;
            seged.VerticalAlignment = VerticalAlignment.Top;
            seged.HorizontalAlignment = HorizontalAlignment.Left;
            seged.Background = Brushes.Blue;
            seged.Margin = new Thickness(40, 250, 0, 0);
            seged.Click += new RoutedEventHandler(myon_Click);
            mybuttons.Add(seged);
            ButtonRestrict();
            return mybuttons;
        }


        
    }
}
