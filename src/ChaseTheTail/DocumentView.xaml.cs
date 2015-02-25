using System;
using System.Windows;
using ReactiveUI;

namespace ChaseTheTail
{
    public partial class DocumentView : IViewFor<DocumentViewModel>
    {
        public DocumentView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Content, v => v.Content.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DocumentViewModel) value; }
        }

        public DocumentViewModel ViewModel
        {
            get { return (DocumentViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof (DocumentViewModel), typeof (DocumentView));

    }
}
