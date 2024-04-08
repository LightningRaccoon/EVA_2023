using Labyrinth_game.Model;

namespace Labyrinth_game.View
{

    public partial class FormWindow : Form
    {
        private readonly TableLayoutPanel _tb;
        private Button[,]? _gameTable;
        private readonly MainModel _model;

        public FormWindow()
        {
            InitializeComponent();
            _tb = new TableLayoutPanel();
            Controls.Add(_tb);
            _tb.Dock = DockStyle.Fill;
            _tb.BackColor = Color.LightGray;
            _tb.BackgroundImage = Image.FromFile("..\\..\\..\\Assets\\45908.jpg");
            _tb.BackgroundImageLayout = ImageLayout.Zoom;
            _tb.Padding = new Padding(0, 30, 0, 0);
            _model = new MainModel(new Persistence.Persistence());
            KeyPreview = true;
            KeyDown += KeyPressed;

            _model.Refresh += Refresh;
            _model.RefreshCell += RefreshCell;
            _model.RefreshText += RefreshText;
            _model.GameEnd += GameEnd;
            _model.SaveMessage += SaveMessage;
            _model.OnNewGame += OnNewGame;
        }

        /* Setup table */

        #region SetupTable

        private void SetupTable(int size)
        {
            _gameTable = new Button[size, size];
            _tb.ColumnCount = size;
            _tb.RowCount = size;
            _tb.BackgroundImage = null;

            for (var i = 0; i < size; ++i)
            {
                for (var j = 0; j < size; ++j)
                {
                    _gameTable[i, j] = new Button
                    {
                        Size = new Size(30, 30),
                        Enabled = false,
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(1),
                        BackColor = Color.White
                    };
                    _tb.Controls.Add(_gameTable[i, j], i, j);
                }
            }
        }

        #endregion

        /* Keypress */

        #region Keypress

