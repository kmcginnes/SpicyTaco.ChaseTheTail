using System;
using Caliburn.Micro.ReactiveUI;
using Platform.VirtualFileSystem;

namespace ChaseTheTail
{
    public class DocumentCollectionViewModel : ReactiveConductor<DocumentViewModel>.Collection.OneActive
    {
        readonly Func<IFile, DocumentViewModel> _createDocument;

        public DocumentCollectionViewModel(Func<IFile, DocumentViewModel> createDocument)
        {
            _createDocument = createDocument;
        }

        public void OpenDocument(IFile file)
        {
            var newDocument = _createDocument(file);
            Items.Add(newDocument);
            ActivateItem(newDocument);
        }
    }
}
