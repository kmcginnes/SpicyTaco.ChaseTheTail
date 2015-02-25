using System.IO;
using Caliburn.Micro.ReactiveUI;
using Platform.VirtualFileSystem;
using ReactiveUI;

namespace ChaseTheTail
{
    public class DocumentViewModel : ReactiveScreen
    {
        readonly IFile _file;

        public DocumentViewModel(IFile file)
        {
            _file = file;
            DisplayName = file.Name;
        }

        protected override async void OnInitialize()
        {
            string content;
            using (var reader = _file.GetContent().GetReader(FileShare.ReadWrite))
            {
                content = await reader.ReadToEndAsync();
            }
            Content = content;
        }

        string _content;
        public string Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }
    }
}