        private void KeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _model.MovePlayer(Direction.Up);
            }
            else if (e.KeyCode == Keys.S)
            {
                _model.MovePlayer(Direction.Down);
            }
            else if (e.KeyCode == Keys.A)
            {
                _model.MovePlayer(Direction.Left);
            }
            else if (e.KeyCode == Keys.D)
            {
                _model.MovePlayer(Direction.Right);
            }
        }

        #endregion

        /* Refresh Text */

        #region RefreshText

        private void RefreshText(object? sender, RefreshTextEventArgs e)
        {
            TimerLabel.Text = @"Elapsed Time: " + e.ElapsedTime + @"s";
        }

        #endregion

        /* Game End */

        #region EndGame

        private void GameEnd(object? sender, GameEndEventArgs e)
        {
            string caption = "Victory!";
            MessageBox.Show(e.EndGameMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion

        /* Refresh table */

        #region Refresh

        private void Refresh(object? o, RefreshEventArgs e)
        {
            if (_gameTable == null || e.GameTable == null) return;
            foreach (var b in _gameTable) b.BackColor = Color.White;

            if (e.Walls == null) return;
            foreach (var a in e.Walls.Where(_ => _gameTable != null))
            {

                _gameTable[a.X, a.Y].Text = @" ";
                _gameTable[a.X, a.Y].BackColor = e.GameTable[a.X, a.Y] switch
                {
                    1 => Color.Black,
                    2 => Color.Yellow,
                    _ => _gameTable[a.X, a.Y].BackColor
                };
            }

            var pX = _model.GetPlayerX();
            var pY = _model.GetPlayerY();

            if (_gameTable == null) return;
            for (var i = 0; i < e.TableSize; ++i)
            {
                for (var j = 0; j < e.TableSize; ++j)
                {
                    if (!(Math.Abs(_model.GetPlayerX() - i) < 3 && Math.Abs(_model.GetPlayerY() - j) < 3))
                    {
                        //_gameTable[i, j].Text = @"";
                        _gameTable[i, j].BackColor = Color.LightSteelBlue;
                    }
                }
            }


            if (_gameTable == null || e.GameTable == null) return;
            if (pX < e.TableSize - 2 &&
                pY < e.TableSize - 2)
            {
                if (e.GameTable[pX + 1, pY + 1] == 1)
                {
                    _gameTable[pX + 2, pY + 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX + 1, pY + 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX + 2, pY + 1].BackColor = Color.LightSteelBlue;
                }
            }

            if (pX >= 2 &&
                pY < e.TableSize - 2)
            {
                if (e.GameTable[pX - 1, pY + 1] == 1)
                {
                    _gameTable[pX - 2, pY + 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX - 1, pY + 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX - 2, pY + 1].BackColor = Color.LightSteelBlue;
                }
            }

            if (pX < e.TableSize - 2 &&
                pY >= 2)
            {
                if (e.GameTable[pX + 1, pY - 1] == 1)
                {
                    _gameTable[pX + 2, pY - 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX + 1, pY - 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX + 2, pY - 1].BackColor = Color.LightSteelBlue;
                }

            }

            if (pX >= 2 &&
                pY >= 2)
            {
                if (e.GameTable[pX - 1, pY - 1] == 1)
                {
                    _gameTable[pX - 2, pY - 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX - 1, pY - 2].BackColor = Color.LightSteelBlue;
                    _gameTable[pX - 2, pY - 1].BackColor = Color.LightSteelBlue;
                }
            }

            if (pX < e.TableSize - 2)
            {
                if (e.GameTable[pX + 1, pY] == 1)
                {
                    _gameTable[pX + 2, pY].BackColor = Color.LightSteelBlue;
                    if (pY < e.TableSize - 1)
                    {
                        _gameTable[pX + 2, pY + 1].BackColor = Color.LightSteelBlue;
                    }

                    if (pY >= 1)
                    {
                        _gameTable[pX + 2, pY - 1].BackColor = Color.LightSteelBlue;
                    }
                }
            }

            if (pX >= 2)
            {
                if (e.GameTable[pX - 1, pY] == 1)
                {
                    _gameTable[pX - 2, pY].BackColor = Color.LightSteelBlue;
                    if (pY < e.TableSize - 1)
                    {
                        _gameTable[pX - 2, pY + 1].BackColor = Color.LightSteelBlue;
                    }

                    if (pY >= 1)
                    {
                        _gameTable[pX - 2, pY - 1].BackColor = Color.LightSteelBlue;
                    }
                }
            }

            if (pY < e.TableSize - 2)
            {
                if (e.GameTable[pX, pY + 1] == 1)
                {
                    _gameTable[pX, pY + 2].BackColor = Color.LightSteelBlue;
                    if (pX < e.TableSize - 1)
                    {
                        _gameTable[pX + 1, pY + 2].BackColor = Color.LightSteelBlue;
                    }

                    if (pX >= 1)
                    {
                        _gameTable[pX - 1, pY + 2].BackColor = Color.LightSteelBlue;
                    }
                }
            }

            if (pY >= 2)
            {
                if (e.GameTable[pX, pY - 1] == 1)
                {
                    _gameTable[pX, pY - 2].BackColor = Color.LightSteelBlue;
                    if (pX < e.TableSize - 1)
                    {
                        _gameTable[pX + 1, pY - 2].BackColor = Color.LightSteelBlue;
                    }

                    if (pX >= 1)
                    {
                        _gameTable[pX - 1, pY - 2].BackColor = Color.LightSteelBlue;
                    }
                }
            }

        }

        #endregion

        /* Refresh Cell */

        #region RefreshCell

        private void RefreshCell(object? o, RefreshCellEventArgs e)
        {
            switch (e.CellType)
            {
                case Model.Type.Empty:
                    if (_gameTable != null)
                    {
                        _gameTable[e.CellX, e.CellY].Text = @" ";
                        _gameTable[e.CellX, e.CellY].BackColor = Color.White;
                        _gameTable[e.CellX, e.CellY].BackgroundImage = null;
                    }

                    break;
                case Model.Type.Wall:
                    if (_gameTable != null)
                    {
                        _gameTable[e.CellX, e.CellY].Text = @"W";
                        _gameTable[e.CellX, e.CellY].BackColor = Color.Black;
                        _gameTable[e.CellX, e.CellY].BackgroundImage = null;
                    }

                    break;
                case Model.Type.Player:
                    if (_gameTable != null)
                    {
                        _gameTable[e.CellX, e.CellY].Text = @"P";

                        var bit1 = new Bitmap("..\\..\\..\\Assets\\player.ico");
                        var bit2 = new Bitmap(bit1, 30, 30);
                        _gameTable[e.CellX, e.CellY].BackgroundImage = bit2;
                    }

                    break;
                case Model.Type.Exit:
                    if (_gameTable != null)
                    {
                        _gameTable[e.CellX, e.CellY].Text = @"";
                        _gameTable[e.CellX, e.CellY].BackColor = Color.Yellow;
                    }

                    break;
            }
        }

        #endregion

        /* Save Message */

        #region SaveMessage

        private void SaveMessage(object? o, SaveEventArgs e)
        {
            const string caption = "Saving failed";
            MessageBox.Show(e.SaveFailedMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        /* New Game Click */

        #region NewGameClick

        private void New_easy_game_Click(object sender, EventArgs e)
        {
            _tb.Controls.Clear();
            _gameTable = null;

            _model.NewGame(10);
        }

        private void New_medium_game_Click(object sender, EventArgs e)
        {
            _tb.Controls.Clear();
            _gameTable = null;

            _model.NewGame(15);
        }

        private void New_hard_game_Click(object sender, EventArgs e)
        {
            _tb.Controls.Clear();
            _gameTable = null;

            _model.NewGame(20);
        }

        #endregion

        /* Save Game Click */

        #region SaveGameClick

        private void Save_button_Click(object sender, EventArgs e)
        {
            //save();
            _model.SaveGame();
        }

        #endregion

        /* Load Game Click */

        #region LoadGameClick

        private void Load_button_Click(object sender, EventArgs e)
        {
            _tb.Controls.Clear();
            _gameTable = null;

            _model.LoadGame();
        }

        #endregion

        /* Pause Game Click */

        #region PauseGameClick

        private void Pause_button_Click(object sender, EventArgs e)
        {
            _model.PauseGame();
        }

        #endregion

        /* Exit Game Click */

        #region ExitGameClick

        private void Exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private void OnNewGame(object? sender, OnNewGameEventArgs e)
        {
            var size = e.Size;

            SetupTable(size);

            switch (size)
            {
                case 10:
                    if (Form.ActiveForm == null) return;
                    Form.ActiveForm.Size = new Size(338, 428);
                    Form.ActiveForm.AutoSize = false;
                    break;
                case 15:
                    if (Form.ActiveForm == null) return;
                    Form.ActiveForm.Size = new Size(338 + 5 * 32, 428 + 5 * 32);
                    Form.ActiveForm.AutoSize = false;
                    break;
                case 20:
                    if (Form.ActiveForm == null) return;
                    Form.ActiveForm.Size = new Size(338 + 10 * 32, 428 + 10 * 32);
                    Form.ActiveForm.AutoSize = false;
                    break;
            }
        }

    }
}