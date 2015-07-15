using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ReactiveUI;

namespace ChaseTheTail
{
    public partial class DocumentView : IViewFor<DocumentViewModel>
    {
        public DocumentView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Content, v => v.Content.Text);

            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            using (var stream = typeof(DocumentView).Assembly.GetManifestResourceStream("ChaseTheTail.LogFile.xshd"))
            {
                if (stream == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(stream))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting(
                "LogFile Highlighting", 
                new [] { ".log" }, 
                customHighlighting);

            this.SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);

            Content.SyntaxHighlighting = customHighlighting;
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
