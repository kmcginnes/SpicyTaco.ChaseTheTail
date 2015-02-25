using System;
using System.Reactive.Linq;
using Caliburn.Micro.ReactiveUI;
using Platform.VirtualFileSystem;
using ReactiveUI;

namespace ChaseTheTail
{
    public class DocumentViewModel : ReactiveScreen
    {
        public DocumentViewModel(IFile file)
        {
            DisplayName = file.Name;
            Content = string.Empty;

            var tailService = new TailService();
            _subscription = tailService.Tail(file)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => Content += String.Format("{0}{1}", x, Environment.NewLine));
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _subscription.Dispose();
            }
        }

        readonly IDisposable _subscription;

        string _content;
        public string Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }
    }
}