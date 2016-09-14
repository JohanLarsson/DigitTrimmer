namespace DigitTrimmer
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private string input;
        private int digits;
        private bool shiftToOrigin;
        private double? size;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Input
        {
            get
            {
                return this.input;
            }

            set
            {
                if (value == this.input)
                {
                    return;
                }

                this.input = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public string Output => this.GetOutput();

        public int Digits
        {
            get
            {
                return this.digits;
            }

            set
            {
                if (value == this.digits)
                {
                    return;
                }

                this.digits = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public bool ShiftToOrigin
        {
            get
            {
                return this.shiftToOrigin;
            }

            set
            {
                if (value == this.shiftToOrigin)
                {
                    return;
                }

                this.shiftToOrigin = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public double? Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (value.Equals(this.size))
                {
                    return;
                }

                this.size = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetOutput()
        {
            if (string.IsNullOrWhiteSpace(this.input))
            {
                return string.Empty;
            }

            try
            {
                var text = this.input;
                if (this.shiftToOrigin)
                {
                    text = GeometryConverter.ShiftToOrigin(text);
                }

                if (this.size != null)
                {
                    text = GeometryConverter.WithSize(text, this.size.Value);
                }

                return GeometryConverter.RoundDigits(text, this.digits);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
