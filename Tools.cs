using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Lana_Renee_Lashes.Tools.Logger;

namespace Lana_Renee_Lashes
{
    public static class Tools
    {
        #region Class Scope Variables

        ///////
        // Defaults
        ////

        // default inactive color
        static Color defaultInactiveColor = Color.LightGray;
        // default active textbox color
        static Color defaultActiveTextBoxColor = SystemColors.InactiveCaption;

        #endregion

        #region Disable(TextBoxBase textbox)
        /// <summary>
        /// Extends a textBoxBase to disable it
        /// </summary>
        /// <param name="textBox">Textbox to disable</param>
        public static void Disable(this TextBoxBase textBox)
        {
            // greys out textbox
            textBox.BackColor = defaultInactiveColor;
            // disables textbox
            textBox.Enabled = false;

        } // end void
        #endregion

        #region Enable(this TextBoxBase textbox, string defaultValue)
        /// <summary>
        /// Extends a textBoxBase to enable it and inserts the passed default value
        /// </summary>
        /// <param name="textBox">Textbox to enable</param>
        /// <param name="defaultValue">Default value to insert into textbox</param>
        /// <param name="readOnlySetting">Read-only status of textbox</param>
        public static void Enable(this TextBoxBase textBox, string defaultValue)
        {

            // disables textbox
            textBox.Enabled = true;
            // sets backcolor back to normal
            textBox.BackColor = defaultActiveTextBoxColor;
            // sets textbox text to 0
            textBox.Text = defaultValue;

        } // end void
        #endregion

        #region Enable(this TextBoxBase textbox)
        /// <summary>
        /// Enables a passed textbox and sets it's default value to zero
        /// </summary>
        /// <param name="textBox">Textbox to enable</param>
        public static void Enable(this TextBoxBase textBox)
        {
            // enables the textbox with passed default value
            textBox.Enable("0");

        } // end void
        #endregion

        #region ToDecimal()
        /// <summary>
        /// Extends a formatted string to return a decimal value stripped of any "$" signs or ","
        /// </summary>
        /// <param name="thisString">String to convert to a stripped decimal value</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string thisString)
        {
            try
            {
                // removes any "$" symbols from the string
                string newString = thisString.Replace("$", "");
                // removes "cents" from the text (found in boxing cost)
                newString = newString.Replace("cents", "");
                // removes " hours" from the text (found in est. hours to box)
                newString = newString.Replace("hrs", ".");
                // removes " minutes" from the text (found in est. hours to box)
                newString = newString.Replace("mins", "");
                // removes any white spaces from the string
                newString = newString.Replace(" ", "");
                // removes any commas from the string
                decimal newDecimal = decimal.Parse(newString.Replace(",", ""));

                // returns the stripped decimal
                return newDecimal;
            }
            catch (Exception ex)
            {
                LogError("Failed to parse \"" + thisString + "\" into a decimal value", ex.Message);
                return -1;

            } // end try

        } // end decimal
        #endregion

        public static double ToDouble(this string thisString)
        {
            decimal decimalValue = thisString.ToDecimal();
            string decimalString = decimalValue.ToString("0.##");
            double newDouble = double.Parse(decimalString);

            return newDouble;

        }

        #region FormatCurrency(this decimal thisDecimal)
        /// <summary>
        /// Extends a decimal value to return it as a string formatted to a currency value
        /// </summary>
        /// <param name="thisDecimal">Decimal to convert to a string formatted as a currency value</param>
        /// <returns></returns>
        public static string FormatCurrency(this decimal thisDecimal)
        {
            try
            {

                // stores the decimal value formatted as a currency string
                string newCurrency;

                // if thisDecimal is equal to 0
                if (thisDecimal == 0)
                {
                    // sets newCurrency to $0
                    newCurrency = "$0";
                }
                // if thisDecimal is a negative value
                else if (thisDecimal < 0)
                {
                    // formats passed decimal value to 2 d.p. with a 0 in front and inserts the dollar sign 'after' the negative sign
                    newCurrency = thisDecimal.ToString("#.00").Insert(1, " $");
                }
                // if thisDecimal is less than 1
                else if (thisDecimal < 1)
                {
                    // formats passed decimal value to 2 d.p. with a 0 in front
                    newCurrency = "$0" + thisDecimal.ToString("#.00");
                }
                // else if thisDecimal is greater than 1
                else
                {
                    // formats passed decimal value to 2 d.p.
                    newCurrency = "$" + thisDecimal.ToString("#.00");

                } // end if

                // returns the formatted decimal value as a string
                return newCurrency;
            }
            catch (Exception ex)
            {
                // logs error message
                LogError("Failed to parse " + thisDecimal + " into a decimal value", ex.Message);
                return null;

            } // end try

        } // end string
        #endregion

