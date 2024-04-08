using Labyrinth_game.Model;
using Labyrinth_game.Persistence;
using Moq;

namespace GameTest
{
    [TestClass]
    public class LabyrinthTest : IDisposable
    {
        private Mock<IPersistence>? _mock;
        private MainModel? _model;


        [TestInitialize]
        public void Initialize()
        {

            _mock = new Mock<IPersistence>();
            _mock.Setup(p => p.ReadMap(It.IsAny<string>(), 3)).Returns(new int[3, 3]
            {
                { 1, 0, 3 },
                { 1, 0, 1 },
                { 2, 0, 1 }
            });

            _mock.Setup(p => p.ReadMap(It.IsAny<string>(), 10)).Returns(new int[10, 10]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            });
            _mock.Setup(p => p.ReadMap(It.IsAny<string>(), 15)).Returns(new int[15, 15]
            {
                { 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 3 },
                { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0 },
                { 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1 },
                { 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            });
            _mock.Setup(p => p.ReadMap(It.IsAny<string>(), 20)).Returns(new int[20, 20]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 ,1 ,1 ,1 ,1 ,0 ,0 ,1 ,1 ,1 ,0 },
                { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 },
                { 1, 0, 0, 1, 1, 1, 1, 0, 0, 1 ,1 ,1 ,0 ,0 ,0 ,1 ,0 ,1 ,1 ,0 },
                { 1, 1, 0, 1, 0, 0, 0, 0, 1, 1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 },
                { 1, 1, 0, 1, 0, 1, 1, 0, 1, 1 ,0 ,1 ,1 ,1 ,0 ,1 ,1 ,1 ,0 ,1 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
                { 1, 0, 0, 1, 0, 0, 1, 1, 1, 1 ,1 ,1 ,1 ,1 ,0 ,1 ,1 ,1 ,1 ,1 },
                { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,0 ,1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 ,1 ,1 ,1 ,0 ,0 ,0 ,1 ,1 ,1 ,0 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,0 ,0 ,1 ,1 ,0 ,0 ,0 ,0 ,1 ,0 },
                { 1, 0, 0, 1, 0, 0, 1, 1, 1, 0 ,1 ,1 ,1 ,1 ,0 ,1 ,1 ,1 ,1 ,0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 0, 1 ,1 ,1 ,1 ,0 ,0 ,1 ,1 ,1 ,1 ,0 },
                { 1, 1, 1, 0, 0, 0, 0, 1, 0, 1 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,0 },
                { 1, 0, 0, 0, 1, 1, 0, 1, 0, 1 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,0 },
                { 2, 0, 1, 1, 1, 1, 1, 1, 0, 1 ,1 ,1 ,1 ,0 ,0 ,1 ,0 ,1 ,0 ,1 }
            });
            _model = new MainModel(_mock.Object);
        }

        [TestMethod]
        public void NewGameTest1()
        {
            _model?.NewGame(10);

            if (_model == null) return;
            Assert.AreEqual(0, _model.GetTime());
            Assert.AreEqual(false, _model.GetGameEnd());
            Assert.AreEqual(false, _model.GetPause());
            Assert.AreEqual(10, _model.GetSize());
            Assert.AreEqual(0, _model.GetPlayerX());
            Assert.AreEqual(9, _model.GetPlayerY());
            Assert.AreEqual(0, _model.GetTime());
            Assert.IsNotNull(_model.GetWalls());
        }

        [TestMethod]
        public void NewGameTest2()
        {
            _model?.NewGame(15);

            if (_model == null) return;
            Assert.AreEqual(0, _model.GetTime());
            Assert.AreEqual(false, _model.GetGameEnd());
            Assert.AreEqual(false, _model.GetPause());
            Assert.AreEqual(15, _model.GetSize());
            Assert.AreEqual(0, _model.GetPlayerX());
            Assert.AreEqual(14, _model.GetPlayerY());
            Assert.AreEqual(0, _model.GetTime());
            Assert.IsNotNull(_model.GetWalls());
        }

        [TestMethod]
        public void NewGameTest3()
        {
            _model?.NewGame(20);

            if (_model == null) return;
            Assert.AreEqual(0, _model.GetTime());
            Assert.AreEqual(false, _model.GetGameEnd());
            Assert.AreEqual(false, _model.GetPause());
            Assert.AreEqual(20, _model.GetSize());
            Assert.AreEqual(0, _model.GetPlayerX());
            Assert.AreEqual(19, _model.GetPlayerY());
            Assert.AreEqual(0, _model.GetTime());
            Assert.IsNotNull(_model.GetWalls());
        }

        [TestMethod]
        public void PlayerMovementTest()
        {
            _model?.NewGame(10);

            if (_model == null) return;
            _model.MovePlayer(Direction.Up);
            Assert.AreEqual(_model.GetPlayerX(), 0);
            Assert.AreEqual(_model.GetPlayerY(), 9);

            _model.MovePlayer(Direction.Right);
            Assert.AreEqual(_model.GetPlayerX(), 1);
            Assert.AreEqual(_model.GetPlayerY(), 9);

            _model.MovePlayer(Direction.Left);
            Assert.AreEqual(_model.GetPlayerX(), 0);
            Assert.AreEqual(_model.GetPlayerY(), 9);

            _model.MovePlayer(Direction.Down);
            Assert.AreEqual(_model.GetPlayerX(), 0);
            Assert.AreEqual(_model.GetPlayerY(), 9);

            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Up);
            Assert.AreEqual(_model.GetPlayerX(), 1);
            Assert.AreEqual(_model.GetPlayerY(), 8);
        }

        [TestMethod]
        public void GameEndTest()
        {
            Assert.IsTrue(_model != null && _model.GetGameEnd());
        }

        [TestMethod]
        public void TimerTest()
        {
            _model?.NewGame(10);

            if (_model == null) return;
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            _model.OnTick(this, null);
            Assert.AreEqual(8, _model.GetTime());
        }

        [TestMethod]
        public void PauseTest()
        {
            _model?.NewGame(10);

            if (_model == null) return;
            _model.OnTick(this, null);
            _model.PauseGame();
            Assert.AreEqual(true, _model.GetPause());
            _model.PauseGame();
            Assert.AreEqual(false, _model.GetPause());
        }

        [TestMethod]
        public void EndGameTest()
        {
            _model?.NewGame(3);

            if (_model == null) return;
            _model.MovePlayer(Direction.Up);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Up);

            Assert.AreEqual(true, _model.GetGameEnd());
        }

        [TestMethod]
        public void HitWallTest()
        {
            _model?.NewGame(3);

            if (_model == null) return;
            _model.MovePlayer(Direction.Up);
            _model.MovePlayer(Direction.Up);
            _model.MovePlayer(Direction.Up);
            _model.MovePlayer(Direction.Up);

            Assert.AreEqual(1, _model.GetPlayerY());
        }

        public void Dispose()
        {
            _model?.Dispose();
        }
    }
}