using Labyrinth.Model;
using Labyrinth.Persistence;
using Labyrinth.ViewModel;

namespace Labyrinth
{
    public partial class App : IDisposable
    {
        private readonly AppShell _appShell;
        private readonly IPersistence _persistence;
        private readonly MainModel _model;
        private readonly LabyrinthViewModel _viewModel;


        public App()
        {
            InitializeComponent();

            _persistence = new Persistence.Persistence(FileSystem.Current.CacheDirectory);

            _model = new MainModel(_persistence);
            _viewModel = new LabyrinthViewModel(_model);

            _appShell = new AppShell(_persistence, _model, _viewModel)
            {
                BindingContext = _viewModel
            };

            MainPage = _appShell;
        }

        public void Dispose()
        {
            _persistence?.Dispose();
            _model?.Dispose();
        }
    }
}
