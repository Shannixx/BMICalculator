using Microsoft.Maui.Controls;
using BMICalculator.ViewModels;

namespace BMICalculator.Views
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage(ViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm; 
        }

        private async void OnRecalculateClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}