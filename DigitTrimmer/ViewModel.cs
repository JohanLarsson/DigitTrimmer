namespace DigitTrimmer
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged
    {
        private string? input;
        private int? digits = 0;
        private bool shiftToOrigin;
        private double? size;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Input
        {
            get => this.input;

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

        public int? Digits
        {
            get => this.digits;

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
            get => this.shiftToOrigin;

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
            get => this.size;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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

                return this.digits.HasValue
                           ? GeometryConverter.RoundDigits(text, this.digits.Value)
                           : text;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
