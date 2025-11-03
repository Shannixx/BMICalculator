using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls; 
using BMICalculator.Views;

namespace BMICalculator.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        // --- 1. Bindable Properties (Input) ---
        private double _weight;
        public double Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private double _heightCm;
        public double HeightCm
        {
            get => _heightCm;
            set => SetProperty(ref _heightCm, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private string _gender = "Male"; // Default gender
        public string Gender
        {
            get => _gender;
            set
            {
                // When gender changes, we notify the UI to refresh the image source
                if (SetProperty(ref _gender, value))
                {
                    OnPropertyChanged(nameof(GenderImageSource));
                }
            }
        }

        // --- 2. Computed/Derived Properties (Output & UI) ---
        private double _bmiResult;
        public double BmiResult
        {
            get => _bmiResult;
            set => SetProperty(ref _bmiResult, value);
        }

        private string _bmiCategory = string.Empty;
        public string BmiCategory
        {
            get => _bmiCategory;
            set => SetProperty(ref _bmiCategory, value);
        }

       
        public string GenderImageSource => Gender == "Male" ? "male.png" : "female.png";

        // --- 3. Commands (Actions) ---
        public ICommand CalculateCommand { get; }
        public ICommand SetGenderCommand { get; }

        public ViewModel()
        {
            // Initialize commands
            CalculateCommand = new Command(async () => await CalculateBmiAsync(), CanCalculate);
            SetGenderCommand = new Command<string>((g) => Gender = g);

            // Subscribe to property changes to control the Calculate button's state
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Weight) || e.PropertyName == nameof(HeightCm) || e.PropertyName == nameof(Age))
                {
                    ((Command)CalculateCommand).ChangeCanExecute();
                }
            };
        }

        // --- 4. Core Logic ---
        private bool CanCalculate()
        {
            return Weight > 0 && HeightCm > 0 && Age > 0;
        }

        private async Task CalculateBmiAsync()
        {
            if (!CanCalculate()) return;

            double heightM = HeightCm / 100.0;
            double bmi = Weight / (heightM * heightM);
            BmiResult = Math.Round(bmi, 1);

            string category;
            if (BmiResult < 18.5) category = "Underweight";
            else if (BmiResult < 25) category = "Normal";
            else if (BmiResult < 30) category = "Overweight";
            else category = "Obese";

            BmiCategory = category;

           
            await Application.Current.MainPage.Navigation.PushAsync(new ResultPage(this));
        }


        // --- 5. INotifyPropertyChanged Implementation ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
