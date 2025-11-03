using BMICalculator.ViewModels;
using Microsoft.Maui.Controls; 

namespace BMICalculator.Views
{

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            InitializeComponent();

            BindingContext = new ViewModel();
        }
    }
}