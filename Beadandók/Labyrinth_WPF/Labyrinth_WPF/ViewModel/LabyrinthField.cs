using System;


namespace Labyrinth_WPF.ViewModel
{
    public class LabyrinthField : ViewModelBase
    {
        private String _text = String.Empty;
        private Model.CType _type;

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public Model.CType CType
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;

                Text = _type switch
                {
                    Model.CType.Player => "P",
                    Model.CType.Wall => "W",
                    Model.CType.Empty => "E",
                    Model.CType.Exit => "X",
                    Model.CType.Unknown => "U",
                    _ => Text
                };
                OnPropertyChanged();
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public DelegateCommand? StepCommand { get; set; }
    }
}
