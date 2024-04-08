using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Model;

namespace Labyrinth.ViewModel
{
    public class LabyrinthViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public DelegateCommand NewEasyGameCommand { get; private set; }
        public DelegateCommand NewMediumGameCommand { get; private set; }
        public DelegateCommand NewHardGameCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand UpCommand { get; private set; }
        public DelegateCommand DownCommand { get; private set; }
        public DelegateCommand LeftCommand { get; private set; }
        public DelegateCommand RightCommand { get; private set; }
        public ObservableCollection<LabyrinthField> Fields { get; set; }

        #region Events

        public event EventHandler? NewEasyGame;
        public event EventHandler? NewMediumGame;
        public event EventHandler? NewHardGame;
        public event EventHandler? PauseGame;
        public event EventHandler? ExitGame;


        #endregion

        public String GameTime => (_model.GetTime()).ToString();

        public Boolean IsPaused => _model.GetPause();


        public RowDefinitionCollection GameTableRows
        {
            get => new(Enumerable.Repeat(new RowDefinition(GridLength.Star), _model.GetSize()).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), _model.GetSize()).ToArray());
        }

        public LabyrinthViewModel(MainModel model)
        {
            _model = model;


            _model.Refresh += Refresh;
            _model.RefreshCell += RefreshCell;
            //_model.SaveMessage += SaveMessage;


            //Delegate Commands
            NewEasyGameCommand = new DelegateCommand(_ => OnNewEasyGame());
            NewMediumGameCommand = new DelegateCommand(_ => OnNewMediumGame());
            NewHardGameCommand = new DelegateCommand(_ => OnNewHardGame());
            PauseCommand = new DelegateCommand(_ => OnPauseGame());
            ExitCommand = new DelegateCommand(_ => OnExitGame());
            UpCommand = new DelegateCommand(_ => OnMovePlayer(Direction.Up));
            DownCommand = new DelegateCommand(_ => OnMovePlayer(Direction.Down));
            LeftCommand = new DelegateCommand(_ => OnMovePlayer(Direction.Left));
            RightCommand = new DelegateCommand(_ => OnMovePlayer(Direction.Right));


            Fields = new ObservableCollection<LabyrinthField>();
        }

        public void SetupTable()
        {
            Fields.Clear();
            var size = _model.GetSize();

            for (var i = 0; i < size; ++i)
            {
                for (var j = 0; j < size; ++j)
                {
                    Fields.Add(new LabyrinthField()
                    {
                        X = i,
                        Y = j,
                        Text = @" ",
                        CType = CType.Empty

                    });
                }
            }
            RefreshTable();
        }


        private void RefreshTable()
        {
            foreach (var field in Fields)
            {
                field.CType = CType.Empty;
            };

            OnPropertyChanged(nameof(GameTime));
            //TSize = _model.GetSize();
            OnPropertyChanged();
            OnPropertyChanged(nameof(GameTableColumns));
            OnPropertyChanged(nameof(GameTableRows));
        }

        private void Refresh(object? o, RefreshEventArgs e)
        {
            if (e.GameTable != null && (Fields.Count == 0 || Fields.Count != e.GameTable.Length)) return;

            if (e.GameTable == null) return;
            foreach (var field in Fields)
            {
                field.CType = CType.Empty;
            };

            if (e.Walls == null) return;
            foreach (var field in e.Walls.Select(wall => Fields.Single(f => f.X == wall.X && f.Y == wall.Y)))
            {
                field.CType = CType.Wall;
            }

            var pX = _model.GetPlayerX();
            var pY = _model.GetPlayerY();

            var playerField = Fields.Single(f => f.X == pX && f.Y == pY);
            playerField.CType = CType.Player;

            var exitX = _model.GetExitX();
            var exitY = _model.GetExitY();

            var exitField = Fields.Single(f => f.X == exitX && f.Y == exitY);
            exitField.CType = CType.Exit;

            for (var i = 0; i < e.TableSize; ++i)
            {
                for (var j = 0; j < e.TableSize; ++j)
                {
                    if (Math.Abs(_model.GetPlayerX() - i) < 3 && Math.Abs(_model.GetPlayerY() - j) < 3) continue;
                    if (Math.Abs(_model.GetExitX() - i) == 0 && Math.Abs(_model.GetExitY() - j) == 0) continue;
                    var field = Fields.Single(f => f.X == i && f.Y == j);
                    field.CType = CType.Unknown;
                }
            }


            if (pX < e.TableSize - 2 &&
                pY < e.TableSize - 2)
            {
                if (e.GameTable[pX + 1, pY + 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX + 2 && f.Y == pY + 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX + 1 && f.Y == pY + 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX + 2 && f.Y == pY + 1);
                    field.CType = CType.Unknown;
                }
            }
            if (pX >= 2 &&
                pY < e.TableSize - 2)
            {
                if (e.GameTable[pX - 1, pY + 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX - 2 && f.Y == pY + 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX - 1 && f.Y == pY + 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX - 2 && f.Y == pY + 1);
                    field.CType = CType.Unknown;
                }
            }
            if (pX < e.TableSize - 2 &&
                pY >= 2)
            {
                if (e.GameTable[pX + 1, pY - 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX + 2 && f.Y == pY - 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX + 1 && f.Y == pY - 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX + 2 && f.Y == pY - 1);
                    field.CType = CType.Unknown;
                }

            }
            if (pX >= 2 &&
                pY >= 2)
            {
                if (e.GameTable[pX - 1, pY - 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX - 2 && f.Y == pY - 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX - 1 && f.Y == pY - 2);
                    field.CType = CType.Unknown;
                    field = Fields.Single(f => f.X == pX - 2 && f.Y == pY - 1);
                    field.CType = CType.Unknown;
                }
            }
            if (pX < e.TableSize - 2)
            {
                if (e.GameTable[pX + 1, pY] == 1)
                {
                    var field = Fields.Single(f => f.X == pX + 2 && f.Y == pY);
                    field.CType = CType.Unknown;

                    if (pY < e.TableSize - 1)
                    {
                        field = Fields.Single(f => f.X == pX + 2 && f.Y == pY + 1);
                        field.CType = CType.Unknown;
                    }

                    if (pY >= 1)
                    {
                        field = Fields.Single(f => f.X == pX + 2 && f.Y == pY - 1);
                        field.CType = CType.Unknown;
                    }
                }
            }
            if (pX >= 2)
            {
                if (e.GameTable[pX - 1, pY] == 1)
                {
                    var field = Fields.Single(f => f.X == pX - 2 && f.Y == pY);
                    field.CType = CType.Unknown;
                    if (pY < e.TableSize - 1)
                    {
                        field = Fields.Single(f => f.X == pX - 2 && f.Y == pY + 1);
                        field.CType = CType.Unknown;
                    }

                    if (pY >= 1)
                    {
                        field = Fields.Single(f => f.X == pX - 2 && f.Y == pY - 1);
                        field.CType = CType.Unknown;
                    }
                }
            }
            if (pY < e.TableSize - 2)
            {
                if (e.GameTable[pX, pY + 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX && f.Y == pY + 2);
                    field.CType = CType.Unknown;
                    if (pX < e.TableSize - 1)
                    {
                        field = Fields.Single(f => f.X == pX + 1 && f.Y == pY + 2);
                        field.CType = CType.Unknown;
                    }

                    if (pX >= 1)
                    {
                        field = Fields.Single(f => f.X == pX - 1 && f.Y == pY + 2);
                        field.CType = CType.Unknown;
                    }
                }
            }
            if (pY >= 2)
            {
                if (e.GameTable[pX, pY - 1] == 1)
                {
                    var field = Fields.Single(f => f.X == pX && f.Y == pY - 2);
                    field.CType = CType.Unknown;
                    if (pX < e.TableSize - 1)
                    {
                        field = Fields.Single(f => f.X == pX + 1 && f.Y == pY - 2);
                        field.CType = CType.Unknown;
                    }

                    if (pX >= 1)
                    {
                        field = Fields.Single(f => f.X == pX - 1 && f.Y == pY - 2);
                        field.CType = CType.Unknown;
                    }
                }
            }

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(IsPaused));
        }


        private void RefreshCell(object? o, RefreshCellEventArgs e)
        {
            if (Fields.Count == 0 || Fields.Count / 2 != _model.GetSize()) return;
            var field = Fields.Single(f => f.X == e.CellX && f.Y == e.CellY);
            switch (e.CellCType)
            {
                case CType.Empty:
                    field.CType = CType.Empty;
                    break;
                case CType.Wall:
                    field.CType = CType.Wall;
                    break;
                case CType.Player:
                    field.CType = CType.Player;
                    break;
                case CType.Exit:
                    field.CType = CType.Exit;
                    break;
            }
        }


        private void OnNewEasyGame()
        {
            NewEasyGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewMediumGame()
        {
            NewMediumGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewHardGame()
        {
            NewHardGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnMovePlayer(Direction direction)
        {
            _model.MovePlayer(direction);
        }
    }
}
