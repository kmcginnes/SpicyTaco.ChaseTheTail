using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using ReactiveUI;

namespace ChaseTheTail
{
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, vm => vm.OpenDocument, v => v.OpenDocument);
            this.WhenAnyValue(x => x.ViewModel.DocumentCollection)
                .Subscribe(x => View.SetModel(Documents, x));


            this.Events().KeyUp
                .Where(x => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                .Where(x => x.Key == Key.O)
                .InvokeCommand(this, x => x.ViewModel.OpenDocument);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ShellViewModel) value; }
        }

        public ShellViewModel ViewModel
        {
            get { return (ShellViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof (ShellViewModel), typeof (ShellView));

    }
}
