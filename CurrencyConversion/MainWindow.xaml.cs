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

namespace CurrencyConversion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Regex _regex = new Regex("^[0-9]*,?[0-9]*$");
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string textInput = txtNumber.Text;

            if (!string.IsNullOrWhiteSpace(textInput) && IsTextNumber(textInput) && textInput!=",")
            {
                decimal inputNumber = decimal.Parse(textInput);
                ExceptionTxt.Text = inputNumber > (decimal)999999999.99 ? "Number exceeds maximum limit" : "";
                ExceptionTxt.Text = ExceptionTxt.Text == "" ? (decimal.Round(inputNumber, 2) != inputNumber ? "Only 2 decimal values allowed" : ""): "";

                if (ExceptionTxt.Text == "")
                {
                    outputText.Text = ConvertToWords(inputNumber);
                }
                else
                    outputText.Text = "";
            }
            else
            {
                ExceptionTxt.Text = string.IsNullOrWhiteSpace(textInput)? "Enter a number in Textbox!":"Enter only numbers and \",\" as decimal seperator in textbox!";
                outputText.Text = "";
            }
        }

        private string ConvertToWords(decimal input)
        {
            string outputString = input < 0 ? "minus " : "";

            input = Math.Abs(input);

            int dollar = (int)Math.Floor(input);
            int cents = (int)((input- dollar)*100);

            if(dollar > 0)
            {
                if(dollar == 1)
                {
                    outputString = UnitsTensHundredsConversion(dollar, outputString) + " dollar ";
                }
                else
                {
                    if (dollar / 1000000 > 0)
                    {
                        outputString = UnitsTensHundredsConversion(dollar / 1000000, outputString) + " million ";
                        dollar = dollar % 1000000;
                    }
                    if (dollar / 1000 > 0)
                    {
                        outputString = UnitsTensHundredsConversion(dollar / 1000, outputString) + " thousand ";
                        dollar = dollar % 1000;
                    }

                    outputString = UnitsTensHundredsConversion(dollar, outputString).TrimEnd('-') +" dollars ";
                }
                
            }            
            
            if(cents > 0)
            {
                outputString += outputString == "" ? "":  "and ";
                
                outputString = (cents == 1) ? UnitsTensHundredsConversion(cents, outputString) + " cent":
                                            UnitsTensHundredsConversion(cents, outputString).TrimEnd('-') + " cents";
            }

            if (outputString == "")
                outputString = "zero dollars";

            return outputString;
        }
        
        private string UnitsTensHundredsConversion(int number, string outputString)
        {
            if (number <= 0)
                return outputString;

            string[] units = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "forteen", "fifteen", "sixteen", "seventeen", "eighteen", "ninteen" }; 
            string[] tens = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninty" };

            int singleDigit = number / 100;
            if(singleDigit > 0)
            {
                outputString += units[singleDigit] + " hundred ";
                number = number % 100;
                if (number < 10 && number > 0)
                {
                    outputString += "and ";
                }
            }
            
            if (number > 19)
            {
                singleDigit = number / 10;
                outputString += tens[singleDigit] + "-";
                number = number % 10;
            }

            if(number > 0)
                outputString += units[number];
            
            return outputString;
        }
        private static bool IsTextNumber(string text)
        {
            var t= _regex.IsMatch(text);
            return t;
        }
    }
}
