using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FormattinTextBoxBox
{
    using System.Windows.Controls;

    public class FormattingTextBox : TextBox
    {
        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "FormattedText",
            typeof (string),
            typeof (FormattingTextBox),
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
            this.textDrawingVisual = this.NestedChildren().OfType<DrawingVisual>().SingleOrDefault();
            var drawingGroup = this.textDrawingVisual.Drawing;
            this.FormattedText = $"#{this.Text}#";
            base.OnPreviewLostKeyboardFocus(e);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            //this.textDrawingVisual.
            base.OnPreviewGotKeyboardFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!this.IsKeyboardFocused)
            {
                this.FormattedText = $"#{this.Text}#";
            }

            base.OnTextChanged(e);
        }
    }
}
