using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Lana_Renee_Lashes.Tools.Logger;


namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        #region Declarations


        // maximum cost per unit before warning
        const double COST_WARNING = 4.5;
        // max hourly rate
        const int MAX_RATE = 40;
        // min hourly rate
        const int MIN_RATE = 20;
        // estimated box cost per unit
        const decimal EST_BOX_PRICE = 0.2632m;
        // estimated shipping cost per unit
        const decimal EST_SHIP_PRICE = 0.2263m;
        // retail price per unit
        decimal RETAIL_PRICE = 24.99m;
        // est usd to aud multiplier - this should only be a guide
        double estUsdToAusMultiplier = 1.5;


        // total cost of this goody order
        decimal goodyCost = 0m;
        // quantity of lashes from goody
        int goodyQuantity = 0;
        // price per goody lash
        decimal goodyLashPrice = 2.9m;
        // estimated goody shipping price per unit
        decimal goodyPricePerUnit = 0.0309m;


        // total cost of this olivia order
        decimal oliviaCost = 0m;
        // quantity of lashes from olivia
        int oliviaQuantity = 0;
        // price per olivia lash 
        decimal oliviaLashPrice = 2;
        // price per flatpacked box (unknown)
        decimal boxPriceFlat = 0;
        // price per packed box (unknown)
        decimal boxPricePacked = 0;


        // total lashes ordered
        int totalQuantity = 0;


        // personal assistant hourly rate
        decimal pA_HourlyRate = 25.00m;
        // hours spent boxing
        double pA_HoursSpentBoxing = 0;
        // estimated hours it will take to box this order (based on the const int BOX_PER_HOUR taken from previous data)
        double pA_EstimatedHoursToBox = 0;
        // estimated total personal assistant cost
        decimal pA_EstimatedCost = 0m;
        // personal assistant estimated amount of boxes packed per hour
        double pA_BoxesPerHour = 53.33;



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
        decimal gstToPay = 0m;
        // rough price per box
        decimal roughBoxCost = 0;
        // rough shipping cost per unit
        decimal roughShippingCost = 0;
        // user information


        // AUD currency on?
        bool audCurrency = true;
        // protects user from accidentally looping confirmation by pressing enter to exit error dialogs
        bool spamProtect = true;
        // regex pattern to match priceboxes
        static public Regex pricePatternString = new Regex(@"^\$?(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse price regex
        string pricePattern = pricePatternString.ToString();
        // regex pattern to match non-priceboxes
        static public Regex numberPatternString = new Regex(@"^(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse text regex
        string numberPattern = numberPatternString.ToString();
        // regex pattern used to match numeric digits
        static public Regex numberDigitPattern = new Regex(@"\d", RegexOptions.Compiled);
        // variable used to parse valid keys for textboxes
        string numberDigit = numberDigitPattern.ToString();
        // stores valid keys that can be typed into textboxes
        Keys[] validKeyArray = { Keys.Back, Keys.Space, Keys.Oemcomma, Keys.OemPeriod, Keys.Decimal, Keys.ShiftKey };
        Keys lastCharRemoved;

        #endregion

        public Main()
        {
            InitializeComponent();
            KeyPreview = true;

            goodyQuantity = 25;
            oliviaQuantity = 30;
            goodyCost = 10;
            oliviaCost = 40;
            textBoxOliviaCost.Text = oliviaCost.ToString("c2");
            textBoxGoodyCost.Text = goodyCost.ToString("c2");
            textBoxGoodyQuantity.Text = goodyQuantity.ToString();
            textBoxOliviaQuantity.Text = oliviaQuantity.ToString();
        }

        #region CreateAccount_KeyUp Event
        /// <summary>
        /// When any key is lifted, checks if all controls are valid to enable/disable "Create Account" button and displays tool tips to assist user with validation process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            //try
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
                    // if textbox is empty
                    if (text is null or "")
                    {
                        // handles event
                        e.Handled = true;
                        // writes info to console
                        Log("Event handled due to null or empty textbox");
                        return;

                    }
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
                            }
                            // stores current index
                            int oldIndex = textBox.SelectionStart;
                            // replaces keypress with nothing
                            textBox.Text = text.Remove(oldIndex - 1, 1);
                            // resets index
                            textBox.SelectionStart = oldIndex;
                            lastCharRemoved = e.KeyCode;
                            e.Handled = true;
                            return;
                        }
                        // if user has selected autofill on
                        if (checkBoxAutoFill.Checked)
                        {
                            // calculates and updates values
                            Calculupdate();
                        } // end if

                    } // end if


                } // end if

                // else if user was releasing the "enter" key and currently active  control is not a textbox
                else
                {
                    // if user releases enter once in a row
                    if (e.KeyCode == Keys.Enter && spamProtect == false)
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
                    }
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
            //catch
            {
                LogError("PHere?");

            } // end try

        }
        #endregion

        #region Calculupdate()
        /// <summary>
        /// Calculates our new values and updates the textboxes with results
        /// </summary>
        private void Calculupdate()
        {
            try
            {

                // for each control in this form
                foreach (Control textBox in this.Controls)
                {
                    if (textBox is TextBox && !(textBox.Text is null or "" or "0"))
                    {
                        // if regex finds a match between this control and pricebox pattern
                        if (Regex.IsMatch(textBox.Text, pricePattern))
                        {
                            // stores textbox name into a shorter variable
                            string text = textBox.Name;
                            // takes a substring of the text to remove the $ symbol
                            text = text.Substring(0, 1);
                            // switch to input name
                            switch (text)
                            {
                                // textbox good cost 
                                case "textBoxGoodyCost":
                                    // stores user input to goodyCost variable
                                    goodyCost = decimal.Parse(textBoxGoodyCost.Text);

                                    break;

                                case "textBoxOliviaCost":
                                    // stores user input to oliviaCost variable
                                    oliviaCost = decimal.Parse(textBoxOliviaCost.Text);

                                    break;

                                case "textBoxPaHourlyRate":
                                    // stores user input to textBoxPaHourlyRate
                                    pA_HourlyRate = decimal.Parse(textBoxPaHourlyRate.Text);

                                    break;

                                // if no case was matched
                                default:
                                    // logs error
                                    LogError("[" + DateTime.Now + "]" + "Failed to find price box: " + textBox);
                                    break;

                            } // end switch

                        } // end if

                        // if regex finds a match between this control and pricebox pattern
                        else if (Regex.IsMatch(textBox.Text, pricePattern))
                        {
                            // stores textbox name into a shorter variable
                            string text = textBox.Name;

                            // if regex finds a match between this textbox text and text pattern
                            if (Regex.IsMatch(textBox.Text, numberPattern))
                            {
                                // switch to input name
                                switch (text)
                                {
                                    // textbox good cost 
                                    case "textBoxGoodyQuantity":
                                        // stores user input to goodyCost variable
                                        goodyQuantity = int.Parse(textBoxGoodyQuantity.Text);

                                        break;

                                    case "textBoxOliviaQuantity":
                                        // stores user input to olivia quantity variable
                                        oliviaQuantity = int.Parse(textBoxOliviaQuantity.Text);

                                        break;


                                    case "textBoxUsdToAud":
                                        // stores user input usd to aud conversion rate (default 1.5)
                                        estUsdToAusMultiplier = double.Parse(textBoxUsdToAud.Text);

                                        break;

                                    case "textBoxPaHoursSpentBoxing":
                                        // stores user input to textBox spent boxing
                                        pA_HoursSpentBoxing = double.Parse(textBoxPaHoursSpentBoxing.Text);

                                        break;

                                    // textbox good cost 
                                    case "textBoxPaBoxesPerHour":
                                        // stores user input to lashed boxed per hour variable
                                        pA_BoxesPerHour = double.Parse(textBoxPaBoxesPerHour.Text);

                                        break;

                                    case "textBoxRoughBoxPrice":
                                        // stores user input to rough box price variable
                                        roughBoxCost = totalQuantity * EST_BOX_PRICE * (decimal)estUsdToAusMultiplier;

                                        break;

                                    case "textBoxRoughShipPrice":
                                        // stores user input to rough shipping cost variable
                                        roughShippingCost = totalQuantity * EST_SHIP_PRICE * (decimal)estUsdToAusMultiplier; ;

                                        break;

                                    // if no case was matched
                                    default:
                                        // logs error
                                        LogError("[" + DateTime.Now + "]" + "Failed to find textbox: " + textBox);

                                        break;

                                } // end switch

                            } // end if

                        } // end if

                    } // end if

                } // end for


                CheckCurrency();

                // stores quantity of goody lashes
                goodyQuantity = int.Parse(textBoxGoodyQuantity.Text);
                // stores quantity of olivia lashes
                oliviaQuantity = int.Parse(textBoxOliviaQuantity.Text);

                // if personal assistant checkbox is checked AND P.A's hourly rate is not null
                if (checkBoxPaCosts.Checked)
                {

                    // if P.A. hourly greater than max rate OR less than min rate
                    if (pA_HourlyRate > MAX_RATE || pA_HourlyRate < MIN_RATE)
                    {
                        // writes error message to user
                        MessageBox.Show("Please ensure hourly rate is valid!");

                    }
                    else
                    {

                        ///////
                        /// Calculate new variables
                        /////

                        if (pA_HoursSpentBoxing != 0)
                        {
                            // sets personal assistants estimated cost to hourly rate * hours spent boxing
                            pA_EstimatedCost = pA_HourlyRate * (decimal)pA_HoursSpentBoxing;
                            // roughly estimates amount of hours it will take to box this order
                            pA_EstimatedHoursToBox = totalQuantity / pA_BoxesPerHour - pA_HoursSpentBoxing;
                        }
                        else
                        {
                            // sets personal assistants estimated cost to hourly rate * P.A.s average number of boxes packed per hour
                            pA_EstimatedCost = pA_HourlyRate * (decimal)pA_BoxesPerHour;
                            // roughly estimates amount of hours it will take to box this order
                            pA_EstimatedHoursToBox = totalQuantity / pA_BoxesPerHour;

                        } // end if

                    } // end if

                    // if total quantity adds up to more than 0
                    if (totalQuantity > 0)
                    {
                        // adds the cost of both orders together for a total cost
                        estTotalCost = (goodyCost + oliviaCost + pA_EstimatedCost) * (decimal)estUsdToAusMultiplier;
                        // estimates the cost per lash set
                        estCostPerUnit = estTotalCost / totalQuantity;
                        // estimated profit per sale
                        estProfitPerUnit = RETAIL_PRICE - estCostPerUnit;
                        // sales required to profit
                        estSalesToProfit = (int)estTotalCost / (int)RETAIL_PRICE + 1;
                        // adds quantities of both orders together for a total quantity
                        totalQuantity = goodyQuantity + oliviaQuantity;
                        //total profit margin this order
                        estProfit = estProfitPerUnit * totalQuantity;
                        //total profit margin this order minus gst
                        estProfitLessGst = (estProfit *= (decimal)gstMultiplier);
                        // gst amount
                        gstToPay = (estProfit * (decimal)gstMultiplier);
                        // if checkbox extraboxes is checked and extra boxes value is 0, 
                        checkBoxUsdToAud.Text = checkBoxUsdToAud.Text == "0" ? goodyQuantity.ToString() : checkBoxUsdToAud.Text;
                        // stores usd to aud multiplier into a variable
                        estUsdToAusMultiplier = double.Parse(textBoxUsdToAud.Text);

                    } // end if
                }

                Display();



            } // end try
            catch
            {
                MessageBox.Show("Take a video of whats happening with what values and in which textboxes plus what you are trying to achieve so I can fix it");

            } // end try

        }

        #endregion

        private void CheckCurrency()
        {

            if (goodyCost + oliviaCost > 0 && goodyQuantity + oliviaQuantity > 0)
            {
                if (!audCurrency)
                {
                    // converts total from olivia to usd if the check aud checkbox is ticked
                    oliviaCost = checkBoxUsdToAud.Checked ? oliviaCost * (decimal)estUsdToAusMultiplier : oliviaCost;
                    // converts total from goody to usd if the check aud checkbox is ticked
                    goodyCost = checkBoxUsdToAud.Checked ? goodyCost * (decimal)estUsdToAusMultiplier : goodyCost;
                    // calculates total cost
                    estTotalCost = goodyCost + oliviaCost;
                    // calculates total quantity
                    totalQuantity = goodyQuantity + oliviaQuantity;
                    // converts rough box cost per unit to aud
                    roughBoxCost = checkBoxUsdToAud.Checked ? roughBoxCost * (decimal)estUsdToAusMultiplier : roughBoxCost;
                    // converts rough ship cost per unit to aud
                    roughShippingCost = checkBoxUsdToAud.Checked ? roughShippingCost * (decimal)estUsdToAusMultiplier : roughShippingCost;
                    // sets audCurrency to true so we don't keep popping in and boosting ourselves every lap
                    audCurrency = true;

                } // end if

            } // end if
        }

        private void Display()
        {
            ///////
            /// Display Calculated values to respective textboxes
            /////


            // display hours spent boxing
            textBoxPaHoursSpentBoxing.Text = pA_HoursSpentBoxing.ToString("d2");
            // display pa's estimated hours to box
            textBoxPaBoxesPerHour.Text = pA_EstimatedHoursToBox.ToString("d2");

            // display estimated p.a. cost
            textBoxPaEstCost.Text = pA_EstimatedCost.ToString("c2");
            // updates estimated cost per unit
            textBoxEstCostPerUnit.Text = estCostPerUnit.ToString("c2");
            // updates estimated profit per unit
            textBoxEstProfitPerUnit.Text = (estProfitPerUnit.ToString("c2"));
            // updates estimated sales until we start to see profit
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
            textBoxGstToPay.Text = gstToPay.ToString("c2");
        }

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
                textBoxPaBoxesPerHour.Show();

                // resets hourly rate to it's default
                pA_HourlyRate = 25m;
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
                textBoxPaBoxesPerHour.Hide();
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
        private void checkRoughBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // if extra boxes checkbox is checked
            if (checkBoxRoughBoxes.Checked)
            {
                // show extra boxes controls
                labelRoughBoxPrice.Show();
                textBoxRoughBoxPrice.Show();
                if (totalQuantity != 0)
                {
                    // displays a rough estimate of box price per unit in the correct currency
                    textBoxRoughBoxPrice.Text = checkBoxUsdToAud.Checked ? (roughBoxCost * (decimal)estUsdToAusMultiplier).ToString("c2") : (roughBoxCost).ToString("c2");
                    // displays a rough estimate of shipping price per unit in the correct currency
                    textBoxRoughShip.Text = checkBoxUsdToAud.Checked ? (roughShippingCost * (decimal)estUsdToAusMultiplier).ToString("c2") : (roughShippingCost).ToString("c2");

                }
                // else if extra boxes checkbox is not checked
                else
                {
                    // show extra boxes controls
                    labelRoughBoxPrice.Hide();
                    textBoxRoughBoxPrice.Hide();

                } // end if

            }
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

        /// <summary>
        /// checkBoxConvert_CheckedChanged (Currency converter)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxUsdToAud_CheckedChanged(object sender, EventArgs e)
        {
            // used to reference different price boxes in the form
            string text = null;
            // if aud currency is enabled
            if (checkBoxUsdToAud.Checked)
            {
                // changes the multiplier to the figure in the usd to aud rate textbox
                estUsdToAusMultiplier = double.Parse(textBoxUsdToAud.Text);
                // audcurrency is enabled
                audCurrency = true;
            }
            // else if aud currency is not enabled
            else
            {
                // multiplier is set to 1 to return the same number
                estUsdToAusMultiplier = 1.0;
                // audcurrency is disabled
                audCurrency = false;

            } // end if
        }
    }

}
namespace Lana_Renee_Lashes
{
    public static class Tools
    {
        #region Tool Tips
        /// <summary>
        /// Displays a tool tip at the current control
        /// </summary>
        /// <param name="control">Current Control</param>
        /// <param name="title">Desired tool tip title</param>
        /// <param name="text">Desired tool tip message</param>
        /// <returns></returns>
        public static Control ShowUsYaTips(this Control control, string title, string text)
        {
            // creates a new instance of the ToolTip method
            var toolTip = new ToolTip
            {
                // sets the tool tip icon
                ToolTipIcon = ToolTipIcon.Warning,
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
            return control;
        }
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

