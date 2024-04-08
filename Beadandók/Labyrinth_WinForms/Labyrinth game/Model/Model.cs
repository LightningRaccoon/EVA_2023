using Labyrinth_game.Persistence;
using Timer = System.Windows.Forms.Timer;

namespace Labyrinth_game.Model
{
    public enum Type { Player, Wall, Empty, Exit }

    public enum Direction { Up, Right, Down, Left };

    public class Wall
    {
        public int X { get; }
        public int Y { get; }

        public Wall(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Wall()
        {
            X = 0;
            Y = 0;
        }
    }

    public class RefreshEventArgs : EventArgs
    {
        public RefreshEventArgs(int size, int[,]? gameTable, List<Wall>? walls)
        {
            TableSize = size;
            GameTable = gameTable;
            Walls = walls;
        }

        public int TableSize { get; }
        public int[,]? GameTable { get; }
        public List<Wall>? Walls { get; }
    }
    public class RefreshCellEventArgs : EventArgs
    {
        public RefreshCellEventArgs(int x, int y, Type t)
        {
            CellX = x;
            CellY = y;
            CellType = t;
        }

        public int CellX { get; }
        public int CellY { get; }
        public Type CellType { get; }
    }
    public class GameEndEventArgs : EventArgs
    {
        public GameEndEventArgs(string msg)
        {
            EndGameMessage = msg;
        }
        public string EndGameMessage { get; private set; }
    }
    public class RefreshTextEventArgs : EventArgs
    {
        public RefreshTextEventArgs(int elapsedTime)
        {
            ElapsedTime = elapsedTime;
        }
        public int ElapsedTime { get; }
    }
    public class SaveEventArgs : EventArgs
    {
        public SaveEventArgs(string msg)
        {
            SaveFailedMessage = msg;
        }
        public string SaveFailedMessage { get; }
    }

    public class OnNewGameEventArgs : EventArgs
    {
        public OnNewGameEventArgs(int size)
        {
            Size = size;
        }
        public int Size { get; }
    }


    public class MainModel : IDisposable
    {
        //Map and save file path
        private const string Path = "..\\..\\..\\Assets\\save.txt";
        private const string Map3Path = "..\\..\\..\\Assets\\Maps\\map3Test.txt";
        private const string Map10Path = "..\\..\\..\\Assets\\Maps\\map10.txt";
        private const string Map15Path = "..\\..\\..\\Assets\\Maps\\map15.txt";
        private const string Map20Path = "..\\..\\..\\Assets\\Maps\\map20.txt";

        //Progress and setting variables
        private int _time;
        private readonly Timer _timer;
        private bool _isPaused;
        private bool _isEnd = true;
        private int _size;
        private int[,]? _gameTable;
        private List<Wall>? _walls;
        private int _playerX;
        private int _playerY;
        private int _exitX;
        private int _exitY;
        private readonly IPersistence _per;

        //Event handlers
        public event EventHandler<RefreshEventArgs>? Refresh;
        public event EventHandler<RefreshCellEventArgs>? RefreshCell;
        public event EventHandler<GameEndEventArgs>? GameEnd;
        public event EventHandler<RefreshTextEventArgs>? RefreshText;
        public event EventHandler<SaveEventArgs>? SaveMessage;
        public event EventHandler<OnNewGameEventArgs>? OnNewGame;

        #region Getters
        public int GetTime()
        {
            return _time;
        }

        public bool GetPause()
        {
            return _isPaused;
        }

        public int GetPlayerX()
        {
            return _playerX;
        }

        public int GetPlayerY()
        {
            return _playerY;
        }

        public int GetSize()
        {
            return _size;
        }

        public List<Wall>? GetWalls()
        {
            return _walls;
        }

        public bool GetGameEnd()
        {
            return _isEnd;
        }

        #endregion

        #region GameModel
        public MainModel(IPersistence persistence)
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += OnTick;

            _per = persistence;
        }
        #endregion

