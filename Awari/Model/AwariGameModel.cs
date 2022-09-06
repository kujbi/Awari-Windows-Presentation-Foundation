using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Awari.Persistence;


namespace Awari.Model
{
    /// <summary>
    /// Játéktáblák mérete.
    /// </summary>
    public enum GameDifficulty { Negyes, Nyolcas, Tizenkettes }

    /// <summary>
    /// Awari játék.
    /// </summary>
    public class AwariGameModel
    {

        private Player _currentplayer;

        private IAwariDataAccess _dataAccess;
        private AwariTable _table;
        private GameDifficulty _gameDifficulty;
        private Int32 _tablen;
       


        public Boolean gameLoaded { get; private set;  }
        public Int32 CurrentPlayer { get { return _currentplayer == Player.PlayerRed ? 0 : 1; } }
        public void ChangePlayer() { _currentplayer = _currentplayer == Player.PlayerRed ? Player.PlayerBlue : Player.PlayerRed; }

        public Int32 Tn { get { return _tablen; } }

        public AwariTable Table { get { return _table; } }

        public Boolean IsGameOver { get { return (_table.IsBlueNull || _table.IsRedNull); } }

        public GameDifficulty GameDifficulty { get { return _gameDifficulty; } set { _gameDifficulty = value; } }

        public event EventHandler<AwariEventArgs> GameOver;

        public event EventHandler<AwariEventArgs> GameCreated;


        public AwariGameModel(IAwariDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _table = new AwariTable();
            _gameDifficulty = GameDifficulty.Nyolcas;
        }

        public void OnGameOver(int WhoWon)
        {
            //0 - red v 1 - Blue  
            if (GameOver != null)
                GameOver(this, new AwariEventArgs(WhoWon));
        }


        public void NewGame()
        {

            _table = new AwariTable();
            gameLoaded = true;
            switch (_gameDifficulty) // játéktábla beállítás
            {
                case GameDifficulty.Negyes:
                    _tablen = 4;
                    _table = new AwariTable(_tablen);
                    _currentplayer = Player.PlayerRed;
                    break;
                case GameDifficulty.Nyolcas:
                    _tablen = 8;
                    _table = new AwariTable(_tablen);
                    _currentplayer = Player.PlayerRed;
                    break;
                case GameDifficulty.Tizenkettes:
                    _tablen = 12;
                    _table = new AwariTable(_tablen);
                    _currentplayer = Player.PlayerRed;
                    break;
            }
            OnGameCreated();
        }

        public void LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table =  _dataAccess.LoadAsync(path);


            switch (_table.NNumber) // játéktábla beállítás
            {
                case 4:
                    GameDifficulty = GameDifficulty.Negyes;
                    _tablen = 4;
                    _currentplayer = Player.PlayerRed;
                    break;
                case 8:
                    _tablen = 8;
                    GameDifficulty = GameDifficulty.Nyolcas;
                    _currentplayer = Player.PlayerRed;
                    break;
                case 12:
                    _tablen = 12;
                    GameDifficulty = GameDifficulty.Tizenkettes;
                    _currentplayer = Player.PlayerRed;
                    break;
            }

            OnGameCreated();
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);
        }





        public Boolean StonePacking(Int32 player, Int32 x, Boolean firstround)
        {
            if (IsGameOver)
                return false;
            

            Int32 starter = x;
            Boolean lastownscore = false; // Score cup is the last one
            Int32 db = _table.GetValue(x);
            _table.SetValue(x, 0);
            int i;
            for (i = x; db != 0;)
            {
                i++;
                if (i == x)
                {
                    i++;
                }
                i %= _table.TableSize;


                _table.AddValue(i, 1);
                db--;

            }

            /* ehete nandayo
            if (i == 0)
            {
                i = _table.TableSize-1;
            }
            else { i = i - 1; }
            */
            if ((player == 0 && i < _table.NNumber / 2) || (player == 1 && i > _table.NNumber / 2 && i < _table.TableSize))
            {
                if (_table.GetValue(i) == 1)
                {
                    _table.SetValue(i, 0);
                    int szemkozti;
                    
                    szemkozti = _table.GetValue(_table.NNumber - i);
                    Debug.WriteLine("A szemkozti button: " + szemkozti+'\n');
                    _table.SetValue(_table.NNumber-i, 0);
                    if (player == 0)
                    {
                        _table.AddValue(_table.NNumber / 2, 1 + szemkozti);
                    }
                    if (player == 1)
                    {
                        _table.AddValue(_table.TableSize - 1, 1 + szemkozti);
                    }
                }

            }
            else if ((player == 0 && i == _table.NNumber / 2) || (player == 1 && i == _table.TableSize))
            {
                lastownscore = true;
            }

            return lastownscore && firstround;

        }
        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new AwariEventArgs(3));
        }
    }
}
