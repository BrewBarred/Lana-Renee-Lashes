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
                // removes any commas from the string
                decimal newDecimal = decimal.Parse(newString.Replace(",", ""));
                // returns the stripped decimal
                return newDecimal;
            }
            catch (Exception ex)
            {
                LogError("Failed to parse " + thisString + " into a decimal value");
                Log(ex.Message);
                return -1;

            } // end try

        } // end void
        #endregion

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
                string newCurrency = "$" + thisDecimal.ToString("#.##");
                // returns the formatted decimal value as a string
                return newCurrency;
            }
            catch (Exception ex)
            {
                // logs error message
                LogError("Failed to parse " + thisDecimal + " into a decimal value", ex.Message);
                return null;

            } // end try

        } // end void
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