        #region newGame
        public void NewGame(int size)
        {
            _time = 0;
            _isPaused = false;
            _isEnd = false;
            _size = size;
            _gameTable = new int[size, size];


            _walls = new List<Wall>();
            int[,]? map = size switch
            {
                3 => _per.ReadMap(Map3Path, size),
                10 => _per.ReadMap(Map10Path, size),
                15 => _per.ReadMap(Map15Path, size),
                20 => _per.ReadMap(Map20Path, size),
                _ => null
            };
            _gameTable = map;

            if (_gameTable == null) return;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_gameTable[i, j] == 1)
                    {
                        _walls.Add(new Wall(i, j));
                    }
                    else if (_gameTable[i, j] == 3)
                    {
                        _playerX = i;
                        _playerY = j;
                    }
                    else if (_gameTable[i, j] == 2)
                    {
                        _exitX = i;
                        _exitY = j;
                    }
                }
            }

            OnNewGame?.Invoke(this, new OnNewGameEventArgs(size));
            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, Type.Player));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_exitX, _exitY, Type.Exit));
            _timer.Start();

        }
        #endregion

        #region saveGame

        public void SaveGame()
        {
            if (_isPaused)
            {
                _per.SaveGame(Path, _time, _playerX, _playerY, _size, _gameTable, _walls);
                SaveMessage?.Invoke(this, new SaveEventArgs("Successful saving!"));
            }
            else
            {
                SaveMessage?.Invoke(this, new SaveEventArgs("Game can not be saved! \n It is not paused!"));
            }
           
        }

        #endregion

        #region loadGame
        public void LoadGame()
        {
            _per.LoadGame(Path, out int time, out int playerX, out int playerY, out int size, out int[,]? gameTable, out List<Wall>? walls);

            NewGame(size);

            _isPaused = false;
            _isEnd = false;
            _time = time;
            _size = size;
            //_gameTable = gameTable;

            _playerX = playerX;
            _playerY = playerY;

            _walls = walls;

            RefreshCell?.Invoke(this, new RefreshCellEventArgs(0, _size - 1, Type.Empty));
            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, Type.Player));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_exitX, _exitY, Type.Exit));
            _timer.Start();
        }
        #endregion

        #region pauseGame
        public void PauseGame()
        {
            if (_isPaused)
            {
                _isPaused = false;
                _timer.Start();
            }
            else
            {
                _isPaused = true;
                _timer.Stop();
            }
        }
        #endregion

        #region onTick
        public void OnTick(object? o, EventArgs? e)
        {
            _time++;
            RefreshText?.Invoke(this, new RefreshTextEventArgs(_time));
            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, Type.Player));
        }
        #endregion

        #region movePlayer
        public void MovePlayer(Direction direction)
        {
            if (_isEnd || _isPaused) return;
            int nextX = 0, nextY = 0;
            switch (direction)
            {
                case Direction.Up:
                    nextX = 0;
                    nextY = -1;
                    break;
                case Direction.Down:
                    nextX = 0;
                    nextY = 1;
                    break;
                case Direction.Left:
                    nextX = -1;
                    nextY = 0;
                    break;
                case Direction.Right:
                    nextX = 1;
                    nextY = 0;
                    break;

            }
            int nextPlayerX = _playerX + nextX, nextPlayerY = _playerY + nextY;

            bool collision = false;
            if (_walls != null)
                foreach (var w in _walls)
                {
                    if (nextPlayerX == w.X && nextPlayerY == w.Y)
                    {
                        collision = true;
                    }
                }

            if (!collision)
            {
                if (_gameTable != null && nextPlayerX >= 0 && nextPlayerY >= 0 && nextPlayerX < _size && nextPlayerY < _size && _gameTable[nextPlayerX, nextPlayerY] != 1)
                {
                    RefreshCell?.Invoke(this, new RefreshCellEventArgs(nextPlayerX, nextPlayerY, Type.Player));
                    RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, Type.Empty));

                    _playerX = nextPlayerX;
                    _playerY = nextPlayerY;

                    Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
                }
            }

            if (nextPlayerX == _exitX && nextPlayerY == _exitY)
            {
                GameOver("WIN - You have found the exit!");
            }


        }
        #endregion

        #region gameOver
        private void GameOver(string m)
        {
            _isEnd = true;
            _timer.Stop();

            //invoke game end
            GameEnd?.Invoke(this, new GameEndEventArgs(m));
        }
        #endregion

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
