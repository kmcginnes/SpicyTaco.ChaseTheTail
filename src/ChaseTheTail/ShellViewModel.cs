using System;
using System.Reactive.Linq;
using Caliburn.Micro.ReactiveUI;
using Microsoft.Win32;
using Platform.VirtualFileSystem;
using ReactiveUI;

namespace ChaseTheTail
{
    public class ShellViewModel : ReactiveConductor<DocumentCollectionViewModel>, IShell
    {
        public ShellViewModel(
            IFileSystemManager fileSystem,
            Lazy<DocumentCollectionViewModel> documentCollection)
        {
            _fileSystem = fileSystem;
            _documentCollection = documentCollection;

            OpenDocument = ReactiveCommand.Create();
            OpenDocument
                .Select(_ => new OpenFileDialog())
                .Where(x => x.ShowDialog().GetValueOrDefault())
                .SelectMany(x => x.FileNames)
                .Select(x => _fileSystem.ResolveFile(x))
                .Subscribe(x => DocumentCollection.OpenDocument(x));
        }

        protected override void OnInitialize()
        {
            DisplayName = "Chase the Tail";
            ActivateItem(DocumentCollection);
        }

        public ReactiveCommand<object> OpenDocument { get; private set; }

        readonly IFileSystemManager _fileSystem;
        readonly Lazy<DocumentCollectionViewModel> _documentCollection;
        public DocumentCollectionViewModel DocumentCollection { get { return _documentCollection.Value; } }
    }
}