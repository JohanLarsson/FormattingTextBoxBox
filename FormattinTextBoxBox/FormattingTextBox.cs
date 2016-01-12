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

        private bool isInitialized;

        public string FormattedText
        {
            get { return (string)this.GetValue(FormattedTextProperty); }
            private set { this.SetValue(FormattedTextPropertyKey, value); }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (!this.isInitialized)
            {
                var scrollViewer = this.NestedChildren().OfType<ScrollViewer>().Single();
                var textBlock = new TextBlock { Margin = new Thickness(2, 0, 2, 0) };
                var formattedTextBinding = new Binding
                {
                    Path = new PropertyPath(FormattedTextProperty),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };
                BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, formattedTextBinding);

                var binding = new Binding
                {
                    Path = new PropertyPath(IsKeyboardFocusWithinProperty),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = new FocusedContentConverter(scrollViewer.Content, textBlock)
                };
                BindingOperations.SetBinding(scrollViewer, ContentControl.ContentProperty, binding);
                this.isInitialized = true;
            }
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.FormattedText = $"#{this.Text}#";
            base.OnTextChanged(e);
        }

        private class FocusedContentConverter : IValueConverter
        {
            private readonly object whenFocused;
            private readonly TextBlock whenNotFocused;

            public FocusedContentConverter(object whenFocused, TextBlock whenNotFocused)
            {
                this.whenFocused = whenFocused;
                this.whenNotFocused = whenNotFocused;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Equals(value, true) ? this.whenFocused : this.whenNotFocused;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
