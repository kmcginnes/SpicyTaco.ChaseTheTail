using System.Windows;
using ReactiveUI;

namespace ChaseTheTail
{
    public partial class DocumentCollectionView : IViewFor<DocumentCollectionViewModel>
    {
        public DocumentCollectionView()
        {
            InitializeComponent();

            this.Bind(ViewModel, vm => vm.ReactiveItems, v => v.Documents.ItemsSource);
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
