using System.Windows.Input;

namespace FormattinTextBoxBox
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Controls;

    public class FormattingTextBox : TextBox
    {
        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "FormattedText",
            typeof(string),
            typeof(FormattingTextBox),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty FormattedTextProperty = FormattedTextPropertyKey.DependencyProperty;

        private TextBlock whenNotFocused;

        public string FormattedText
        {
            get { return (string)this.GetValue(FormattedTextProperty); }
            private set { this.SetValue(FormattedTextPropertyKey, value); }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (this.whenNotFocused == null)
            {
                var scrollViewer = this.NestedChildren().OfType<ScrollViewer>().Single();
                this.whenNotFocused = new TextBlock { Margin = new Thickness(2, 0, 2, 0) };
                var formattedTextBinding = new Binding
                {
                    Path = new PropertyPath(FormattedTextProperty),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };
                BindingOperations.SetBinding(this.whenNotFocused, TextBlock.TextProperty, formattedTextBinding);

                var whenNotFocusedVisibility = new Binding
                {
                    Path = new PropertyPath(IsKeyboardFocusWithinProperty),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = HiddenWhenTrueConverter.Default
                };
                BindingOperations.SetBinding(this.whenNotFocused, TextBlock.VisibilityProperty, whenNotFocusedVisibility);

                var content = (UIElement)scrollViewer.Content;
                var grid = new Grid();
                scrollViewer.Content = grid;
                grid.Children.Add(this.whenNotFocused);
                grid.Children.Add(content);

                var whenFocusedVisibility = new Binding
                {
                    Path = new PropertyPath(IsKeyboardFocusWithinProperty),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = VisibleWhenTrueConverter.Default
                };
                BindingOperations.SetBinding(content, UIElement.VisibilityProperty, whenFocusedVisibility);
            }

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.FormattedText = $"#{this.Text}#";
            base.OnTextChanged(e);
        }

        private class VisibleWhenTrueConverter : IValueConverter
        {
             internal static readonly VisibleWhenTrueConverter Default = new VisibleWhenTrueConverter();

            private VisibleWhenTrueConverter()
            {
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool) value ? System.Windows.Visibility.Visible : Visibility.Hidden;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Only for one way bindings");
            }
        }

        private class HiddenWhenTrueConverter : IValueConverter
        {
            internal static readonly HiddenWhenTrueConverter Default = new HiddenWhenTrueConverter();

            private HiddenWhenTrueConverter()
            {
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? System.Windows.Visibility.Hidden : Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("Only for one way bindings");
            }
        }
    }
}
