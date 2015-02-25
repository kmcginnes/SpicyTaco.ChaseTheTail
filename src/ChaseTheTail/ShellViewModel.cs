using System;
using System.Reactive.Linq;
using Caliburn.Micro.ReactiveUI;
using Microsoft.Win32;
using ReactiveUI;

namespace ChaseTheTail
{
    public class ShellViewModel : ReactiveScreen, IShell
    {
        public ShellViewModel()
        {
            OpenDocument = ReactiveCommand.Create();
            OpenDocument
                .Select(_ => new OpenFileDialog())
                .Where(x => x.ShowDialog().GetValueOrDefault())
                .SelectMany(x => x.FileNames)
                .Subscribe(x => { });
        }

        protected override void OnInitialize()
        {
            DisplayName = "Chase the Tail";
        }

        public ReactiveCommand<object> OpenDocument { get; private set; }
    }
}