        #region ToHours()
        /// <summary>
        /// Extends a double value, splits it into hours and minutes and returns the result as a string
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static string FormatHours(this double thisDouble)
        {
            try
            {
                // if the passed double value has a decimal point
                if (thisDouble.ToString().Contains("."))
                {
                    // splits the string by decimal point
                    string[] splitString = thisDouble.ToString().Split('.');
                    // if the double value had more than 1 decimal point
                    if (splitString.Length < 1)
                    {
                        // writes error to console
                        LogError("Failed to convert the value \"" + thisDouble + "\" to hours and minutes");
                        return "Error!";
                    }
                    // else if the value after the decimal point is equal to zero
                    if (double.Parse(splitString[1]) == 0)
                    {
                        // returns the estimated hours as a string
                        return splitString[0] + " hrs";

                    }
                    else if (splitString.Length >= 2)
                    {
                        // stores the hours from the split string and formats it into an "hours" string
                        string hours = splitString[0] + " hrs ";
                        // converts value after decimal point to a to a 2 digit number
                        string minutes = splitString[1];

                        // if minutes is a single digit number
                        if (minutes.Count() == 1)
                        {
                            // inserts a zero to the end of the number
                            minutes = minutes.Insert(1, "0");

                        }
                        // else if minutes length is greater than two
                        else if (minutes.Count() > 2)
                        {
                            // takes only the first two characters of the string
                            minutes = minutes.Substring(0, 2);

                        }// end if

                        // converts 2 digit decimal value into a formatted "minutes" string
                        minutes = Math.Ceiling((decimal.Parse(minutes) / 100 * 60)).ToString() + " mins";

                        // returns the passed double value as a string in hours and minutes format
                        return hours + minutes;

                    } // end if

                } // end if

                // returns the passed double as a string formatted to hours
                return thisDouble.ToString() + " hrs";
            }
            catch (Exception ex)
            {
                // writes error to console
                LogError("Couldn't convert \"" + thisDouble + "\" to hours format!", ex.Message);
                return "Error!";

            } // end try

        } // end string
        #endregion

        #region Count(this string thisString)
        /// <summary>
        /// Extends a string and returns the number of characters in it excluding white spaces
        /// </summary>
        /// <returns></returns>
        public static int Count(this string thisString)
        {
            // trims all white spaces from the string
            thisString = thisString.Trim();
            // splits this string into a character array
            char[] charArray = thisString.ToCharArray();

            // returns the length of the character array as an integer value
            return charArray.Length;

        } // end int
        #endregion

        #region ShowUsYaTips(text)
        /// <summary>
        /// Displays a tool tip at the current control
        /// </summary>
        /// <param name="control">Current Control</param>
        /// <param name="text">Desired tool tip message</param>
        /// <returns></returns>
        public static void ShowUsYaTips(this Control control, string text)
        {

            var toolTip = new ToolTip
            {
                // sets the tool tip shape to a rectangle instead of a balloon
                IsBalloon = false,
                // enables tool tip even if parent control is not active
                ShowAlways = true,
                // sets the period of time that the tool tip is shown for
                AutoPopDelay = 1600,
                // sets the period of time that the mouse pointer must remain stationary for before the tool tip is shown
                InitialDelay = 0,
                // enables tool tip animation effect
                UseAnimation = true,
                // enables tool tip fading effect
                UseFading = true
            };
            toolTip.SetToolTip(control, text);
        }
        #endregion

        #region Logger
        public static class Logger
        {

            // creates a new list to write errors to
            public static List<string> errorReportList = new List<string>();

            #region Log(message)
            /// <summary>
            /// Writes passed string to console window on a new line
            /// </summary>
            /// <param name="message"></param>
            public static void Log(string message)
            {
                Console.WriteLine(message);
            }
            #endregion

            #region LogError(string errorMessage)
            /// <summary>
            /// Writes error message to console window and error report list
            /// </summary>
            public static void LogError(string errorMessage)
            {
                try
                {
                    // writes error to error report
                    errorReportList.Add("[" + DateTime.Now + "] " + errorMessage);
                    // writes line to console window on its own line
                    Console.WriteLine("Error: " + errorMessage);
                }
                catch (Exception ex)
                {
                    // write error to console
                    Log(ex.Message);

                } // end try

            } // end void
            #endregion

            #region LogError(string errorMessage, Exception exMessage)
            /// <summary>
            /// Writes error message to error report list and console window along with passed exception message
            /// </summary>
            public static void LogError(string errorMessage, string exMessage)
            {
                try
                {
                    // writes error to error report along with exception message
                    errorReportList.Add("[" + DateTime.Now + "] " + errorMessage + ". Exception: " + exMessage);
                    // writes error to console window along with exception message
                    Console.WriteLine("Error: " + errorMessage + ". Exception: " + exMessage);
                }
                catch (Exception ex)
                {
                    // write error to console
                    Log(ex.Message);

                } // end try

            } // end void
            #endregion

            #region LogException()
            /// <summary>
            /// Writes exception message to console window and error report list
            /// </summary>
            public static void LogException(string errorMessage)
            {
                try
                {
                    // writes error to error report
                    errorReportList.Add("[" + DateTime.Now + "]" + errorMessage);
                    // writes line to console window on its own line
                    Console.WriteLine("Exception: " + errorMessage);
                }
                catch (Exception ex)
                {
                    // write error to console
                    Log(ex.Message);

                } // end try
            }

            #endregion
        }

        #endregion
    }
}
