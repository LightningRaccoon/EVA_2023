using System;
using System.Windows;
using Labyrinth_WPF.ViewModel;
using Labyrinth_WPF.Model;
using System.ComponentModel;
using System.Windows.Threading;
using Labyrinth_WPF.Persistence;
using Labyrinth_WPF.View;

namespace Labyrinth_WPF
{
    public partial class App : IDisposable
    {
        private MainModel _model = null!;
        private LabyrinthViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;


        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _model = new MainModel(new Persistence.Persistence());
            _model.GameEnd += Model_GameOver;

            _viewModel = new LabyrinthViewModel(_model);
            _viewModel.NewEasyGame += ViewModel_NewEasyGame;
            _viewModel.NewMediumGame += ViewModel_NewMediumGame;
            _viewModel.NewHardGame += ViewModel_NewHardGame;
            _viewModel.PauseGame += ViewModel_PauseGame;
            _viewModel.LoadGame += ViewModel_LoadGame;
            _viewModel.SaveGame += ViewModel_SaveGame;
            _viewModel.ExitGame += ViewModel_ExitGame;

            _view = new MainWindow
            {
                DataContext = _viewModel
            };
            _view.Closing += View_Closing;
            _view.Show();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.OnTick();
        }


        private void View_Closing(object? sender, CancelEventArgs e)
        {
            _timer.Stop();

            if (MessageBox.Show("Are you sure you want to exit?", "Labyrinth", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ViewModel_NewEasyGame(object? sender, EventArgs e)
        {
            _model.NewGame(10);
            _viewModel.SetupTable();
            _timer.Start();
        }

        private void ViewModel_NewMediumGame(object? sender, EventArgs e)
        {
            _model.NewGame(15);
            _viewModel.SetupTable();
            _timer.Start();
        }

        private void ViewModel_NewHardGame(object? sender, EventArgs e)
        {
            _model.NewGame(20);
            _viewModel.SetupTable();
            _timer.Start();
        }

        private void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            _timer.Stop();
            try
            {
                _model.LoadGame();
                _viewModel.SetupTable();
                _timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Labyrinth game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _timer.Start();
            }
        }

        private void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            if (_model.GetPause())
            {
                _timer.Stop();

                try
                {
                    _model.SaveGame();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Labyrinth game", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    _timer.Start();
                }
            }
            else
            { 
                MessageBox.Show("You can only save the game when it is paused!", "Labyrinth game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {
            if (_model.GetPause())
            {
                _timer.Start();
                _model.PauseGame();
            }
            else
            {
                _timer.Stop();
                _model.PauseGame();
            }
        }

        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            _view.Close();
        }

        private void Model_GameOver(object? sender, GameEndEventArgs e)
        {
            _timer.Stop();
            MessageBox.Show(e.EndGameMessage, "Labyrinth game", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
            _timer = null!;
            _view.Closing -= View_Closing;
            _view.Close();
            _view = null!;
            _viewModel.NewEasyGame -= ViewModel_NewEasyGame;
            _viewModel.NewMediumGame -= ViewModel_NewMediumGame;
            _viewModel.NewHardGame -= ViewModel_NewHardGame;
            _viewModel.PauseGame -= ViewModel_PauseGame;
            _viewModel.LoadGame -= ViewModel_LoadGame;
            _viewModel.SaveGame -= ViewModel_SaveGame;
            _viewModel.ExitGame -= ViewModel_ExitGame;
            _viewModel = null!;
            _model.GameEnd -= Model_GameOver;
            _model = null!;
        }
    }
}
