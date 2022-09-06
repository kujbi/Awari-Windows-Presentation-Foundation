using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Awari.Model;
using Awari.Persistence;



namespace Awari.Test
{
    [TestClass]
    public class AwariTest
    {
        private AwariGameModel _model;
        private AwariTable _mocktable;
        private Mock<IAwariDataAccess> _mock;

        [TestInitialize]
        public void Initialize()
        {
            _mocktable = new AwariTable();
            _mocktable.SetValue(0, 1);
            _mocktable.SetValue(1, 3);
            _mocktable.SetValue(2, 4);
            _mocktable.SetValue(3, 7);
            _mocktable.SetValue(4, 0);
            _mocktable.SetValue(5, 7);
            _mocktable.SetValue(6, 8);
            _mocktable.SetValue(7, 9);
            _mocktable.SetValue(8, 0);
            _mock = new Mock<IAwariDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>())).Returns(() => (_mocktable));


            _model = new AwariGameModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            _model.GameOver += new EventHandler<AwariEventArgs>(Model_GameOver);

        }


        [TestMethod]
        public void AwariGameModelOriginalTest()
        {
            _model.NewGame();
            ///Checking the Original Gamediff
            Assert.AreEqual(GameDifficulty.Nyolcas, _model.GameDifficulty);
            ///Both Red and Blue score Cup's has 0 stones in it.
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.NNumber / 2));
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.TableSize - 1));
            ///8-Cup Game
            Assert.AreEqual(8, _model.Table.NNumber);
            ///Red Player's starter cups each have 6 stones in it.
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }
            ///Blue Player's starter cups each have 6 stones in it.
            for (int i = (_model.Table.NNumber / 2) + 1; i < _model.Table.TableSize - 1; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }

        }

        [TestMethod]
        public void AwariGameModelNewGameFourTest()
        {
            _model.GameDifficulty = GameDifficulty.Negyes;
            _model.NewGame();
            ///Checking Difficulty
            Assert.AreEqual(GameDifficulty.Negyes, _model.GameDifficulty);
            ///Both Red and Blue score Cup's has 0 stones in it.
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.NNumber / 2));
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.TableSize - 1));
            ///4-Cup Game
            Assert.AreEqual(4, _model.Table.NNumber);
            ///Red Player's starter cups each have 6 stones in it.
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }
            ///Blue Player's starter cups each have 6 stones in it.
            for (int i = (_model.Table.NNumber / 2) + 1; i < _model.Table.TableSize - 1; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }
        }

        [TestMethod]
        public void AwariGameModelNewGamerTwelveTest()
        {
            _model.GameDifficulty = GameDifficulty.Tizenkettes;
            _model.NewGame();
            ///Checking Difficulty
            Assert.AreEqual(GameDifficulty.Tizenkettes, _model.GameDifficulty);
            ///Both Red and Blue score Cup's has 0 stones in it.
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.NNumber / 2));
            Assert.AreEqual(0, _model.Table.GetValue(_model.Table.TableSize - 1));
            ///12-Cup Game
            Assert.AreEqual(12, _model.Table.NNumber);
            ///Red Player's starter cups each have 6 stones in it.
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }
            ///Blue Player's starter cups each have 6 stones in it.
            for (int i = (_model.Table.NNumber / 2) + 1; i < _model.Table.TableSize - 1; i++)
            {
                Assert.AreEqual(6, _model.Table.GetValue(i));
            }
        }

        [TestMethod]
        public void AwariGameModelLoadTest()
        {
            _model.NewGame();
            _model.LoadGameAsync(String.Empty);
            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }

        private void Model_GameOver(Object sender, AwariEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver);
        }

        [TestMethod]
        public void AwariGameModelStonepacking()
        {
            _model.NewGame();

            bool valtozo = false;
            valtozo = _model.StonePacking(_model.CurrentPlayer, 3, true);
            Assert.AreEqual(6, _model.Table.GetValue(0));
            Assert.AreEqual(6, _model.Table.GetValue(1));
            Assert.AreEqual(6, _model.Table.GetValue(2));
            Assert.AreEqual(0, _model.Table.GetValue(3));
            Assert.AreEqual(1, _model.Table.GetValue(4));
            Assert.AreEqual(7, _model.Table.GetValue(5));
            Assert.AreEqual(7, _model.Table.GetValue(6));
            Assert.AreEqual(7, _model.Table.GetValue(7));
            Assert.AreEqual(7, _model.Table.GetValue(8));
            Assert.AreEqual(1, _model.Table.GetValue(9));
            if (valtozo)
            {
                //Whatever you want to write here. There is no second chance for Red Player.
                valtozo = _model.StonePacking(_model.CurrentPlayer, 1, false);

            }
            else {
                _model.ChangePlayer();
                valtozo = false;
            }
            //Blue Player's turn
            valtozo = _model.StonePacking(_model.CurrentPlayer, 8, true);
            if (valtozo)
            {
                //It will be flase so whatever you want to write here.
                _model.StonePacking(_model.CurrentPlayer, 6, false);
            }
            else
            {
                //Red Player
                _model.ChangePlayer();
                valtozo = false;
            }
            valtozo = _model.StonePacking(_model.CurrentPlayer, 3, true);
            Assert.AreEqual(true, valtozo);
            if (valtozo)
            {
                valtozo = _model.StonePacking(_model.CurrentPlayer, 2, false);

            }
            else
            {
                _model.ChangePlayer();
                valtozo = false;
            }

        }

        [TestMethod]
        public void AwariGameModelStonepackingScoreCup()
        {
            //Testing if you score your last in your own you get your and the enemy's cup's score into your own scorecup.
            _model.NewGame();
            _model.Table.SetValue(0, 0);
            _model.Table.SetValue(1, 1);
            _model.Table.SetValue(2, 0);
            _model.Table.SetValue(3, 4);
            _model.Table.SetValue(4, 5);
            _model.Table.SetValue(5, 9);
            _model.Table.SetValue(6, 1);
            _model.Table.SetValue(7, 6);
            _model.Table.SetValue(8, 8);
            _model.Table.SetValue(9, 6);
            _model.StonePacking(_model.CurrentPlayer, 1, true);
            Assert.AreEqual(_model.Table.GetValue(0), 0);
            Assert.AreEqual(_model.Table.GetValue(1), 0);
            Assert.AreEqual(_model.Table.GetValue(2), 0);
            Assert.AreEqual(_model.Table.GetValue(3), 4);
            Assert.AreEqual(_model.Table.GetValue(4), 7);
            Assert.AreEqual(_model.Table.GetValue(5), 9);
            Assert.AreEqual(_model.Table.GetValue(6), 0);
            Assert.AreEqual(_model.Table.GetValue(7), 6);
            Assert.AreEqual(_model.Table.GetValue(8), 8);
            Assert.AreEqual(_model.Table.GetValue(9), 6);
        }
    } 
}
