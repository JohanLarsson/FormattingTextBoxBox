namespace FormattinTextBoxBox
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class FormattingTextBox : TextBox
    {
        private readonly List<TextChangedEventArgs> changes = new List<TextChangedEventArgs>();
        private bool ignoreChange;

        public FormattingTextBox()
        {
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.ignoreChange = true;
            this.Text = this.Text.Trim('#');
            this.RaiseEvent(new TextChangedEventArgs(TextChangedEvent, UndoAction.Clear));
            foreach (var change in this.changes)
            {
                this.RaiseEvent(change);
            }

            this.ignoreChange = false;
            base.OnPreviewGotKeyboardFocus(e);
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.ignoreChange = true;
            this.Text = $"#{this.Text}#";
            this.ignoreChange = false;
            base.OnPreviewLostKeyboardFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!this.ignoreChange)
            {
                if (e.UndoAction == UndoAction.Clear)
                {
                    this.changes.Clear();
                }
                else
                {
                    this.changes.Add(e);
                }
            }

            base.OnTextChanged(e);
        }
    }
}
