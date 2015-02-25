using Caliburn.Micro.ReactiveUI;

namespace ChaseTheTail
{
    public class ShellViewModel : ReactiveScreen, IShell
    {
        protected override void OnInitialize()
        {
            DisplayName = "Chase the Tail";
        }
    }
}