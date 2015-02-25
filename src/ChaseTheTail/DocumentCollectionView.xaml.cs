using System.Windows;
using ReactiveUI;

namespace ChaseTheTail
{
    public partial class DocumentCollectionView : IViewFor<DocumentCollectionViewModel>
    {
        public DocumentCollectionView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Items, v => v.Documents.ItemsSource);
            this.Bind(ViewModel, vm => vm.ActiveItem, v => v.Documents.SelectedItem);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DocumentCollectionViewModel) value; }
        }

        public DocumentCollectionViewModel ViewModel
        {
            get { return (DocumentCollectionViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof (DocumentCollectionViewModel), typeof (DocumentCollectionView));

    }
}
