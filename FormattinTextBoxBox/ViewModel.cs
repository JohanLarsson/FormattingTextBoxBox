using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FormattinTextBoxBox
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string text = "abc";

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => GetErrors(nameof(Text)).Any();

        public string Text
        {
            get { return this.text; }
            set
            {
                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.OnPropertyChanged();
                this.OnErrorsChanged();
            }
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(Text))
            {
                yield return "Cannot be empty";
            }

            yield break;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
