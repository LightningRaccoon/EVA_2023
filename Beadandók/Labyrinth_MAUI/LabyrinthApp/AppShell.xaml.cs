using System.Runtime.CompilerServices;
using Labyrinth.Model;
using Labyrinth.Persistence;
using Labyrinth.View;
using Labyrinth.ViewModel;

namespace Labyrinth
{
    public partial class AppShell : Shell
    {

        private IPersistence _persistence;
        private readonly MainModel _model;
        private readonly LabyrinthViewModel _viewModel;

        private readonly IDispatcherTimer _timer;

        public AppShell(IPersistence persistence, MainModel mainModel, LabyrinthViewModel viewModel)
        {
            InitializeComponent();

        #if WINDOWS
            Items.Add(new ShellItem
            {
                Route = "MainPage",
                Items = 
                {
                    new ShellContent
                    {
                        Title = "Labyrinth Windows",
                        ContentTemplate = new DataTemplate(typeof(MainPage))
                    }
                }
            });
        #elif ANDROID
            Items.Add(new ShellItem
            {
                Route = "MainPage",
                Items = 
                {
                    new ShellContent
                    {
                        Title = "Labyrinth Android",
                        ContentTemplate = new DataTemplate(typeof(MainPageAndroid))
                    }
                }
            });
        #endif

            _persistence = persistence;
            _model = mainModel;
            _viewModel = viewModel;

            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (_, _) => _model.OnTick();

            _model.GameEnd += ViewModel_GameOver;

            _viewModel.NewEasyGame += ViewModel_NewEasyGame;
            _viewModel.NewMediumGame += ViewModel_NewMediumGame;
            _viewModel.NewHardGame += ViewModel_NewHardGame;
            _viewModel.PauseGame += ViewModel_PauseGame;
            //_viewModel.LoadGame += ViewModel_LoadGame;
            //_viewModel.SaveGame += ViewModel_SaveGame;
            _viewModel.ExitGame += ViewModel_ExitGame;
            //_viewModel.GameOver += ViewModel_GameOver;



        }

        internal void StartTimer() => _timer.Start();
        internal void StopTimer() => _timer.Stop();

        private void ViewModel_NewEasyGame(object? sender, EventArgs e)
        {
            var stream = FileSystem.Current.OpenAppPackageFileAsync("map10.txt").Result;
            _model.NewGame(10, stream);
            _viewModel.SetupTable();
            StartTimer();
        }

        private void ViewModel_NewMediumGame(object? sender, EventArgs e)
        {
            var stream = FileSystem.Current.OpenAppPackageFileAsync("map15.txt").Result;
            _model.NewGame(15, stream);
            _viewModel.SetupTable();
            StartTimer();
        }

        private void ViewModel_NewHardGame(object? sender, EventArgs e)
        {
            var stream = FileSystem.Current.OpenAppPackageFileAsync("map20.txt").Result;
            _model.NewGame(20, stream);
            _viewModel.SetupTable();
            StartTimer();
        }
        internal void ViewModel_PauseGame(object? sender, EventArgs? e)
        {
            if (_model.GetPause() && !_model.GetGameEnd())
            {
                StartTimer();
                _model.PauseGame();
            }
            else
            {
                StopTimer();
                _model.PauseGame();
            }
        }
        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            StopTimer();
            Application.Current?.Quit();
        }

        private async void ViewModel_GameOver(object? sender, GameEndEventArgs e)
        {
            StopTimer();
            await DisplayAlert("End game", e.EndGameMessage, "OK");
        }
    }
}
