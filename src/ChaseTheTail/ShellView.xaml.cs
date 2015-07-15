using System;
using System.Linq.Expressions;
using System.Windows;
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
