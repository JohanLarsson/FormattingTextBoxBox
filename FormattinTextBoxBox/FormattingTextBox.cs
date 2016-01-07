using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FormattinTextBoxBox
{
    using System.Windows.Controls;

    public class FormattingTextBox : TextBox
    {
        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "FormattedText",
            typeof(string),
            typeof(FormattingTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FormattedTextProperty = FormattedTextPropertyKey.DependencyProperty;
        private DrawingVisual textDrawingVisual;

        static FormattingTextBox()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattingTextBox), new FrameworkPropertyMetadata(typeof(FormattingTextBox)));
        }

        public string FormattedText
        {
            get { return (string)this.GetValue(FormattedTextProperty); }
            private set { this.SetValue(FormattedTextPropertyKey, value); }
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            DrawFormatted();
            base.OnPreviewLostKeyboardFocus(e);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.textDrawingVisual = this.NestedChildren().OfType<DrawingVisual>().Single();
            using (var drawingContext = textDrawingVisual.RenderOpen())
            {
                var formattedText = GetFormattedtext(Text);
                drawingContext.DrawText(formattedText, new Point(0, 0));
            }

            base.OnPreviewGotKeyboardFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.FormattedText = $"#{this.Text}#";
            if (IsVisible && !IsKeyboardFocused)
            {
                DrawFormatted();
            }
            base.OnTextChanged(e);
        }

        private void DrawFormatted()
        {
            this.textDrawingVisual = this.NestedChildren().OfType<DrawingVisual>().Single();
            using (var drawingContext = textDrawingVisual.RenderOpen())
            {
                var formattedText = GetFormattedtext(FormattedText);
                drawingContext.DrawText(formattedText, new Point(0, 0));
            }
        }

        private FormattedText GetFormattedtext(string text)
        {
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var textFormattingMode = (TextFormattingMode)GetValue(TextOptions.TextFormattingModeProperty);
            var formattedText = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection,
                typeface,
                FontSize, Foreground, null, textFormattingMode);
            return formattedText;
        }
    }
}
