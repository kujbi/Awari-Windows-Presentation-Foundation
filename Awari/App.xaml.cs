using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Awari.Model;
using Awari.Persistence;
using Awari.ViewModel;
using Awari;
using Microsoft.Win32;

namespace Awari
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AwariGameModel _model;
        private AwariViewModel _viewModel;
        private MainWindow _view;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
          
            _model = new AwariGameModel(new AwariFileDataAccess());
            _model.GameOver += new EventHandler<AwariEventArgs>(Model_GameOver);
            _model.NewGame();

        
            _viewModel = new AwariViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();         
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {               

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Awari", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            if (_model.gameLoaded)
            {
                _model.NewGame();
            }
        }

        private void ViewModel_LoadGame(object sender, System.EventArgs e)
        {
      
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); 
                openFileDialog.Title = "Awari tábla betöltése";
                openFileDialog.Filter = "Awari tábla|*.awt";
                if (openFileDialog.ShowDialog() == true)
                {                 
                     _model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (AwariDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Awari", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Awari tábla betöltése";
                saveFileDialog.Filter = "Awari tábla|*.awt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {                      
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (AwariDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Awari", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close();
        }
        private void Model_GameOver(object sender, AwariEventArgs e)
        {

            if (e.WhoWon==0) //Red Player won.
            {
                MessageBox.Show("Gratulálok, győztél piros játékos!",                                                        
                                "Awari játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else if(e.WhoWon==1) //Blue Player won.
            {
                MessageBox.Show("Gratulálok, győztél kék játékos!",
                                "Awari játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else if(e.WhoWon==2) //Tie
            {
                MessageBox.Show("Gratulálok játékosok, döntetlen lett!",
                               "Awari játék",
                               MessageBoxButton.OK,
                               MessageBoxImage.Asterisk);

            }
            else if(e.WhoWon==3)
            {
                MessageBox.Show("Váratlan hiba!",
                              "Awari játék",
                              MessageBoxButton.OK,
                              MessageBoxImage.Asterisk);

            }
        }


    }
}
