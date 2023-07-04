using Lana_Renee_Lashes.Lana_Renee_Lashes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Lana_Renee_Lashes.Lana_Renee_Lashes.Tools.Logger;


namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        #region Declarations

        ///////
        // Constants
        ////

        // maximum total 
        const double MAX_VALUE = 2147483647;
        // maximum cost per unit before warning
        const decimal COST_WARNING = 5.5m;
        // max hourly rate
        const int MAX_RATE = 40;
        // min hourly rate
        const int MIN_RATE = 20;
        // default box cost per unit based on past order
        const decimal DEFAULT_BOX_PRICE = 0.2632m;
        // default shipping cost per unit based on past order
        const decimal DEFAULT_SHIP_PRICE = 0.2263m;
        // default USD to AUS currency conversion rate
        const double DEFAULT_USD_TO_AUS = 1.5d;
        // default P.A. estimated boxes per hour
        const double DEFAULT_BOXES_PER_HOUR = 53.33d;

        ///////
        // Semi-Constants (Preset but will be changeable via settings later)
        ////

        // retail price per unit
        decimal retailSalePrice = 24.99m;
        // est usd to aud multiplier - this should only be a guide
        double estUsdToAusMultiplier = 1.5;

        ///////
        // Goody price break-down (Boxing company too)
        ////

        // total cost of this goody order
        double goodyCost = 0;
        // quantity of lashes from goody
        int goodyQuantity = 0;
        // price per goody lash
        decimal goodyLashCost = 2.9m;
        // estimated goody price per unit calculated from a past order
        decimal goodyPastOrderCost = 2.9309m;

        ///////
        // Olivia price break-down (Boxing company too)
        ////

        // total cost of this olivia order
        double oliviaCost = 0;
        // quantity of lashes from olivia
        int oliviaQuantity = 0;
        // price per olivia lash 
        decimal oliviaLashCost = 2;
        // estimated olivia price per unit calculated from a past order
        decimal oliviaPastOrderCost = 3.3523m;


        // total lashes ordered
        int totalQuantity = 0;

        ///////
        // Personal price break-down
        ////

        // personal assistant hourly rate
        double pA_HourlyRate = 25.00;
        // hours spent boxing
        double pA_HoursSpentBoxing = 0;
        // estimated hours it will take to box this order (based on the const int BOX_PER_HOUR taken from previous data)
        double pA_EstimatedHoursToBox = 0;
        // estimated total personal assistant cost
        decimal pA_EstimatedCost = 0m;
        // personal assistant estimated amount of boxes packed per hour
        double pA_BoxesPerHour = DEFAULT_BOXES_PER_HOUR;



        // estimated cost per unit
        decimal estCostPerUnit = 0m;
        // estimated profit per unit
        decimal estProfitPerUnit = 0m;
        // estimated sales required before profit starts
        int estSalesToProfit = 0;
        // estimated total cost
        decimal estTotalCost = 0m;
        // estimated profit
        decimal estProfit = 0m;
        // estimated profit less gst
        decimal estProfitLessGst = 0m;
        // australian gst multiplier (10%)
        double gstMultiplier = 0.1;
        // gst to pay
        decimal estGstToPay = 0m;
        // rough price per box
        decimal roughBoxCost = 0;
        // rough shipping cost per unit
        decimal roughShippingCost = 0;
        // user information


        // protects user from accidentally looping confirmation by pressing enter to exit error dialogs
        bool spamProtect = true;
        // regex pattern to match priceboxes
        //static public Regex pricePatternString = new Regex(@"^\$?(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse price regex
        //string pricePattern = pricePatternString.ToString();
        // regex pattern to match non-priceboxes
        //static public Regex numberPatternString = new Regex(@"^(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse text regex
        //string numberPattern = numberPatternString.ToString();
        // regex pattern used to match numeric digits
        static public Regex numberDigitPattern = new Regex(@"\d", RegexOptions.Compiled);
        // variable used to parse valid keys for textboxes
        string numberDigit = numberDigitPattern.ToString();

        // stores valid keys that can be typed into textboxes
        Keys[] validKeyArray = { Keys.Back, Keys.Oemcomma, Keys.OemPeriod, Keys.Decimal, Keys.ShiftKey, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Tab };
        // stores last character removed by filter
        Keys lastCharRemoved;

        // stores current index of mouse cursor
        int oldIndex = 0;


        #endregion

        public Main()
        {
            InitializeComponent();
            textBoxGoodyCost.Focus();
            textBoxGoodyCost.SelectionStart = 1;
        }

        #region CreateAccount_KeyUp Event
        /// <summary>
        /// When any key is lifted, checks if all controls are valid to enable/disable "Create Account" button and displays tool tips to assist user with validation process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {

                // if the active control of this form is a textbox
                if (ActiveControl is TextBoxBase)
                {

                    // unlocks the proper textbox properties
                    TextBox textBox = ActiveControl as TextBox;
                    // creates a string reference of the textbox being used atm
                    string text = textBox.Text;
                    // creates a string reference of the char being used atm
                    string keyPressed = e.KeyCode.ToString();


                    // stores current index
                    oldIndex = textBox.SelectionStart;

                    // if textbox is empty
                    if (text is null or "" or "$")
                    {
                        ClearAll();
                        // resets prices
                        ClearDisplays();
                        // writes info to console
                        Log("Event handled due to null or empty textbox");
                        // breaks out of event
                        Calculupdate();

                    } // end if

                    // else if user was releasing any key other than the "enter key" and currently active control is a textbox
                    else if (!(e.KeyCode is Keys.Enter))
                    {


                        // if pressed key is a valid key defined by regex and keystrokes array
                        if (!(Regex.IsMatch(keyPressed, numberDigit) || validKeyArray.Contains(e.KeyCode)))
                        {
                            // if the user enters the same invalid character twice in a row
                            if (lastCharRemoved == e.KeyCode)
                            {
                                // informs the user that they may not use that character
                                MessageBox.Show("\"" + e.KeyCode + "\" is not a valid input!");

                            } // end if

                            // replaces keypress with nothing
                            textBox.Text = text.Remove(oldIndex - 1, 1);
                            // stores this character as the last removed character
                            lastCharRemoved = e.KeyCode;
                            return;

                        } // end if

                        // if user has selected autofill on
                        if (checkBoxAutoFill.Checked)
                        {
                            // calculates and updates values
                            Calculupdate();

                        } // end if

                    } // end if

                    SetCursor();

                } // end if

                // else if user was releasing the "enter" key and currently active  control is not a textbox
                else
                {
                    // if user releases enter once in a row
                    if (e.KeyCode == Keys.Enter && spamProtect == false && e.KeyCode != Keys.Back)
                    {
                        //sets spam protection to true to prevent double entering while trying to exit an error dialog
                        spamProtect = true;
                    }
                    // else if user presses twice in a row
                    else
                    {
                        // breaks out to prevent looping
                        spamProtect = false;
                        return;

                    } // end if

                    // if the currently active control is a checkbox
                    if (ActiveControl is CheckBox thisCheckBox)
                    {
                        // checks/unchecks current textbox depending on current state
                        thisCheckBox.Checked = thisCheckBox.Checked = false ? true : false;
                    }
                    // else if the current active control is not a checkbox
                    else
                    {
                        // calculates new textbox values and updates textboxes
                        Calculupdate();

                    } // end if

                } // end if
            }
            catch
            {
                LogError("PHere?");

            } // end try

        }
        #endregion
        /// <summary>
        /// Sets cursor back into it's correct position
        /// </summary>
        #region SetCursor()

        private void SetCursor()
        {
            TextBox textBox = ActiveControl as TextBox;

            // if textbox is displaying $0
            if (textBox.Text is "$0")
            {
                // sets cursor to the end
                textBox.SelectionStart = 2;
            }
            // else if textbox is display a $ symbol
            else if (textBox.Text is "$")
            {
                // sets cursor to the end
                textBox.SelectionStart = 1;
            }
            // if textbox text length is equal to 2
            else if (textBox.TextLength is 2)
            {
                // sets cursor to the end of the text
                textBox.SelectionStart = 2;
            }

            // else if textbox is displaying something other than $0 or nothing
            else
            {
                // reverts index from start of string back to last position
                textBox.SelectionStart = oldIndex;

            } // end if
        }
        #endregion

        #region ClearAll()
        /// <summary>
        /// Resets all values
        /// </summary>
        private void ClearAll()
        {
            // clears all goody/olivia values
            goodyCost = 0;
            oliviaCost = 0;
            goodyQuantity = 0;
            oliviaQuantity = 0;

            // clears total quantity
            totalQuantity = 0;

            // clears P.A. related values
            pA_EstimatedCost = 0;
            pA_EstimatedHoursToBox = 0;

            // clears display values
            estTotalCost = 0;
            estSalesToProfit = 0;
            estProfitPerUnit = 0;
            estProfitLessGst = 0;
            estProfit = 0;
            estCostPerUnit = 0;
            estGstToPay = 0;

            // resets defaults

            pA_BoxesPerHour = DEFAULT_BOXES_PER_HOUR;
            roughBoxCost = DEFAULT_BOX_PRICE;
            roughShippingCost = DEFAULT_SHIP_PRICE;
            estUsdToAusMultiplier = DEFAULT_USD_TO_AUS;


        } // end void
        #endregion

        #region ClearDisplays()

        /// <summary>
        /// Clears all display boxes
        /// </summary>
        private void ClearDisplays()
        {
            // clears all display boxes
            textBoxRoughBoxCost.Text = "$0" + DEFAULT_BOX_PRICE.ToString("#.00");
            textBoxRoughShippingCost.Text = "$0" + DEFAULT_SHIP_PRICE.ToString("#.00");
            textBoxEstTotalPaCost.Text = "$0";
            textBoxEstCostPerUnit.Text = "$0";
            textBoxEstProfitPerUnit.Text = "$0";
            textBoxEstSalesToProfit.Text = "0";
            textBoxTotalQuantity.Text = "0";
            textBoxEstTotalCost.Text = "$0";
            textBoxEstProfit.Text = "$0";
            textBoxEstProfitLessGst.Text = "$0";
            textBoxEstGstToPay.Text = "$0";


        } // end void

        #endregion

        #region Calculupdate()
        /// <summary>
        /// Calculates our new values and updates the textboxes with results
        /// </summary>
        private void Calculupdate()
        {
            try
            {

                TextBox[] displayBoxes = { textBoxEstTotalPaCost, textBoxEstCostPerUnit , textBoxEstProfitPerUnit, textBoxEstSalesToProfit,
                                         textBoxTotalQuantity, textBoxEstTotalCost, textBoxEstProfit, textBoxEstProfitLessGst, textBoxEstGstToPay };


                ///////
                /// Parses all values to their respective variables
                /////

                // for each control in this form
                foreach (Control control in this.Controls)
                {
                    // if control is not a textbox
                    if (!(control is TextBox))
                    {
                        // continues to next control
                        continue;
                    }

                    // sets control to a proper textbox for referencing
                    TextBox textBox = control as TextBox;
                    // creates a string reference for the name of the textbox being used
                    string textBoxName = textBox.Name;
                    // creates a string reference for the text in the textbox being used
                    string text = textBox.Text;

                    // if the current textbox is used for display only
                    if (displayBoxes.Contains(textBox))
                    {
                        // continues to next control
                        continue;

                    } // end if

                    if (!(text is null))
                    {
                        // if the text has a $ symbol in it
                        if (text.Contains("$"))
                        {
                            // replaces it with nothing to avoid calculation errors
                            text = text.Replace("$", "");

                        } // end if

                        if (text is "")
                        {
                            text = "0";
                        }

                        switch (textBoxName)
                        {
                            // textbox good cost 
                            case "textBoxGoodyCost":
                                // stores user input to goodyCost variable
                                goodyCost = double.Parse(text);

                                break;

                            case "textBoxOliviaCost":
                                // stores user input to oliviaCost variable
                                oliviaCost = double.Parse(text);

                                break;

                            // textbox good cost 
                            case "textBoxGoodyQuantity":
                                // stores user input to goodyCost variable
                                goodyQuantity = int.Parse(text);

                                break;

                            case "textBoxOliviaQuantity":
                                // stores user input to olivia quantity variable
                                oliviaQuantity = int.Parse(text);

                                break;

                            case "textBoxPaHourlyRate":
                                // stores user input to textBoxPaHourlyRate
                                pA_HourlyRate = double.Parse(text);

                                break;

                            case "textBoxPaHoursSpentBoxing":
                                // stores user input to textBox spent boxing
                                pA_HoursSpentBoxing = double.Parse(text);

                                break;

                            // textbox good cost 
                            case "textBoxPaHoursToBox":
                                // stores user input into boxes per hour variable
                                pA_EstimatedHoursToBox = double.Parse(text);

                                break;

                            case "textBoxRoughBoxCost":
                                // stores user input to rough box price variable
                                roughBoxCost = decimal.Parse(text);

                                break;

                            case "textBoxRoughShippingCost":
                                // stores user input to rough shipping cost variable
                                roughShippingCost = decimal.Parse(text);

                                break;

                            case "textBoxUsdToAud":
                                // stores user input usd to aud conversion rate (default 1.5)
                                estUsdToAusMultiplier = double.Parse(text);

                                break;

                            // if no case was matched
                            default:
                                // logs error
                                LogError("[" + DateTime.Now + "]" + "Failed to process control: " + textBox);

                                break;

                        } // end switch 

                    } // end if 

                } // end for

                // calculates total quantity since this is needed for most calculations
                totalQuantity = goodyQuantity + oliviaQuantity;

                ///////
                /// Calculate personal assistant costs
                /////

                // if personal assistant checkbox is checked AND P.A's hourly rate is not null
                if (checkBoxPaCosts.Checked)
                {
                    // if P.A. hourly rate is greater than max rate OR less than min rate
                    if (pA_HourlyRate > MAX_RATE || pA_HourlyRate < MIN_RATE)
                    {
                        // writes error message to user
                        textBoxPaHourlyRate.ShowUsYaTips("Please ensure hourly rate is valid!");

                    }

                    // roughly estimates amount of hours it will take to box this order
                    pA_EstimatedHoursToBox = goodyQuantity / pA_BoxesPerHour;

                    // if deduct hours spent checkbox is checked
                    if (checkBoxDeductHoursSpent.Checked)
                    {
                        // subtracts hours spent boxing from estimated hours to box
                        pA_EstimatedHoursToBox -= pA_HoursSpentBoxing;

                    } // end if

                    // sets personal assistants estimated cost to hourly rate * estimated hours to box
                    pA_EstimatedCost = decimal.Parse((pA_HourlyRate * pA_EstimatedHoursToBox).ToString());

                    ///////
                    /// Calculate other costs
                    /////

                    // if checkbox estimate prices by quantity is checked
                    if (checkBoxEstPricesByQuantity.Checked)
                    {
                        // sets goody cost to an estimated value based on a past order
                        goodyCost = goodyQuantity * (double)goodyPastOrderCost;
                        // sets olivia cost to an estimated value based on a past order
                        oliviaCost = oliviaQuantity * (double)oliviaPastOrderCost;

                    } // end if

                    // if goody cost and quantity are not equal to 0 OR olivia cost and quantity are not equal to 0
                    if (goodyCost + goodyQuantity > 0 || oliviaCost + oliviaQuantity > 0)
                    {
                        // adds the cost of both orders together for a total cost
                        estTotalCost = decimal.Parse((goodyCost + oliviaCost + (double)pA_EstimatedCost).ToString());
                        // estimates the cost per lash set
                        estCostPerUnit = estTotalCost / totalQuantity;
                        // estimated profit per sale
                        estProfitPerUnit = retailSalePrice - estCostPerUnit;
                        // sales required to profit
                        estSalesToProfit = (int)estTotalCost / (int)estProfitPerUnit + 1;
                        // adds quantities of both orders together for a total quantity
                        totalQuantity = goodyQuantity + oliviaQuantity;
                        // total profit margin this order
                        estProfit = estProfitPerUnit * totalQuantity;
                        // total profit margin this order minus gst
                        estProfitLessGst = estProfit - estProfit * (decimal)gstMultiplier;
                        // gst amount
                        estGstToPay = estProfit * (decimal)gstMultiplier;

                        // rough box cost
                        roughBoxCost = totalQuantity * DEFAULT_BOX_PRICE;
                        // rough shipping cost
                        roughShippingCost = totalQuantity * DEFAULT_SHIP_PRICE;


                    } // end if

                } // end if

                Display();

            } // end try
            catch (Exception ex)
            {
                // writes error to console
                LogError(ex.Message);

            } // end try

        }

        #endregion

        #region Display()

        private void Display()
        {
            try
            {
                ///////
                /// Displays monetary values to respective textboxes in correct currency
                /////

                if (goodyCost != 0)
                {
                    // displays goody cost in USD
                    textBoxGoodyCost.Text = "$" + goodyCost.ToString("#.##");
                }
                if (oliviaCost != 0)
                {
                    // displays olivia cost in USD
                    textBoxOliviaCost.Text = "$" + oliviaCost.ToString("#.##");
                } // end if

                // displays estimated total cost in USD
                textBoxEstTotalCost.Text = "$" + (goodyCost + oliviaCost).ToString();
                if (roughBoxCost > 1)
                {
                    // displays estimated rough box cost in USD to 2.d.p
                    textBoxRoughBoxCost.Text = "$" + roughBoxCost.ToString("#.00");
                }
                else
                {
                    // displays estimated rough box cost in USD to 2.d.p
                    textBoxRoughBoxCost.Text = "$0" + roughBoxCost.ToString("#.00");

                } // end if

                // displays rough shipping cost in USD to 2.d.p.
                textBoxRoughShippingCost.Text = "$" + roughShippingCost.ToString("#.00");
                // if P.A's hours spent boxing AND est hours to box is both empty
                if (pA_HoursSpentBoxing + pA_EstimatedHoursToBox > 0)
                {
                    // display hours spent boxing to 1 d.p.
                    textBoxPaHoursSpentBoxing.Text = pA_HoursSpentBoxing.ToString("#.#");
                    // display pa's estimated hours to box to 1 d.p.
                    textBoxPaHoursToBox.Text = pA_EstimatedHoursToBox.ToString("#.#");
                }
                // display estimated p.a. cost
                textBoxEstTotalPaCost.Text = pA_EstimatedCost.ToString("c2");
                // updates estimated cost per unit
                textBoxEstCostPerUnit.Text = estCostPerUnit.ToString("c2");
                // updates estimated profit per unit
                textBoxEstProfitPerUnit.Text = (estProfitPerUnit.ToString("c2"));
                // updates estimated sales until we start to see profit to 0 d.p.
                textBoxEstSalesToProfit.Text = estSalesToProfit.ToString();
                // updates total quantity
                textBoxTotalQuantity.Text = totalQuantity.ToString();
                // updates estimated total cost
                textBoxEstTotalCost.Text = estTotalCost.ToString("c2");
                // updates estimated profit
                textBoxEstProfit.Text = estProfit.ToString("c2");
                // updates profit less gst
                textBoxEstProfitLessGst.Text = estProfitLessGst.ToString("c2");
                // updates gst to pay
                textBoxEstGstToPay.Text = estGstToPay.ToString("c2");

                // if active control is a textbox
                if (ActiveControl is TextBox)
                {
                    // creates a reference variable to the active textbox
                    TextBox textBox = ActiveControl as TextBox;

                    // 
                    if (textBox.ReadOnly)
                    {
                        return;
                    
                    } // end if

                    // if textbox text is $0
                    if (textBox.Text is "$0")
                    {
                        // sets cursor to 2
                        textBox.SelectionStart = 2;

                    } // end if

                } // end if
            }
            catch (Exception ex)
            {
                // writes error to console
                LogError(ex.Message);
            }
        }

        #endregion

        #region buttonUsdToAud_Click

        private void buttonUsdToAud_Click(object sender, EventArgs e)
        {
            // displays goody cost in AUD
            textBoxGoodyCost.Text = (goodyCost * estUsdToAusMultiplier).ToString("c2");
            // displays olivia cost in AUD
            textBoxOliviaCost.Text = (oliviaCost * estUsdToAusMultiplier).ToString("c2");
            // displays rough box cost in AUD
            textBoxRoughBoxCost.Text = (roughBoxCost * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays rough shipping cost in AUD
            textBoxRoughShippingCost.Text = (roughShippingCost * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated cost per unit in AUD
            textBoxEstCostPerUnit.Text = (estCostPerUnit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit per unit in AUD
            textBoxEstProfitPerUnit.Text = (estProfitPerUnit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated sales to profit in AUD
            textBoxEstSalesToProfit.Text = (estSalesToProfit * (decimal)estUsdToAusMultiplier).ToString();
            // displays estimated total cost in AUD
            textBoxEstTotalCost.Text = ((goodyCost + oliviaCost) * estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit in AUD
            textBoxEstProfit.Text = (estProfit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit less GST in AUD
            textBoxEstProfitLessGst.Text = (estProfitLessGst * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated GST to pay in AUD
            textBoxEstGstToPay.Text = (estGstToPay * (decimal)estUsdToAusMultiplier).ToString("c2");
        }
        #endregion

        #region checkBoxPaCosts_CheckChanged Event
        /// <summary>
        /// Shows/Hides personal assistant extras and adds/deducts cost from profits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPaCosts_CheckedChanged(object sender, System.EventArgs e)
        {
            // if P.A. checkbox is checked
            if (checkBoxPaCosts.Checked)
            {
                // show P.A. controls
                labelPaHourlyRate.Show();
                textBoxPaHourlyRate.Show();
                labelPaHoursSpent.Show();
                textBoxPaHoursSpentBoxing.Show();
                labelPaHoursToBox.Show();
                textBoxPaHoursToBox.Show();

                // resets hourly rate to it's default
                pA_HourlyRate = 25;
                // displays hourly rate again
                textBoxPaHourlyRate.Text = pA_HourlyRate.ToString("c2");
            }
            // else if P.A. checkbox is not checked
            else
            {
                // hide P.A. controls
                labelPaHourlyRate.Hide();
                textBoxPaHourlyRate.Hide();
                labelPaHoursSpent.Hide();
                textBoxPaHoursSpentBoxing.Hide();
                labelPaHoursToBox.Hide();
                textBoxPaHoursToBox.Hide();
                pA_HourlyRate = 0;

            } // end if

        }
        #endregion

        #region checkBoxRoughBox_CheckChanged Event
        /// <summary>
        /// Shows/Hides extra boxes controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxRoughBoxes_CheckedChanged(object sender, EventArgs e)
        {
            // if extra boxes checkbox is checked
            if (checkBoxRoughBoxes.Checked)
            {
                // show extra boxes controls
                labelRoughBoxCost.Show();
                textBoxRoughBoxCost.Show();
                labelRoughBoxCost.Show();
                textBoxRoughShippingCost.Show();
                Calculupdate();

            }
            // else if extra boxes checkbox is not checked
            else
            {
                // show extra boxes controls
                labelRoughBoxCost.Hide();
                textBoxRoughBoxCost.Hide();
                labelRoughShippingCost.Hide();
                textBoxRoughShippingCost.Hide();

            } // end if
        }

        #endregion

        #region linkLabelUsdToAud_LinkClicked Event
        /// <summary>
        /// Click me! link to check usd to aud conversion rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelUsdToAud_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // sets website address to currency comparison on google
            string url = "https://www.google.com/search?q=usd+to+aud&ei=7yubZKOKEfOK2roPo4qSoAI&ved=0ahUKEwjjvY_Zi-T_AhVzhVYBHSOFBCQQ4dUDCBA&uact=5&oq=usd+to+aud&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQAzINCAAQigUQsQMQgwEQQzIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDoKCAAQRxDWBBCwAzoKCAAQigUQsAMQQzoHCAAQigUQQ0oECEEYAFD7BFiTDmD1DmgBcAF4AIABoAKIAdQKkgEFMC4yLjSYAQCgAQHAAQHIAQo&sclient=gws-wiz-serp";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }
        #endregion

        #region linkLabelLanaReneeLashes_LinkClicked Event
        /// <summary>
        /// Link to lane renee lashes website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelLanaReneeLashes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // sets website address to lana renee lashes home
            string url = "http://lanareneelashes.com";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }
        #endregion

        #region labelSubTitle_MouseEnter()
        /// <summary>
        ///changes subtitle color to blue when mouse enters region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSubTitle_MouseEnter(object sender, EventArgs e)
        {
            //changes subtitle color to blue
            labelSubTitle.ForeColor = Color.CadetBlue;
        }
        #endregion

        #region labelSubTitle_MouseLeave()
        /// <summary>
        ///changes subtitle color back to white when mouse exits region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSubTitle_MouseLeave(object sender, EventArgs e)
        {
            //changes subtitle color back to white
            labelSubTitle.ForeColor = Color.White;
        }
        #endregion

        #region labelSubTitle_Click()
        private void labelSubTitle_Click(object sender, EventArgs e)
        {
            // sets website address to currency comparison on google
            string url = "https://www.google.com/search?q=calc&source=hp&ei=wqObZIrLLqCw4-EPq-qA-Ao&iflsig=AOEireoAAAAAZJux0siSA2TGgXYq7dNvvdXoMeRk0TFQ&ved=0ahUKEwiKtKr8_eT_AhUg2DgGHSs1AK8Q4dUDCAs&uact=5&oq=calc&gs_lcp=Cgdnd3Mtd2l6EAMyDQgAEIoFELEDEIMBEEMyBwguEIoFEEMyBwgAEIoFEEMyDQgAEIoFELEDEIMBEEMyDQgAEIoFELEDEIMBEEMyBwgAEIoFEEMyDQgAEIoFELEDEIMBEEMyEQguEIAEELEDEIMBEMcBENEDMgsIABCABBCxAxCDATILCAAQgAQQsQMQgwE6FwgAEIoFEOoCELQCEIoDELcDENQDEOUCOggIABCABBCxAzoLCAAQigUQsQMQgwFQ4gJYmAVg3AVoAXAAeACAAc0BiAHtApIBBTAuMS4xmAEAoAEBsAEK&sclient=gws-wiz";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }

        #endregion

        private void checkBoxEstPricesByQuantity_CheckedChanged(object sender, EventArgs e)
        {
            // if checkbox is checked
            if (checkBoxEstPricesByQuantity.Checked)
            {
                // calculates values
                Calculupdate();
            }
            // else if checkbox is not checked
            else
            {
                // resets price values
                goodyCost = 0;
                oliviaCost = 0;
                // resets price displays
                textBoxGoodyCost.Text = "$0";
                textBoxOliviaCost.Text = "$0";

            } // end if

        } // end void 

        private void checkBoxDeductHoursSpent_CheckedChanged(object sender, EventArgs e)
        {
            Calculupdate();
        }
    }
    namespace Lana_Renee_Lashes
    {
        public static class Tools
        {
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

                #region LogError()
                /// <summary>
                /// Writes error message to console window and error report list
                /// </summary>
                public static void LogError(string errorMessage)
                {
                    try
                    {

                        // writes error to error report
                        errorReportList.Add("[" + DateTime.Now + "]" + errorMessage);
                        // writes line to console window on its own line
                        Console.WriteLine("Error: " + errorMessage);
                    }
                    catch (Exception ex)
                    {
                        // write error to console
                        Log(ex.Message);

                    } // end try
                }

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
}

