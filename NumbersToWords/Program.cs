using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbersToWords
{
    // Numbers to Words console program
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string result;

            //Generates an instance of the program, so that the routine signature 
            //does not need modification to become Static
            Program p = new Program();

            Console.Write("Please enter the number for conversion: ");
            input = Console.ReadLine();

            result = p.ConvertNumbersToWords(input);
            Console.Write(result);
            Console.ReadLine();
        }

        public string ConvertNumbersToWords(string NumberString)
        {
            //String to store the text intended for output
            string output = "";
            //Storage for the text regarding cents to add to the dollar values in the output. Not used if the input is solely cents.
            string cents = "";

            //Integers to store the current dollar and cent amounts, only dollarAmount is used in the nested calls of the routine, even for cent calculations
            int dollarAmount = 0;
            int centAmount = 0;
            int thousandAmount = 0;
            int hundredAmount = 0;

            //Storage for the unique values from 0 to 19
            var units = new[]
            {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
            "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
            "EIGHTEEN", "NINETEEN"
            };

            //Storage for the unique tens values
            var tens = new[]
            {
                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
            };

            //Breaks the input into dollars and cents if a decimal is present in the input string
            if (NumberString.Contains('.'))
            {
                var values = NumberString.Split('.');

                //Attempts to convert the input dollars to an integer, returns error if not possible
                bool dollarSuccess = int.TryParse(values[0], out int dollarBool);
                if (dollarSuccess)
                {
                    dollarAmount = int.Parse(values[0]);
                }
                else
                {
                    //Only possible reasons for failure is if the input wasn't numerical or is excessively large, states such
                    output = "INVALID ENTRY (INPUT WAS EITHER NOT A NUMBER OR EXCEEDS STORAGE LIMITS)";
                    return output;
                }

                bool centSuccess = int.TryParse(values[0], out int centBool);
                if (centSuccess)
                {
                    centAmount = int.Parse(values[1]);
                }
                else
                {
                    output = "INVALID ENTRY (INPUT WAS EITHER NOT A NUMBER OR EXCEEDS STORAGE LIMITS)";
                    return output;
                }

                //Culls excess decimal places past 2 if they exist (does not do any rounding however as it wasn't deemed necessary for this task)
                if (centAmount > 99)
                {
                    int power = values[1].Length - 2;
                    centAmount = centAmount / ((int)Math.Pow(10, power));
                }
            }
            //If no decimal is allocated, the cent amount is zeroed
            else
            {
                bool dollarSuccess = int.TryParse(NumberString, out int dollarBool);
                if (dollarSuccess)
                {
                    dollarAmount = int.Parse(NumberString);
                }
                else
                {
                    output = "INVALID ENTRY (INPUT WAS EITHER NOT A NUMBER OR EXCEEDS STORAGE LIMITS)";
                    return output;
                }

                centAmount = 0;
            }

            //Returns error if number is above
            if (dollarAmount > 99999 || dollarAmount < 0 || centAmount < 0)
            {
                output = "OUT OF RANGE (NUMBERS SHOULD BE BETWEEN 0 - 99,999)";
                return output;
            }

            //Unique output if the input is just "0" or "0.00"
            if (dollarAmount == 0 && centAmount == 0)
            {
                output = "ZERO";
            }

            //Code to run if the dollar amount lies between 1000 and 99999
            if ((dollarAmount / 1000) > 0)
            {
                //Int to store the thousands number that has been divided to a level where it can work with the existing unit maps
                thousandAmount = (dollarAmount / 1000);

                //Numbers under 20 don't need additional calculation so they can used straight
                if (thousandAmount < 20)
                {
                    output += units[thousandAmount] + " THOUSAND";
                }

                // Numbers between 20 and 99 need to have two numbers combined with a hyphen between
                else if (thousandAmount >= 20)
                {
                    output += tens[thousandAmount / 10];
                    if ((thousandAmount % 10) > 0)
                    {
                        output += "-" + units[thousandAmount % 10];
                        output += " THOUSAND";
                    }

                    else
                    {
                        output += " THOUSAND";
                    }
                }
                //Takes the thousands column(s) off the integer for storing the total amount
                dollarAmount %= 1000;
            }

            //Code to run if the dollar amount lies between 100 and 999
            if ((dollarAmount / 100) > 0)
            {
                //Appends a comma to the string to make it more 'readable' if there is a number in the thousands present
                if (output != "")
                {
                    output += ", ";
                }

                //Storage for divided hundreds
                hundredAmount = (dollarAmount / 100);

                //Hundreds only have 9 possible numbers, hence upper limit of 10
                if (hundredAmount < 10)
                {
                    output += units[hundredAmount] + " HUNDRED";
                }

                //Takes the hundreds column off the integer for storing the total amount
                dollarAmount %= 100;
            }

            if ((dollarAmount > 0) || (dollarAmount == 0 && hundredAmount > 0) || (dollarAmount == 0 && thousandAmount > 0))
            {
                //Numbers under 20 don't need additional calculation so they can used straight
                if (dollarAmount < 20 && dollarAmount != 0)
                {
                    //Thousands/hundreds and tens/ones always have an "AND" separating them, this is appended if needed
                    if (output != "")
                    {
                        output += " AND ";
                    }

                    output += units[dollarAmount];
                }

                // Numbers between 20 and 99 need to have two numbers combined with a hyphen between
                else if (dollarAmount > 20 && dollarAmount < 100)
                {
                    //Thousands/hundreds and tens/ones always have an "AND" separating them, this is appended if needed
                    if (output != "")
                    {
                        output += " AND ";
                    }

                    output += tens[dollarAmount / 10];
                    if ((dollarAmount % 10) > 0)
                    {
                        output += "-" + units[dollarAmount % 10];
                    }
                }

                // Calculates the cents if present
                if (centAmount > 0)
                {
                    //Numbers under 20 don't need additional calculation so they can used straight
                    if (centAmount < 20)
                    {
                        cents += units[centAmount];
                    }

                    // Numbers between 20 and 99 need to have two numbers combined with a hyphen between
                    else
                    {
                        cents += tens[centAmount / 10];
                        if ((centAmount % 10) > 0)
                        {
                            cents += "-" + units[centAmount % 10];
                        }
                    }

                    //Adds the cents to the output string and returns
                    output += " DOLLARS AND " + cents + " CENTS";
                    return output;
                }
            }

            //Calculates the cents if no dollars are present
            if (dollarAmount == 0 && centAmount > 0)
            {
                if (centAmount < 20)
                {
                    output += units[centAmount];
                }

                else
                {
                    output += tens[centAmount / 10];
                    if ((centAmount % 10) > 0)
                    {
                        output += "-" + units[centAmount % 10];
                    }
                }
                //Appends the unit identifier and outputs the cents
                output += " CENTS";
                return output;
            }
            //If no cents are present, append the dollars and return
            output += " DOLLARS";
            return output;
        }
    }
}
