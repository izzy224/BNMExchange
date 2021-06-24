using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_BNM_CURS.Utilities.Logic;

namespace WPF_BNM_CURS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _leftIndex;
        private int _rightIndex;
        private decimal _leftValue;
        private decimal _rightValue;

        public BOValCurs AppContext;
        public MainWindow()
        {
            InitializeComponent();
            ExchangeDatePicker.SelectedDate = DateTime.Now;
            ExchangeDatePicker.DisplayDateEnd = DateTime.Now;
        }

        private void ExchangeDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LeftMenu.SelectionChanged -= LeftMenu_SelectionChanged;
            RightMenu.SelectionChanged -= RightMenu_SelectionChanged;
            _leftIndex = 0;
            _rightIndex = 0;
            AppContext = new BOValCurs(ExchangeDatePicker.SelectedDate.Value);
            LeftMenu.Items.Clear();
            RightMenu.Items.Clear();
            foreach (var x in AppContext.Valutes)
            {
                LeftMenu.Items.Add(new ListBoxItem() { Content = $"{x.Name} {x.Value}" });
                RightMenu.Items.Add(new ListBoxItem() { Content = $"{x.Name} {x.Value}" });
            }
            LeftMenu.SelectionChanged += LeftMenu_SelectionChanged;
            RightMenu.SelectionChanged += RightMenu_SelectionChanged;
        }

        private void LeftMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _leftIndex = LeftMenu.SelectedIndex;
            LabelLeftText.Content = AppContext.Valutes[_leftIndex].Name + " este egal cu";
            ConverterLeftText.Content =  AppContext.Valutes[_leftIndex].Name;

            //LeftTextBox_TextChanged(this, new TextChangedEventArgs(null, UndoAction.None)); Have to find something in order for it to work
            LeftTextBox.Text = LeftTextBox.Text + " ";
            LeftTextBox.Text = LeftTextBox.Text.Remove(LeftTextBox.Text.Length - 1, 1);
        }
        private void RightMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rightIndex = RightMenu.SelectedIndex;
            LabelRightText.Content = AppContext.Valutes[_rightIndex].Name;
            ConverterRightText.Content = AppContext.Valutes[_rightIndex].Name;

            //RightTextBox_TextChanged(this, new TextChangedEventArgs(null, UndoAction.None));

            RightTextBox.Text = RightTextBox.Text + " ";
            RightTextBox.Text = RightTextBox.Text.Remove(RightTextBox.Text.Length - 1, 1);

        }


        //So that the textbox will only handle decimal numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //This code is pretty much stolen
            var ue = e.Source as TextBox;
            Regex regex;
            if (ue.Text.Contains("."))
            {
                regex = new Regex("[^0-9]+");
            }
            else
            {
                regex = new Regex("[^0-9.]+");
            }

            e.Handled = regex.IsMatch(e.Text);
        }

        private void LeftTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(LeftTextBox.Text))
            {
                RightTextBox.TextChanged -= RightTextBox_TextChanged;

                _leftValue = Decimal.Parse(LeftTextBox.Text);
                _rightValue = Decimal.Round(_leftValue * AppContext.Valutes[_leftIndex].Value/ AppContext.Valutes[_leftIndex].Nominal / AppContext.Valutes[_rightIndex].Value * AppContext.Valutes[_rightIndex].Nominal, 3);
                RightTextBox.Text = _rightValue.ToString();
                LabelLeftValue.Content = _leftValue.ToString();
                LabelRightValue.Content = _rightValue.ToString();
                RightTextBox.TextChanged += RightTextBox_TextChanged;
            }
        }

        private void RightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RightTextBox.Text))
            {
                LeftTextBox.TextChanged -= LeftTextBox_TextChanged;
                _rightValue = Decimal.Parse(RightTextBox.Text);
                _leftValue = Decimal.Round(_rightValue * AppContext.Valutes[_rightIndex].Value / AppContext.Valutes[_rightIndex].Nominal / AppContext.Valutes[_leftIndex].Value * AppContext.Valutes[_leftIndex].Nominal, 3);
                LeftTextBox.Text = _leftValue.ToString();
                LabelLeftValue.Content = _leftValue.ToString();
                LabelRightValue.Content = _rightValue.ToString();
                LeftTextBox.TextChanged += LeftTextBox_TextChanged;
            }
        }
    }
}
