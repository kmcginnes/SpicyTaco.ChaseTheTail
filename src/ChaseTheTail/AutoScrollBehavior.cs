using System;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;

namespace ChaseTheTail
{
    public static class AutoScrollBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled", typeof(bool), typeof(AutoScrollBehavior),
                new PropertyMetadata(false, OnScrollBehaviorChanged));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void OnScrollBehaviorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as TextEditor;
            if (control != null && e.NewValue is bool)
            {
                var enabled = (bool) (e.NewValue);
                if (enabled)
                {
                    control.TextChanged += OnTextChanging;
                }
                else
                {
                    control.TextChanged -= OnTextChanging;
                }
            }
        }

        private static void OnTextChanging(object sender, EventArgs eventArgs)
        {
            var control = sender as TextEditor;
            if (control != null)
            {
                control.ScrollToEnd();
            }
        }
    }
}
