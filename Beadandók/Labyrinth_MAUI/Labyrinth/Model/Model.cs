using Labyrinth.Model;
using Labyrinth.Persistence;
using System.IO;

namespace Labyrinth.Model
{
    public class MainModel : IDisposable
    {

        //Progress and setting variables
        private int _time;
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
        public event EventHandler<SaveEventArgs>? SaveMessage;


        //Getters
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

        public int GetExitX()
        {
            return _exitX;
        }

        public int GetExitY()
        {
            return _exitY;
        }

        public bool GetGameEnd()
        {
            return _isEnd;
        }

        public List<Wall>? GetWalls()
        {
            return _walls;
        }

        //Model constructor
        public MainModel(IPersistence persistence)
        {
            _per = persistence;
        }

        //New game method
        public async void NewGame(int size, Stream stream)
        {
            _time = 0;
            _isPaused = false;
            _isEnd = false;
            _size = size;
            _gameTable = new int[size, size];


            _walls = new List<Wall>();
            int[,]? map = size switch
            {
                3 => await _per.ReadMap(size, stream),
                10 => await _per.ReadMap(size, stream),
                15 => await _per.ReadMap(size, stream),
                20 => await _per.ReadMap(size, stream),
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

            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, CType.Player));
            RefreshCell?.Invoke(this, new RefreshCellEventArgs(_exitX, _exitY, CType.Exit));
        }

        ////Save and load game methods
        //public void SaveGame()
        //{
        //    if (_isPaused)
        //    {
        //        _per.SaveGame(Path, _time, _playerX, _playerY, _size, _gameTable, _walls);
        //        SaveMessage?.Invoke(this, new SaveEventArgs("Successful saving!"));
        //    }
        //    else
        //    {
        //        SaveMessage?.Invoke(this, new SaveEventArgs("Game can not be saved! \n It is not paused!"));
        //    }

        //}

        //public void LoadGame()
        //{
        //    _per.LoadGame(Path, out int time, out int playerX, out int playerY, out int size, out int[,]? gameTable, out List<Wall>? walls);

        //    NewGame(size);

        //    _isPaused = false;
        //    _isEnd = false;
        //    _time = time;
        //    _size = size;

        //    _playerX = playerX;
        //    _playerY = playerY;

        //    _walls = walls;


        //    RefreshCell?.Invoke(this, new RefreshCellEventArgs(0, _size - 1, CType.Empty));
        //    Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
        //    RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, CType.Player));
        //    RefreshCell?.Invoke(this, new RefreshCellEventArgs(_exitX, _exitY, CType.Exit));
        //}

        //Pause method
        public void PauseGame()
        {
            _isPaused = !_isPaused;
            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
        }

        //Timer method
        public void OnTick()
        {
            _time++;
            Refresh?.Invoke(this, new RefreshEventArgs(_size, _gameTable, _walls));
            if (_time == 1)
            {
                RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, CType.Player));
            }
        }

        //Move player method
        public void MovePlayer(Direction direction)
        {
            if (_isEnd || _isPaused) return;
            int nextX = 0, nextY = 0;
            switch (direction)
            {
                case Direction.Up:
                    nextX = -1;
                    nextY = 0;
                    break;
                case Direction.Down:
                    nextX = 1;
                    nextY = 0;
                    break;
                case Direction.Left:
                    nextX = 0;
                    nextY = -1;
                    break;
                case Direction.Right:
                    nextX = 0;
                    nextY = 1;
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
                    RefreshCell?.Invoke(this, new RefreshCellEventArgs(nextPlayerX, nextPlayerY, CType.Player));
                    RefreshCell?.Invoke(this, new RefreshCellEventArgs(_playerX, _playerY, CType.Empty));

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

        //Game over method
        private void GameOver(string m)
        {
            _isEnd = true;
            GameEnd?.Invoke(this, new GameEndEventArgs(m));
        }

        //Dispose method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
