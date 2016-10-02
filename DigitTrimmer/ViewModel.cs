namespace DigitTrimmer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private string input;
        private int? digits = 0;
        private bool alignTopLeft;
        private double? size;
        private bool center;
        private bool noShift;
        private Thickness margin;
        private double tolerance = 0.001;
        private ToleranceType toleranceType;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ToleranceType> ToleranceTypes => Enum.GetValues(typeof(ToleranceType)).Cast<ToleranceType>();

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

        public int? Digits
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

        public bool NoShift
        {
            get
            {
                return this.noShift;
            }

            set
            {
                if (value == this.noShift)
                {
                    return;
                }

                this.noShift = value;
                this.OnPropertyChanged();
            }
        }

        public bool Center
        {
            get
            {
                return this.center;
            }

            set
            {
                if (value == this.center)
                {
                    return;
                }

                this.center = value;
                this.OnPropertyChanged();
            }
        }

        public bool AlignTopLeft
        {
            get
            {
                return this.alignTopLeft;
            }

            set
            {
                if (value == this.alignTopLeft)
                {
                    return;
                }

                this.alignTopLeft = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public Thickness Margin
        {
            get
            {
                return this.margin;
            }

            set
            {
                if (value.Equals(this.margin))
                {
                    return;
                }

                this.margin = value;
                this.OnPropertyChanged();
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

        public double Tolerance
        {
            get
            {
                return this.tolerance;
            }

            set
            {
                if (value.Equals(this.tolerance))
                {
                    return;
                }

                this.tolerance = value;
                this.OnPropertyChanged();
            }
        }

        public ToleranceType ToleranceType
        {
            get
            {
                return this.toleranceType;
            }

            set
            {
                if (value == this.toleranceType)
                {
                    return;
                }

                this.toleranceType = value;
                this.OnPropertyChanged();
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
                if (this.alignTopLeft)
                {
                    text = GeometryConverter.ShiftToTopLeft(text);
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
